param(
    [Parameter()]
    [switch] $pushPackage
)

$ErrorActionPreference = "Stop"

function Set-Version(){
    $version = Get-Content version
    $splitVersion = $version.Split(".")
    $splitVersion[2] = ([int]::Parse($splitVersion[2]) + 1).ToString()
    $incrementedVersion = [string]::Join(".", $splitVersion)
    Set-Content -Path version -Value $incrementedVersion
}

$root = (Resolve-Path ../)

Push-Location "$root/src/NugetXray.Tests"
dotnet test 

Push-Location "$root/src/NugetXray"
Set-Version
dotnet publish --runtime win7-x64

rm *.nupkg
nuget pack -NoPackageAnalysis -Properties "version=$(Get-Content version)"

if($pushPackage)
{
    nuget push NugetXray*.nupkg
}

popd
popd
