Param
(
    [Parameter(Mandatory=$True, Position=1)]
    [string] $TargetAssembly,

	[Parameter(Mandatory=$True, Position=2)]
	[string] $TargetTestProj,

	[Parameter(Mandatory=$True, Position=3)]
	[string] $BuildConfiguration
)

$nugetPackagesPath = "C:\Users\$($env:USERNAME)\.nuget\packages"

$openCover = "$($nugetPackagesPath)\opencover\4.7.922\tools\OpenCover.Console.exe"
$reportGenerator = "$($nugetPackagesPath)\reportgenerator\4.7.1\tools\net47\ReportGenerator.exe"

## OpenCover to consume results of executing tests
&$openCover -target:'C:\Program Files\dotnet\dotnet.exe' -targetargs:"test $TargetTestProj --no-build --no-restore -p:Configuration=$BuildConfiguration" -output:Coverage.xml -register:user -oldStyle -filter:"+[$TargetAssembly]*"

## Generate Test Coverage report from OpenCover coverage.xml file
&$reportGenerator -reports:Coverage.xml -targetdir:CodeCoverage


