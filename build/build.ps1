param(
    [Parameter()]
    [switch] $pushPackage
)

$ErrorActionPreference = "Stop"

function Get-IncrementedVersion(){
    $version = Get-Content version
    $splitVersion = $version.Split(".")
    $splitVersion[2] = ([int]::Parse($splitVersion[2]) + 1).ToString()
    return [string]::Join(".", $splitVersion)
}

function Set-Version($version){
    Set-Content -Path version -Value $version
}

$root = (Resolve-Path ../)

Push-Location "$root/src/NugetXray.Tests"
dotnet test 

Push-Location "$root/src/NugetXray"
$version = Get-IncrementedVersion
Set-Version $version
dotnet publish --runtime win7-x64 --configuration release

rm *.nupkg,*.zip
nuget pack NugetXray.nuspec -NoPackageAnalysis -Properties "version=$version"

$zipPath = [System.IO.Path]::Combine((resolve-path .), "NugetXray.$version.zip")
Add-Type -As System.IO.Compression.FileSystem
[IO.Compression.ZipFile]::CreateFromDirectory(
    (resolve-path "bin\Debug\netcoreapp1.1\win7-x64\"), 
    $zipPath, 
    "Optimal", 
    $false)

if($pushPackage)
{
    $tag = "v$version"
    git tag $tag ; git push --tags
    ..\..\build\tools\github-release.exe release `
                               --user naeemkhedarun `
                               --repo nugetxray `
                               --tag $tag
    
    ..\..\build\tools\github-release.exe upload `
                               --user naeemkhedarun `
                               --repo nugetxray `
                               --tag $tag `
                               --name "windows-x64-nugetxray-$version" `
                               --file $zipPath

    nuget push NugetXray*.nupkg -Source https://www.nuget.org/api/v2/package
}

popd
popd
