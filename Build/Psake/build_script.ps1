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

Task Clean {
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
				
				#check for a failure file
                if (Test-Path "DOTNETMF_FS_EMULATION\WINFS\TestFailures.txt") {
                    $message = Get-Content "DOTNETMF_FS_EMULATION\WINFS\TestFailures.txt"
                    throw $message
                }
				
				#show summary
				if (Test-Path "DOTNETMF_FS_EMULATION\WINFS\TestSummary.txt") {
					Get-Content "DOTNETMF_FS_EMULATION\WINFS\TestSummary.txt"
				} else {
					"No tests found"
				}
    		}
    }
    
	Pop-Location
}

# DEPLOY Tasks

Task SetVersions {
	# import SemVer class
	$tempPath = PathFromScript "\..\PoshTypeDefinitions\SemVer.cs" 
	$semVerImpl = Get-Content $tempPath -Delimiter [Environment]::NewLine
	Add-Type -TypeDefinition $semVerImpl
	
	# build processing maps 
	# - Component Name to Version
	$mapVersions = @{}
	# - Root Path to Version
	$mapRoots = @{}
	# - Explore source tree	
    # one version for all of core
	$input = Get-Content ( PathFromScript "\..\..\Core\semver.txt" )
	$version = [SemVer]::Parse($input)
	$mapVersions.Add("core", $version)
	$mapRoots.Add( ( PathFromScript "\..\..\Core\" ) , $version)
    Write-Host "'Core' using SemVer: $version"
    # one version per feature, all minimum referenced versions defined in nuspec
	# one version per driver, all minimum referenced versions defined in nuspec
	foreach ($area in @("Features", "Hardware")) {
		Get-ChildItem ( PathFromScript "\..\..\$area\" ) -Recurse |
			Where-Object { (!$_.PsIsContainer) } |
    		Where-Object { ($_.Name -eq "semver.txt") } | 
    		ForEach-Object { 
                $dir = $_.DirectoryName;
                $component = $_.Directory.Name;
                $semVer = [SemVer]::Parse( (Get-Content $_.FullName) )			
				$mapVersions.Add($component, $semVer)
				$mapRoots.Add($dir, $semVer)
            	Write-Host "'$component' using SemVer: $semVer"
            }
    }

	# - Process every component directory
	$mapRoots.GetEnumerator() |
		ForEach-Object {
			$version = $_.Value
			Get-ChildItem ( $_.Key ) -Recurse |
				Where-Object { (!$_.PsIsContainer) } |
				Where-Object { ($_.Name -eq "AssemblyInfo.cs") -or ($_.Name -like "*.nuspec" ) } | 
				ForEach-Object { 
					SetVersionsInFile $_.FullName $version $mapVersions
				}
			
		}		
}

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

Task Release -depends Clean, SetVersions, Test, Package, ClearVersions

Task Upload {
	Get-ChildItem ($packageOutput) -Recurse | 
		Where-Object { (!$_.PsIsContainer) } |
		Where-Object { ($_.Name -like "uScoober*.nupkg") } | 
		ForEach-Object { 
			$path = $_.FullName
			
			# do a nuget list first, do not upload duplicates
			throw "Not Implemented"
			
			Write-Host "Uploading: " $path
			exec { & $nuget push $_.FullName -Verbosity detailed -NonInteractive }
		}
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

