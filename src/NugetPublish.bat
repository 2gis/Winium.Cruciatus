REM delete existing nuget packages
del *.nupkg
set NUGET=.\.nuget\nuget.exe
%NUGET% pack .\Winium.Cruciatus\Winium.Cruciatus.csproj -IncludeReferencedProjects -Prop Configuration=Release
%NUGET% push *.nupkg