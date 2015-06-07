# Psake (https://github.com/psake/psake) build script for uScoober

FormatTaskName "-------- {0} --------"
Framework 4.5.1x86

Properties {
	# tools
	$script:nuget = PathFromScript "\..\NuGet\NuGet.exe"
	
    # inputs
    [String[]] $script:sourceRoots = {};
    foreach ($area in @("Core", "Features", "Hardware")) {
        $temp = (PathFromScript "\..\..\$area\"), (PathFromScript "\..\..\$area.Validation\");
        $sourceRoots = $sourceRoots + $temp;
    }
	$script:sourceSolution = PathFromScript "\..\..\uScoober.sln"
	
	#version maps
	# - Component Name to Version
	$script:mapComponentToVersions = @{}
	# - Root Path to Version
	$script:mapRootPathToVersion = @{}
	# - Nuget published versions
	$script:mapLargestVersions = @{}

    # outputs
	$script:packageOutput = PathFromScript "\..\Artifacts"
}

Task default -depends Help

Task Help -description "Helper to display task list (and should be auto generated)" {
	'Supported development tasks are: '
	'  Clean          - Remove all intermediate and final compiler outputs.'
	'  Build          - Runs all compilers.'
	'  Rebuild        - Cleans, then Builds.'
	'  Test           - Builds, then runs tests in emulator'
	''
	'Supported deployment tasks are: '
	'  *SetVersions   - Sets versions in AssemblyInfo.cs and *.nuspec files'
	'  *ClearVersions - Restore version 0.0.0 in AssemblyInfo.cs and *.nuspec files'
	'  *Package        - Creates NuGet packages.'
	'  Release        - Cleans, Propogates version numbers, Builds, Tests, Packages'
	'  Upload         - Push uScoober packages to NuGet.org.'
	''
	'* Not intended for direct consumption.'
}

# DEVELOPMENT Tasks

Task Clean -depends ClearVersions {
    foreach ($sourceRoot in $sourceRoots) {
    	# Remove bin and obj directories
    	Get-ChildItem ($sourceRoot) -Recurse | 
    		Where-Object { ($_.PsIsContainer) } |
    		Where-Object { ($_.Name -eq "obj") -or ($_.Name -eq "bin") } | 
    		ForEach-Object { 
    			Write-Host "Removing: " $_.FullName
    			Remove-Item -LiteralPath $_.FullName -Recurse -Force
    		}
    }
}

Task Build {
    Exec { msbuild /nologo /v:m /p:Configuration=Debug /t:Build $sourceSolution }
    Exec { msbuild /nologo /v:m /p:Configuration=Release /t:Build $sourceSolution }
}

Task Rebuild -depends Clean, Build

Task Test -depends Build {
	Push-Location "..\TestRunner"
	if (!(Test-Path "DOTNETMF_FS_EMULATION\WINFS")) {
        mkdir "DOTNETMF_FS_EMULATION\WINFS" | Out-Null
	}
    
	# look for test assemblies -> pass to test host script
    foreach ($sourceRoot in $sourceRoots) {
    	Get-ChildItem ($sourceRoot) -Recurse | 
    		Where-Object { ($_.FullName -match ".*\\bin\\Release\\[\w.]+Tests.exe") } | 
    		ForEach-Object {
				# flag as Build Runner
				Get-ChildItem "DOTNETMF_FS_EMULATION\WINFS" -Include "*.*" -File -Recurse |
					ForEach-Object { $_.Delete() }
        		New-Item "DOTNETMF_FS_EMULATION\WINFS\BuildTesting.txt" -ItemType File | Out-Null
	            
				$testsAssembly = $_.FullName
				Write-Host "Testing: " $testsAssembly
				
				#we need to shell out here so we can unload our reflection context used to find dependencies...
				Exec { powershell.exe -NoProfile -ExecutionPolicy unrestricted -Command "& { . .\BuildTestRunner.ps1; LaunchTestRunner $testsAssembly; }" }
				
				#show summary
				if (Test-Path "DOTNETMF_FS_EMULATION\WINFS\TestSummary.txt") {
					Get-Content "DOTNETMF_FS_EMULATION\WINFS\TestSummary.txt"
				} else {
					throw "Test Runner did not exit gracefully for $testsAssembly"
				}
				
				#check for a failure file
                if (Test-Path "DOTNETMF_FS_EMULATION\WINFS\TestFailures.txt") {
                    $message = Get-Content "DOTNETMF_FS_EMULATION\WINFS\TestFailures.txt"
                    throw $message
                }
    		}
    }
    
	Pop-Location
}

# DEPLOY Tasks
Task ClearVersions {
    foreach ($sourceRoot in $sourceRoots) {
    	Get-ChildItem ($sourceRoot) -Recurse | 
    		Where-Object { (!$_.PsIsContainer) } |
    		Where-Object { ($_.Name -like "*.orig") } | 
    		ForEach-Object { 
    			$path = $_.FullName.Substring(0, $_.FullName.Length - 5)
    			Write-Host "Restoring: $path"
    			[System.IO.File]::Delete($path)
    			[System.IO.File]::Move($_.FullName, $path)
    		}
    }
}

Task DefineSemVer {
	# import SemVer class
	$tempPath = PathFromScript "\..\PoshTypeDefinitions\SemVer.cs" 
	$semVerImpl = Get-Content $tempPath -Delimiter [Environment]::NewLine
	Add-Type -TypeDefinition $semVerImpl
}

Task MapSourceVersions -depends DefineSemVer {
	# build processing maps - Explore the source tree	
	
    # one version for all of core
	$coreVersion = Get-Content ( PathFromScript "\..\..\Core\semver.txt" )
	$version = [SemVer]::Parse($coreVersion)
	$mapComponentToVersions.Add("core", $version)
	$mapRootPathToVersion.Add( ( PathFromScript "\..\..\Core\" ) , $version)
    Write-Host "'Core' using SemVer: $version"
	
    # one version per feature/driver, all minimum referenced versions defined in nuspec
	foreach ($area in @("Features", "Hardware")) {
		Get-ChildItem ( PathFromScript "\..\..\$area\" ) -Recurse |
			Where-Object { (!$_.PsIsContainer) } |
    		Where-Object { ($_.Name -eq "semver.txt") } | 
    		ForEach-Object { 
                $dir = $_.DirectoryName;
                $component = $_.Directory.Name;
                $semVer = [SemVer]::Parse( (Get-Content $_.FullName) )			
				$mapComponentToVersions.Add($component, $semVer)
				$mapRootPathToVersion.Add($dir, $semVer)
            	Write-Host "'$component' using SemVer: $semVer"
            }
    }
}

Task SetVersions -depends DefineSemVer, ClearVersions, MapSourceVersions {
	# - Process every component directory
	$mapRootPathToVersion.GetEnumerator() |
		ForEach-Object {
			$version = $_.Value
			Get-ChildItem ( $_.Key ) -Recurse |
				Where-Object { (!$_.PsIsContainer) } |
				Where-Object { ($_.Name -eq "AssemblyInfo.cs") -or ($_.Name -like "*.nuspec" ) } | 
				ForEach-Object { 
					SetVersionsInFile $_.FullName $version $mapComponentToVersions
				}
		}		
}

Task Package {
	if (!(Test-Path $packageOutput)) { 
		mkdir $packageOutput | Out-Null
	}
    foreach ($sourceRoot in $sourceRoots) {
    	Get-ChildItem ($sourceRoot) -Recurse | 
    		Where-Object { (!$_.PsIsContainer) } |
    		Where-Object { ($_.Name -like "*.nuspec") } | 
    		ForEach-Object { 
    			Write-Host "Create Package: " $_.FullName
    			Exec { & $nuget pack ($_.FullName) -OutputDirectory $packageOutput -Verbosity detailed -NonInteractive }
    			Write-Host ""
    		}
    }
}

Task Release -depends Clean, SetVersions, Test, Package

Task Upload -depends DefineSemVer {
	Get-ChildItem ($packageOutput) -Recurse | 
		Where-Object { (!$_.PsIsContainer) } |
		Where-Object { ($_.Name -like "uScoober*.nupkg") } | 
		ForEach-Object { 
			$namePattern = '(.+)\.(\d+\.\d+\.\d+)' # todo: include pre-release if needed
			$id = $_.BaseName -replace $namePattern , '$1'
			$fileVersion = [SemVer]::Parse( ($_.BaseName -replace $namePattern , '$2') )

			$pubVersion = GetLastPublishedVersion $id

			# do not try to upload duplicates
			if ($pubVersion -lt $fileVersion) {
				$path = $_.FullName
				Write-Host "Uploading: $id [$fileVersion] from $path"
				exec { & $nuget push $path -Verbosity detailed -NonInteractive }
			} else {
				Write-Host "NuGet has: $id [$pubVersion]"							
			}
		}
}

function GetLastPublishedVersion($packageId) {
	if (!($mapLargestVersions.Contains($packageId))) {
		# fetch last nuget published versions
		Write-Host "Querying NuGet.org feed for $packageId..."
		exec { 
			$pubVersions = & $nuget list $packageId -NonInteractive -Source "https://www.nuget.org/api/v2/"
			if ($pubVersions -ne "No packages found.") {
				$pubVersions | ForEach-Object {
					$split = $_.Split(' ')
					$id = $split[0]
					$version = [SemVer]::Parse($split[1])
					if ($mapLargestVersions.Contains($id)) {
						if ($version -gt $mapLargestVersions[$id]) {
							$mapLargestVersions[$id] = $version
						}
					} else {
						$mapLargestVersions.Add($id, $version)
					}
				}
			} 
			# It might not have been listed
			if (!($mapLargestVersions.Contains($packageId))) {
				$mapLargestVersions.Add($packageId, [SemVer]::Zero)
			}
		}
	}
	$mapLargestVersions[$packageId]
}

function PathFromScript($relativePath) {
    [System.IO.Path]::GetFullPath($psake.build_script_dir + $relativePath)
}

function SetVersionsInFile($path, $myVersion, $versionsTable) {
	Write-Host "Updating: $path [$myVersion]"
	$originalcontent = [System.IO.File]::ReadAllText($path)
	$content = $originalcontent
	$content = $content.Replace("0.0.0.0", $myVersion.ToAssemblyVersion())
	$content = $content.Replace("0.0.0-me", $myVersion)
	$content = $content.Replace("1.0.0-me", $myVersion.GetNextNonGuarenteedCompatibleVersion())
	
	$versionsTable.GetEnumerator() |
		ForEach-Object {
			$content = $content.Replace("0.0.0-" + $_.Key, $_.Value)
			$content = $content.Replace("1.0.0-" + $_.Key, $_.Value.GetNextNonGuarenteedCompatibleVersion())
		}
		
	if ($content -ne $originalcontent) {
		[System.IO.File]::WriteAllText("$path.orig", $originalcontent)
		[System.IO.File]::WriteAllText($path, $content)
    }
}

