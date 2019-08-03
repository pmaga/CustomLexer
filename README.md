## Simple lexer

A simple application that performs a lexical analysis of a given string.

## Getting Started

#### Requirements

* [.NET Core SDK 2.2.401](https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.401-macos-x64-installer)
* [Azure Storage Emulator](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409) - Windows Only. For other platforms, you must connect to a real service. Normally [Azurite](https://github.com/Azure/Azurite) can be used as the second option, but it will not work for this application, because batch operations are used that are not supported by this emulator.

### Installing

* Run Azure Storage Emulator or change the connection string to your service (`StorageConnectionString)`
* Run command `sh run.sh`
* The application will be launched on the default port 5000 (http) or 5001 (https)

#### Running the tests

To run unit tests, run the command `sh run_tests.sh`

To run benchmark, go to directory: `./tests/CustomLexer.Benchmark` and  run the command `sh run_benchmark.sh`

