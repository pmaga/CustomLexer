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



## Preliminary analysis

The application is designed to support the lexical analysis of files with a maximum size of 10MB. This means that the amount of output data can be quite large, so that they must be stored in an optimal manner. The following brief analysis shows more or less what we can expect.

The number of characters can be easily counted:

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

| N    | Input size | Number of unique records | Output size |
| ---- | ---------- | ------------------------ | ----------- |
| 1    | 6,5 Mb     | 30436                    | 516 KB      |
| 2    | 6,5 Mb     | 388039                   | 8,2 MB      |
| 3    | 6,5 Mb     | 828248                   | 21,1 MB     |

It's easy to see how the complexity of output data grows exponentially with large files and the maximum value for N parameter.

**N(1)**

![N1](https://raw.githubusercontent.com/pmaga/CustomLexer/master/docs/img_n1.png)

**N(2)**

![N2](https://raw.githubusercontent.com/pmaga/CustomLexer/master/docs/img_n2.png)

**N(3)**

![N3](https://raw.githubusercontent.com/pmaga/CustomLexer/master/docs/img_n3.png)



#### Data partitioning strategy

Considering the initial analysis, I decided to use the simple Azure Table Storage database. This solution is simple, cheap, fast and designed for handling a large amount of semi-structured data.

The data are partitioned based on the operation id that is generated for each request. In addition, statistics for specific strings are identified using Row Key. 

![N3](https://raw.githubusercontent.com/pmaga/CustomLexer/master/docs/img_tablestorage.png)

The current solution definitely requires improvements::

* the maximum key size is 1KB. This has a real effect on the size of the words that create it. It may seem that 1024 characters are a lot for 3 words (N-3), but the [longest word in English](https://en.wikipedia.org/wiki/Longest_word_in_English) consists of 189819 characters :)
* performance of batch operations must be boosted, at the moment it can insert s15k items / second.

#### Algorithm

The prepared algorithm is quite simple. In the first phase, it divides the string into two groups. The first one contains texts and numbers, the second one contains punctuation marks that end the sentence. Then, two indexes are prepared, which store pointers to specific tokens.

Based on the first index, we can iterate through all tokens in the aggregate of the instance. And based on the second index, we can check whether the preceding or following token is the end of sentence.

Then, having all the data about given string, we can put it to the dictionary in which duplicates are aggregated.

![N3](https://raw.githubusercontent.com/pmaga/CustomLexer/master/docs/img_algorithm.png)

The algorithm can be modified in many ways depending on the requirements. Having them we can try to set the perfect balance between such characteristics:
* speed
* efficiency
* readability
* CPU / memory usage

#### Benchmark

TODO