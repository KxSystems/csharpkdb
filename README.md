# C# KDB

[![Travis](https://img.shields.io/travis/com/kxsystems/csharpkdb/main)](https://travis-ci.com/github/KxSystems/csharpkdb) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=KxSystems_csharpkdb&metric=coverage)](https://sonarcloud.io/dashboard?id=KxSystems_csharpkdb) <a href="https://sonarcloud.io/dashboard?id=KxSystems_csharpkdb"><img src="https://sonarcloud.io/images/project_badges/sonarcloud-white.svg" width="125"></a>

# Introduction

The C# KDB library provides functionality for .NET applications to interface with a KDB+ process.

The library is compiled on .NET Standard 2.0 and supports running applications on

- .NET Core         2.0 - 3.1
- .NET Framework    4.6.1 - 4.8

See https://dotnet.microsoft.com/platform/dotnet-standard for further details

## Client integration

### Nuget releases

Latest release version can be downloaded from [nuget](https://www.nuget.org/packages/CSharpKDB/)

## c.cs

The main entry point for client applications is the kx.c.cs class.

Example:
```csharp
  public static void Main(string[]args)
  {
    //establish connection
    c connection = null;    
    try
    {
      connection = new c("localhost", 5001);
      connection.ReceiveTimeout = 1000;
      connection.e = System.Text.Encoding.UTF8;

      Console.WriteLine("Unicode " + connection.k("`$\"c\"$0x52616e627920426ac3b6726b6c756e64204142"));
      
      //insert some data-rows
      object [] x = new object[]
      { 
        DateTime.Now.TimeOfDay,
        "xx",
        (double)93.5,
        300,
      };

      for (int i = 0; i < 1000; ++i)
      {
        connection.k("insert", "trade", x);
      }

      //read data
      Flip result = c.td(connection.k("select sum price by sym from trade"));
      
      Console.WriteLine("cols: " + c.n(result.x));
      Console.WriteLine("rows: "+ c.n(result.y[0]));
    }
    finally
    {
      //finally close connection
      connection.Close();
    }
  }

```

## Examples

Supplied with the code is a series of code examples. The following describes each with an example of how to run from command prompt. Note minimum <a href="https://dotnet.microsoft.com/download">.NET Core SDK and Runtime</a> should be installed.

### QueryResponse Demo

Instructs the remote KDB+ process to execute 'q' code (KDB+ native language) & receives the result. The same principle can be used to execute q functions. Example of a sync request.

Prerequisite:

- a KDB+ server running on port 5001 on your machine i.e. q -p 5001

Run commands:
``` bat
  REM Need to ensure nuget dependencies have been loaded
  dotnet restore .\CSharpKdb.sln 

  REM Build Release version of Demo application
  dotnet msbuild Demos\QueryResponseDemo\QueryResponseDemo.csproj /p:Configuration="Release"

  REM Run demo application
  dotnet .\Demos\QueryResponseDemo\bin\Release\netcoreapp3.1\QueryResponseDemo.dll 
```

### Serialization Demo

Example of code that can be used to serialize/dezerialise a C# type (array of ints) to KDB+ format. 

Prerequisite:

- a KDB+ server running on port 5001 on your machine i.e. q -p 5001

Run commands:
``` bat
  REM Need to ensure nuget dependencies have been loaded
  dotnet restore .\CSharpKdb.sln 

  REM Build Release version of Demo application
  dotnet msbuild Demos\SerializationDemo\SerializationDemo.csproj /p:Configuration="Release"

  REM Run demo application
  dotnet .\Demos\SerializationDemo\bin\Release\netcoreapp3.1\SerializationDemo.dll 
```

### Feed Demo

Example of creating an update function remotely (to capture table inserts), along with table creation and population of the table.
Table population has an example of single row inserts (lower latency) and bulk inserts (better throughput and resource utilization).

Prerequisite: 

- a KDB+ server running on port 5001 on your machine i.e. q -p 5001. 

- as this example depends on a .u.upd function being defined and a table name 'mytable' pre-existing, you may wish to run the following within the KDB+ server (in normal environments, these table and function definitions should be pre-created by your KDB+ admin). 

  ``
  q).u.upd:{[tbl;row] insert[tbl](row)}
  q)mytable:([]time:`timespan$();sym:`symbol$();price:`float$();size:`long$())
  ``

Run commands:
``` bat
  REM Need to ensure nuget dependencies have been loaded
  dotnet restore .\CSharpKdb.sln 

  REM Build Release version of Demo application
  dotnet msbuild Demos\FeedDemo\FeedDemo.csproj /p:Configuration="Release"

  REM Run demo application
  dotnet .\Demos\FeedDemo\bin\Release\netcoreapp3.1\FeedDemo.dll 
```

### Subscriber Demo
Example app that subscribes to real-time updates from a table that is maintained in KDB+. 

Prerequisite: 

- a KDB+ server running on port 5001 on your machine. The instance must have the .u.sub function defined. An example of .u.sub can be found in <a href="https://github.com/KxSystems/kdb-tick">KxSystems/kdb-tick</a> which is an example tickerplant. You can execute this tickerplant process by running `q tick.q` (the default port is set to 5001).

Run commands:
``` bat
  REM Need to ensure nuget dependencies have been loaded
  dotnet restore .\CSharpKdb.sln 

  REM Build Release version of Demo application
  dotnet msbuild Demos\SubscriberDemo\SubscriberDemo.csproj /p:Configuration="Release"

  REM Run demo application
  dotnet .\Demos\SubscriberDemo\bin\Release\netcoreapp3.1\SubscriberDemo.dll 
```

## Change Record

- 2020-11-16 : Issue 2# : Reformat code 

- 2017-05-23 : Identify string[] as type 11

- 2017-04-18 : Added ssl/tls support

- 2014-01-29 : Make method n public

- 2013-12-19 : qn did not detect null guid

- 2013-12-10 : Remove retry logic on authentication fail. For kdb+2.5 and prior, 

Use 
```csharp
  B = new byte[1+u.Length];
  Connect(h,p);
  s=this.GetStream();
  J=0;w(u);
  s.Write(B,0,J);
  if(1!=s.Read(B,0,1))...
```

- 2013.09.16 : za represents -0Wd, not 0Nd

- 2013.08.20 : null val for TimeStamp -> nj

- 2012.06.07 : Fixed scoping of GUID 

- 2012.05.29 : For use with kdb+v3.0, changed handshake and added Guid. boolean v6->vt tracks type capability.

- 2012.01.26 : Refactored clamp into clampDT, for Date.DateTime()

- 2012.01.25 : rz() clamp datetime to valid range

- 2010.11.17 : Block sending new timetypes to version of kdb+ prior to v2.6 (use prior release of c.cs for older kdb+ versions)

Max buffer size (default 64kB) used for reading is now a parameter to the c constructor Date, Month, Minute, Second, KTimeSpan are now serializable, implement IComparable and have default constructors for xml serialization.

Added NULL(Type t)

- 2010/08.05 : Added KException for exceptions due to server error, authentication fail and func decode

- 2010/01.14 : Exposed static var e (Encoding) as public

- 2010/01.12 : Added support for unicode encoding, defaults to ASCII 

- 2010/01.11 : Added null checks for qn for UDTs Date,Month,Minute,Second,KTimespan

- 2010/01.04 : Added new time types (timespan->KTimespan,timestamp->DateTime), drop writing kdb+ datetime

- 2009/10.19 : Limit reads to blocks of 64kB to reduce load on kernel memory

- 2007/09.26 : 0Wz to MaxValue

- 2006/10.05 : Truncate string at null

- 2006/09.29 : NULL  c.Date class(sync with c.java)

Regarding SSL/TLS: To use self-signed certificates add them to the Local Computer Trusted Root Certification Authorities
