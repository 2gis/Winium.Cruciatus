Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
#------------------------------

Import-Module '.\setup.ps1' -Args (,('msbuild', 'nunit'))

# Build
Invoke-MSBuild $solution $msbuildProperties -Verbose

# Test
Invoke-NUnit $testFiles -Verbose
