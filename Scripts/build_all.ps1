# ADD VS2017 MSBuild to system PATH: %PROGRAM_FILES%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin
# TODO: automate this (search and add)

$msbuild = 'J:\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild'

$configuration = 'Debug'
$repository = '..\nuget\'

. $msbuild -t:Restore "-p:Configuration=$configuration" '..\Root\ObscureWare.Console.Root.Shared\ObscureWare.Console.Root.Shared.csproj'
. $msbuild -t:Build "-p:Configuration=$configuration" '..\Root\ObscureWare.Console.Root.Shared\ObscureWare.Console.Root.Shared.csproj'
. $msbuild -t:Pack "-p:Configuration=$configuration" '..\Root\ObscureWare.Console.Root.Shared\ObscureWare.Console.Root.Shared.csproj'
Move-Item  "..\Root\ObscureWare.Console.Root.Shared\bin\$configuration\*.nupkg" $repository


exit

#. $msbuild -t:Build "-p:Configuration=$configuration" '..\Root\ObscureWare.Console.Root.Framework\ObscureWare.Console.Root.Desktop.csproj'
#. $msbuild -t:Pack "-p:Configuration=$configuration" '..\Root\ObscureWare.Console.Root.Framework\ObscureWare.Console.Root.Desktop.csproj'
#Move-Item  "..\Root\ObscureWare.Console.Root.Framework\bin\$configuration\*.nupkg" $repository