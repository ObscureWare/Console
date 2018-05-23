# ADD VS2017 MSBuild to system PATH: %PROGRAM_FILES%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin
# TODO: automate this (search and add)

cd $PSScriptRoot;

# TODO: write msbuild locator
$msbuild = 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\msbuild'
#$msbuild = 'J:\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild'

$configuration = 'Debug'
$repository = '..\nuget\' # TODO: make if not exists

function Build-And-Publish([string]$projectName, [string]$basePath, [bool]$useNuget)
{
    . $msbuild -t:Restore "-p:Configuration=$configuration" "..\$basePath\$projectName\$projectName.csproj"
    . $msbuild -t:Build "-p:Configuration=$configuration" "..\$basePath\$projectName\$projectName.csproj"

    if ($useNuget)
    {
        . "nuget" Pack "..\$basePath\$projectName\$projectName.csproj" -OutputDirectory $repository -Properties "Configuration=$configuration" 

    } else {
        . $msbuild -t:Pack "-p:Configuration=$configuration" "..\$basePath\$projectName\$projectName.csproj"
        Move-Item -Force "..\$basePath\$projectName\bin\$configuration\*.nupkg" $repository
    }    
}

# Root
Build-And-Publish 'ObscureWare.Console.Root.Shared' 'Root' $False
Build-And-Publish 'ObscureWare.Console.Root.Desktop' 'Root' $True

# Operations


# Commands Engine


# Tests







