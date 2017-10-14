# OpenSubtitles.org API Client

## Usage
```
var openSubtitlesApi = new OpenSubtitlesApi();

openSubtitlesApi.LogIn("eng", "OSTestUserAgentTemp");

var subtitles = openSubtitlesApi.FindSubtitles(
    SearchMethod.Qeury,
    "Game of thrones s01e01",
    "eng");

openSubtitlesApi.DownloadSubtitle(
    subtitles[0],
    null);
```

### Note
Additionally project contains XmlRpcClient.

## Usage
```
var rpcClient = new XmlRpcClient("http://some-rpc-endpoint");
var T = rpcClient.Invoke<T>("methodName", parameters);
```
