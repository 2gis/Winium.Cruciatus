Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
#------------------------------

Import-Module '.\setup.ps1' -Args (,('msbuild', 'nunit', 'nuget'))

if (Test-Path $releaseDir)
{
    Remove-Item $releaseDir -Force -Recurse
}

New-Item -ItemType directory -Path $releaseDir | Out-Null

Write-Output "Update CHANGELOG.md"
Write-Output "Update version in Assemblies"
Write-Output "Update release notes in Nuspec file"

Pause

# Build
Invoke-MSBuild $solution $msbuildProperties -Verbose

# Test
Invoke-NUnit $testFiles -Verbose

# Create nuget-package
Invoke-NuGetPack $project $configuration $releaseDir -Verbose

Write-Output "Publish NuGet package using nuget.exe push $releaseDir\*.nupkg"
Write-Output "Add and push tag using git tag -a -m 'Version *.*.*' v*.*.*"
Write-Output "Upload and attach $releaseDir\* files to release"
