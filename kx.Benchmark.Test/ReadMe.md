# kx.Benchmark.Test

The kx.Benchmark.Test.csproj is a test library that leverage the BenchmarkDotNet framework 
to run performance-tests.

For further details see 
https://benchmarkdotnet.org/articles/overview.html

Developers can run the tests
- via command-line

## Run via dotnet command-line

 Recommendation is to run Benchmarks using the dotnet command-line with other processes stopped to avoid resource contentation.

 Open command-prompt at top level solution folder 

 1) Restore any requried nuget packages

 ``` bat
  REM Need to ensure nuget dependencies have been loaded
  dotnet restore .\CSharpKdb.sln 
  ```

 2) Build the kx.Benchmark.Test project
 Note : Build should be done against optimised Release build for accuracy
 
 ``` bat
  REM AnyCpu build
  dotnet msbuild /t:"kx_Benchmark_Test" /p:Configuration=Release /p:Platform="Any CPU"  .\CSharpKdb.sln
  ```

 3) Run benchmark tests using a specified filter

 *i)* Run all benchmarks
 
 ``` bat
 REM run tests
 dotnet .\kx.Benchmark.Test\bin\Release\netcoreapp3.1\kx.Benchmark.Test.dll -f *
 ```

 *ii)* Run a specific set of benchmarks

 ``` bat
 REM run tests
 dotnet .\kx.Benchmark.Test\bin\Release\netcoreapp3.1\kx.Benchmark.Test.dll -f *
 ```

 4) Provided tests complete successfully you should see the following output and a BenchmarkDotNet.Artifacts folder containing the report files.
 
``` cmd
// * Summary *

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.19041
Intel Core i7-6500U CPU 2.50GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.100
  [Host]     : .NET Core 3.1.9 (CoreCLR 4.700.20.47201, CoreFX 4.700.20.47203), X64 RyuJIT
  DefaultJob : .NET Core 3.1.9 (CoreCLR 4.700.20.47201, CoreFX 4.700.20.47203), X64 RyuJIT


|                                            Method | Number |        Mean |      Error |     StdDev |      Median |         Max |    Gen 0 |    Gen 1 |    Gen 2 |  Allocated |
|-------------------------------------------------- |------- |------------:|-----------:|-----------:|------------:|------------:|---------:|---------:|---------:|-----------:|
| ConnectionSerialisesAndDeserialisesByteArrayInput |   1000 |    71.78 us |   1.191 us |   1.114 us |    71.59 us |    74.32 us |   1.7090 |        - |        - |    3.64 KB |
| ConnectionSerialisesAndDeserialisesByteArrayInput |  10000 |   202.72 us |   1.135 us |   1.061 us |   202.67 us |   205.13 us |  14.6484 |        - |        - |   30.01 KB |
| ConnectionSerialisesAndDeserialisesByteArrayInput | 100000 | 1,616.86 us |  32.536 us |  36.164 us | 1,601.41 us | 1,721.33 us |  91.7969 |  91.7969 |  91.7969 |  293.68 KB |
| ConnectionSerialisesAndDeserialisesByteArrayInput | 500000 | 7,409.03 us | 144.813 us | 177.843 us | 7,327.12 us | 7,770.63 us | 335.9375 | 335.9375 | 335.9375 | 1465.54 KB |

// * Legends *
  Number    : Value of the 'Number' parameter
  Mean      : Arithmetic mean of all measurements
  Error     : Half of 99.9% confidence interval
  StdDev    : Standard deviation of all measurements
  Median    : Value separating the higher half of all measurements (50th percentile)
  Max       : Maximum
  Gen 0     : GC Generation 0 collects per 1000 operations
  Gen 1     : GC Generation 1 collects per 1000 operations
  Gen 2     : GC Generation 2 collects per 1000 operations
  Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  1 us      : 1 Microsecond (0.000001 sec)

// * Diagnostic Output - MemoryDiagnoser *


// ***** BenchmarkRunner: End *****
// ** Remained 0 benchmark(s) to run **
Run time: 00:01:38 (98.06 sec), executed benchmarks: 4

Global total time: 00:01:50 (110.53 sec), executed benchmarks: 4
// * Artifacts cleanup *
```