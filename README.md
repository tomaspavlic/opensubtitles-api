# OpenSubtitles.org API Client

![build](https://github.com/tomaspavlic/opensubtitles-api/workflows/build/badge.svg)
![downloads](https://img.shields.io/nuget/dt/Topdev.OpenSubtitles.Client)
![nuget](https://img.shields.io/nuget/v/Topdev.OpenSubtitles.Client)

> With Covid-19 pandemic requests for our API went skyrocket, our servers could not handle so much traffic, so we decided to limit API usage just to authenticated requests for User Agents. In short it means, you have to provide opensubtitles.org username and password in LogIn() method.

## Installation
```bash
## .NET CLI
dotnet add package Topdev.OpenSubtitles.Client

## Package Manager
Install-Package Topdev.OpenSubtitles.Client
```

## Usage
```csharp
var openSubtitlesApi = new OpenSubtitlesApi();

await openSubtitlesApi.LogInAsync("eng", "OSTestUserAgentTemp", "username", "password");

var subtitles = await openSubtitlesApi.FindSubtitlesAsync(
    SearchMethod.Qeury,
    "Game of thrones s01e01",
    "eng");

await openSubtitlesApi.DownloadSubtitleAsync(subtitles[0], null);
```

## XmlRpcClient

Additionally project contains XmlRpcClient.

```csharp
var rpcClient = new XmlRpcClient("http://some-rpc-endpoint");
var T = await rpcClient.InvokeAsync<T>("methodName", parameters);
```

## Donations
Please feel free to donate to me. I'm not going to force you to donate, you can use my software completely free of charge and without limitation for any purpose you want. If you really want to give something to me then you are welcome to do so. I don't expect donations, nor do I insist that you give them.

**ETH** - 22a99ed4ebe631ff87332e6bcdcc6ef5ec01289f
