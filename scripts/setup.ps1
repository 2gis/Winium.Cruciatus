Param(
    [string[]]$modules
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
#------------------------------

$configuration = 'Release'
$solution = Join-Path $PSScriptRoot '..\src\Winium.sln'
$testFiles = ,"..\src\TestApplications.Tests\WindowsFormsTestApplication.Tests\bin\$configuration\WindowsFormsTestApplication.Tests.dll"
$testFiles += "..\src\TestApplications.Tests\WpfTestApplication.Tests\bin\$configuration\WpfTestApplication.Tests.dll"
$releaseDir = Join-Path $PSScriptRoot '../Release'
$project = Join-Path $PSScriptRoot '..\src\Winium.Cruciatus\Winium.Cruciatus.csproj'

$msbuildProperties = @{
    'Configuration' = $configuration
    'Defineconstants' = 'REMOTE'
}

$env:UITestApps=(Join-Path $PSScriptRoot 'UITestApps')

$modulesUrl = 'https://raw.githubusercontent.com/skyline-gleb/dev-help/v0.2.0/psm'

if (!(Get-Module -ListAvailable -Name PsGet))
{
    (new-object Net.WebClient).DownloadString("http://psget.net/GetPsGet.ps1") | iex
}

foreach ($module in $modules)
{
    Install-Module -ModuleUrl "$modulesUrl/$module.psm1" -Update
}
