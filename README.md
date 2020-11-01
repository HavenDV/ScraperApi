# [<img src="https://res.cloudinary.com/dtlp5ycep/image/upload/v1588694340/scraperapi-icon.jpg" alt="ScraperApi" width="24"/>](https://www.scraperapi.com/) ScraperApi C# SDK

[![Language](https://img.shields.io/badge/language-C%23-blue.svg?style=flat-square)](https://github.com/HavenDV/ScraperApi/search?l=C%23&o=desc&s=&type=Code) 
[![License](https://img.shields.io/github/license/HavenDV/ScraperApi.svg?label=License&maxAge=86400)](LICENSE) 
[![Requirements](https://img.shields.io/badge/Requirements-.NET%20Standard%202.0-blue.svg)](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md) 
[![Requirements](https://img.shields.io/badge/Requirements-.NET%20Framework%204.5-blue.svg)](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md) 
[![Build Status](https://github.com/HavenDV/ScraperApi/workflows/.NET%20Core/badge.svg?branch=master)](https://github.com/HavenDV/ScraperApi/actions?query=workflow%3A%22.NET+Core%22)

This is the official C# SDK for the https://www.scraperapi.com/.

## Nuget

[![NuGet](https://img.shields.io/nuget/dt/ScraperApi.svg?style=flat-square&label=ScraperApi)](https://www.nuget.org/packages/ScraperApi/)

```
Install-Package ScraperApi
```

## Usage

```cs
using ScraperApi;

using var client = new HttpClient();
var api = new ScraperApi("your-token", client);

var response = await api.GetCurrentInformationAsync();

Console.WriteLine($"City: {response.City}");
```

## Live Example

C# .NET Fiddle - https://dotnetfiddle.net/i5MmNp  
VB.NET .NET Fiddle - https://dotnetfiddle.net/EUszSY  

## Contacts
* [mail](mailto:havendv@gmail.com)