if (!(Get-Module "psake")) {
	Import-Module .\psake.psm1
}

function psake($tasks = "Build", $buildScript = ".\build_script.ps1") {
	Clear-Host
	Invoke-psake -buildFile $buildScript -taskList $tasks
}
