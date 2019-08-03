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



#### Data partitioning strategy

The application is designed to support the lexical analysis of files with a maximum size of 10MB. This means that the amount of output data can be quite large, so that they must be stored in an optimal manner.

*10 MB = 10485760 bytes = 10485760 characters*

The average number of words per text looks as follows [[data source](https://diuna.biz/length-of-words-average-number-of-characters-in-a-word/)]

| Language | Average number of characters in one word | Number of words on one page of 1800 characters with whitespaces |
| -------- | ---------------------------------------- | ------------------------------------------------------------ |
| English  | 6.19                                     | 290.66                                                       |
| Polish   | 7.21                                     | 249.55                                                       |
| German   | 6.03                                     | 298.33                                                       |

For the maximum text size, we can therefore expect about 1693983 words. Of course, repetitions will occur, and their number depends on the input parameter N (1 - 3):

| N    | Duplicates rate | Output size |
| ---- | --------------- | ----------- |
| 1    | High            | Small       |
| 2    | Moderate        | Medium      |
| 3    | Low             | High        |



#### Test data

For a more meaningful test I used publicly available text from this [source](https://norvig.com/big.txt). 

```
The Project Gutenberg EBook of The Adventures of Sherlock Holmes
by Sir Arthur Conan Doyle
```

By running test on this data set, we get the following results:

| N    | Input size | Number of unique records | Output size (Mb) |
| ---- | ---------- | ------------------------ | ---------------- |
| 1    | 6,5 Mb     | 30436                    | 516 KB           |
| 2    | 6,5 Mb     | 388039                   | 8,2 MB           |
| 3    | 6,5 Mb     | 828248                   | 21,1 MB          |



![N1](https://raw.githubusercontent.com/pmaga/CustomLexer/master/docs/n1.png)

![N2](https://raw.githubusercontent.com/pmaga/CustomLexer/master/docs/n2.png)

![N3](https://raw.githubusercontent.com/pmaga/CustomLexer/master/docs/n3.png)







