Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
#------------------------------

Import-Module '.\setup.ps1' -Args (,('msbuild', 'nunit'))

# Build
Invoke-MSBuild $solution $msbuildProperties -Verbose

# Test
(New-Object -ComObject "Shell.Application").minimizeall()
Invoke-NUnit $testFiles -Verbose
