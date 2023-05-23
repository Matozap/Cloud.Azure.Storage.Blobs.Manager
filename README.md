# Cloud.Azure.Storage.Blobs.Manager

![Build](https://img.shields.io/github/actions/workflow/status/Matozap/Cloud.Azure.Storage.Blobs.Manager/build.yml?style=for-the-badge&logo=github&color=0D7EBF)
![Commits](https://img.shields.io/github/last-commit/Matozap/Cloud.Azure.Storage.Blobs.Manager?style=for-the-badge&logo=github&color=0D7EBF)
![Package](https://img.shields.io/nuget/dt/Cloud.Azure.Storage.Blobs.Manager?style=for-the-badge&logo=nuget&color=0D7EBF)


Cloud.Azure.Storage.Blobs.Manager is an Injectable abstraction to download, upload, move, map, read and delete blobs from Azure.


It simplifies storage usage by allowing developers to inject it into the application and use it anywhere
without having to worry about the azure client. 

### Available Methods

```csharp

public Task UploadBlobAsync(string containerName, string containerDirectory, string blobName, MemoryStream stream);

public Task MoveBlobAsync(string containerName, string sourceContainerDirectory, string sourceBlobName, string destinationDirectory);

Task<MemoryStream> DownloadBlobAsync(string containerName, string containerDirectory, string blobName);

Task<string> DownloadBlobContentAsync(string containerName, string containerDirectory, string blobName);

Task DeleteBlobAsync(string containerName, string containerDirectory, string blobName);

Task<List<string>> ListFilesAsync(string containerName, string containerDirectory);
    
```

------------------------------

### Usage

#### Injection

Just inject and use the ICache interface to store or obtain values from the cache like the example below:

```csharp

public class SomeService
{
    private readonly IBlobStorageManager _storageManager;

    public SomeService(IBlobStorageManager storageManager)
    {
        _storageManager = storageManager;
    }    
}

```


### Configuration

#### Startup/program 

Just add below lines to have the IBlobStorageManager registered in the service collection and ready to use anywhere.

```csharp
services.AddAzureBlobStorage(options =>
{
    options.WithEndpoint("<storage endpoint>");
});
```

###

## Contributing

It is simple, as all things should be:

1. Clone it
2. Improve it
3. Make pull request

## Credits

- Initial development by [Slukad](https://github.com/Slukad)
