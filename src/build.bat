cls

set toolPath=packages\FAKE\tools\Fake.exe

if exist %toolPath% goto build:

".nuget\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion" -source "https://www.nuget.org/api/v2/"

:build
%toolPath% build-solution.fsx