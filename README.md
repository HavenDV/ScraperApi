# [<img src="https://res.cloudinary.com/dtlp5ycep/image/upload/v1588694340/scraperapi-icon.jpg" alt="ScraperApi" width="24"/>](https://www.scraperapi.com/) ScraperApi C# SDK

[![Language](https://img.shields.io/badge/language-C%23-blue.svg?style=flat-square)](https://github.com/HavenDV/ScraperApi/search?l=C%23&o=desc&s=&type=Code) 
[![License](https://img.shields.io/github/license/HavenDV/ScraperApi.svg?label=License&maxAge=86400)](LICENSE) 
[![Requirements](https://img.shields.io/badge/Requirements-.NET%20Standard%202.0-blue.svg)](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md) 
[![Requirements](https://img.shields.io/badge/Requirements-.NET%20Framework%204.5-blue.svg)](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md) 
[![Build Status](https://github.com/HavenDV/ScraperApi/workflows/.NET%20Core/badge.svg?branch=main)](https://github.com/HavenDV/ScraperApi/actions?query=workflow%3A%22.NET+Core%22)

This is the official C# SDK for the https://www.scraperapi.com/.

## Getting Started
Scraper API is designed to simplify web scraping. A few things to consider before we get started:
- Each request will be retried until it can be successfully completed (up to 60 seconds). Remember to set your timeout to 60 seconds to ensure this process goes smoothly. In cases where every request fails in 60 seconds we will return a 500 error, you may retry the request and you will not be charged for the unsuccessful request (you are only charged for successful requests, 200 and 404 status codes). Make sure to catch these errors! They will occur on roughly 1-2% of requests for hard to scrape websites. You can scrape images, PDFs or other files just as you would any other URL, just remember that there is a 2MB limit per request.
- If you exceed your plan concurrent connection limit, the API will respond with a 429 status code, this can be solved by slowing down your request rate
- There is no overage allowed on the free plan, if you exceed 1000 requests per month on the free plan, you will receive a 403 error.
- Each request will return a string containing the raw html from the page requested, along with any headers and cookies.

## Nuget

[![NuGet](https://img.shields.io/nuget/dt/ScraperApi.svg?style=flat-square&label=ScraperApi)](https://www.nuget.org/packages/ScraperApi/)

```
Install-Package ScraperApi
```

## Basic Usage

Scraper API exposes a single API endpoint, simply send a GET request to http://api.scraperapi.com with two query string parameters, api_key which contains your API key, and url which contains the url you would like to scrape.

```cs
using ScraperApi;

using var client = new HttpClient();
var api = new ScraperApi("YOURAPIKEY", client);

var result = await api.GetAsync("http://httpbin.org/ip");

Console.WriteLine($"Result: {result}");
```

Result:
```html
<html>
  <head>
  </head>
  <body>
    <pre style="word-wrap: break-word; white-space: pre-wrap;">
      {"origin":"176.12.80.34"}
    </pre>
  </body>
</html>
```

## Rendering Javascript

If you would like to keep the original request headers in order to pass through custom headers (user agents, cookies, etc.), simply set keep_headers=true. Only use this feature in order to get customized results, do not use this feature in order to avoid blocks, we handle that internally.

```cs
using ScraperApi;

using var client = new HttpClient();
var api = new ScraperApi("YOURAPIKEY", client);

var result = await api.GetAsync("http://httpbin.org/ip", render: true);

Console.WriteLine($"Result: {result}");
```

Result:
```html
<html>
  <head>
  </head>
  <body>
    <pre style="word-wrap: break-word; white-space: pre-wrap;">
      {"origin":"192.15.81.132"}
    </pre>
  </body>
</html>
```

## Custom Headers

If you would like to keep the original request headers in order to pass through custom headers (user agents, cookies, etc.), simply set keep_headers=true. Only use this feature in order to get customized results, do not use this feature in order to avoid blocks, we handle that internally.

```cs
using ScraperApi;

using var client = new HttpClient();
var api = new ScraperApi("YOURAPIKEY", client);

var result = await api.GetAsync("http://httpbin.org/anything", headers: new Dictionary<string, string>
{
    { "X-MyHeader", "123" },
});

Console.WriteLine($"Result: {result}");
```

Result:
```html
<html>
  <head>
  </head>
  <body>
    <pre style="word-wrap: break-word; white-space: pre-wrap;">
    {
      "args":{},
      "data":"",
      "files":{},
      "form":{},
      "headers": {
        "Accept":"*/*",
        "Accept-Encoding":"gzip, deflate",
        "Cache-Control":"max-age=259200",
        "Connection":"close",
        "Host":"httpbin.org",
        "Referer":"http://httpbin.org",
        "Timeout":"10000",
        "User-Agent":"curl/7.54.0",
        "X-Myheader":"123"
      },
      "json":null,
      "method":"GET",
      "origin":"45.72.0.249",
      "url":"http://httpbin.org/anything"
    }
    </pre>
  </body>
</html>
```

## Sessions

To reuse the same proxy for multiple requests, simply use the &session_number= flag (e.g. session_number=123). The value of session can be any integer, simply send a new integer to create a new session (this will allow you to continue using the same proxy for each request with that session number). Sessions expire 15 minutes after the last usage.

```cs
using ScraperApi;

using var client = new HttpClient();
var api = new ScraperApi("YOURAPIKEY", client);

var result1 = await api.GetAsync("http://httpbin.org/ip", session_number: 123);
var result2 = await api.GetAsync("http://httpbin.org/ip", session_number: 123);

Console.WriteLine($"Result1: {result1}");
Console.WriteLine($"Result2: {result2}");
```

Result:
```html
<html>
  <head>
  </head>
  <body>
    <pre style="word-wrap: break-word; white-space: pre-wrap;">
      {"origin":"176.12.80.34"}
    </pre>
  </body>
</html>
<html>
  <head>
  </head>
  <body>
    <pre style="word-wrap: break-word; white-space: pre-wrap;">
      {"origin":"176.12.80.34"}
    </pre>
  </body>
</html>
```

## Geographic Location

To ensure your requests come from the United States, please use the country_code= flag (e.g. country_code=us). United States (us) geotargeting is available on the Startup plan and higher. Business plan customers also have access to Canada (ca), United Kingdom (uk), Germany (de), France (fr), Spain (es), Brazil (br), Mexico (mx), India (in), Japan (jp), China (cn), and Australia (au). Other countries are available to Enterprise customers upon request.

```cs
using ScraperApi;

using var client = new HttpClient();
var api = new ScraperApi("YOURAPIKEY", client);

var result = await api.GetAsync("http://httpbin.org/ip", country_code: "us");

Console.WriteLine($"Result: {result}");
```

Result:
```html
<html>
  <head>
  </head>
  <body>
    <pre style="word-wrap: break-word; white-space: pre-wrap;">
      {"origin":"176.12.80.34"}
    </pre>
  </body>
</html>
```

## Premium Residential/Mobile Proxy Pools

Our standard proxy pools include millions of proxies from over a dozen ISPs, and should be sufficient for the vast majority of scraping jobs. However, for a few particularly difficult to scrape sites, we also maintain a private internal pool of residential and mobile IPs. This pool is only available to users on the Business plan or higher. Requests through our premium residential and mobile pool are charged at 10 times the normal rate (every successful request will count as 10 API calls against your monthly limit), each request that uses both rendering javascript and our premium pool will be charged at 25 times the normal rate (every successful request will count as 25 API calls against your monthly limit). To send a request through our premium proxy pool, please use the premium=true flag.

```cs
using ScraperApi;

using var client = new HttpClient();
var api = new ScraperApi("YOURAPIKEY", client);

var result = await api.GetAsync("http://httpbin.org/ip", premium: true);

Console.WriteLine($"Result: {result}");
```

Result:
```html
<html>
  <head>
  </head>
  <body>
    <pre style="word-wrap: break-word; white-space: pre-wrap;">
      {"origin":"176.12.80.34"}
    </pre>
  </body>
</html>
```

## POST/PUT Requests

(BETA) Some advanced users will want to issue POST/PUT Requests in order to scrape forms and API endpoints directly. You can do this by sending a POST/PUT request through Scraper API. The return value will be stringified, if you want to use it as JSON, you will want to parse it into a JSON object.

```cs
using ScraperApi;

using var client = new HttpClient();
var api = new ScraperApi("YOURAPIKEY", client);

var postResult = await api.PostAsync("http://httpbin.org/anything", new Dictionary<string, string>
{
    { "foo", "bar" },
});
var putResult = await api.PutAsync("http://httpbin.org/anything", new Dictionary<string, string>
{
    { "foo", "bar" },
});

Console.WriteLine($"postResult: {postResult}");
Console.WriteLine($"putResult: {putResult}");
```

Result:
```html
{
  "args": {},
  "data": "{\"foo\":\"bar\"}",
  "files": {},
  "form": {},
  "headers": {
    "Accept": "application/json",
    "Accept-Encoding": "gzip, deflate",
    "Content-Length": "13",
    "Content-Type": "application/json; charset=utf-8",
    "Host": "httpbin.org",
    "Upgrade-Insecure-Requests": "1",
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko"
  },
  "json": {
    "foo": "bar"
  },
  "method": "POST",
  "origin": "191.101.82.154, 191.101.82.154",
  "url": "https://httpbin.org/anything"
}
```

## Scraper API Proxy Mode

To simplify implementation, we offer a proxy front-end to the API. The proxy will take your requests and pass them through to the API which will take care of proxy rotation, captchas and retries.
- Each request will be sent through to the API and the response returned just like the API normally would. Successful requests will return with a status code of 200, 404 or 410.
- Unsuccessful requests will return with a status code of 500. Your requests will receive a 429 error if you go over your concurrency limit and a 403 if you go over your plan's maximum requests. Make sure to catch these errors!
- You can use the proxy to scrape images, documents, PDFs or other files just as you would any other URL. Please remember that there is a 2MB limit per request.
- The username for the proxy is "scraperapi" and the password is your API key. You can pass parameters to the API by adding them to the username, separated by periods. For example, if you want to render a request, the username would be "scraperapi.render=true". If you want to geotarget a request, the username would be "scraperapi.country_code=us". Multiple parameters can be included by separating them with periods; for example: "scraperapi.country_code=us.session_number=1234".
- The proxy will accept the following parameters as part of the username:
  - render
  - country_code
  - session_number
  - keep_headers
  - premium
- Any headers you set for your proxy requests are automatically sent to the site you are scraping. You can override this using "keep_headers=false".
- So that we can properly directy your requests through the API, your code must be configured to not verify SSL certificates.

```cs
using ScraperApi;

using var client = ScraperApi.GetProxyHttpClient("YOURAPIKEY");
var api = new ScraperApi("YOURAPIKEY", client);

var result = await api.GetAsync("http://httpbin.org/ip");

Console.WriteLine($"Result: {result}");
```

## Account Information

If you would like to monitor your account usage and limits programmatically (how many concurrent requests you're using, how many requests you've made, etc.) you may use the /account endpoint, which returns JSON. Note that the requestCount and failedRequestCount numbers only refresh once every 15 seconds, while the concurrentRequests number is availble in realtime.

```cs
using ScraperApi;

using var client = new HttpClient();
var api = new ScraperApi("YOURAPIKEY", client);

var information = await api.GetAccountInformationAsync();

Console.WriteLine($"ConcurrentRequests: {information.ConcurrentRequests}");
Console.WriteLine($"RequestCount: {information.RequestCount}");
Console.WriteLine($"FailedRequestCount: {information.FailedRequestCount}");
Console.WriteLine($"RequestLimit: {information.RequestLimit}");
Console.WriteLine($"ConcurrencyLimit: {information.ConcurrencyLimit}");
```

Result:
```json
{
  "concurrentRequests":553,
  "requestCount":6655888,
  "failedRequestCount":1118,
  "requestLimit":10000000,
  "concurrencyLimit":1000
}
```

## Live Example

C# .NET Fiddle - https://dotnetfiddle.net/i5MmNp  
VB.NET .NET Fiddle - https://dotnetfiddle.net/EUszSY  

## Contacts
* [mail](mailto:havendv@gmail.com)