using System.Diagnostics.CodeAnalysis;
using Azure.Storage.Blobs.Manager.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Azure.Storage.Blobs.Manager;

[ExcludeFromCodeCoverage]
public static class AzureStorageBlobsManagerMiddleware
{
    public static IServiceCollection AddAzureBlobStorage(this IServiceCollection services, Action<BlobStorageOptions> options)
    {
        var blobStorageOptions = new BlobStorageOptions();
        options.Invoke(blobStorageOptions);

        if (string.IsNullOrEmpty(blobStorageOptions.Endpoint))
        {
            throw new ArgumentException("Azure Storage endpoint cannot be null or empty", nameof(blobStorageOptions.Endpoint))
            {
                HelpLink = null,
                HResult = 0,
                Source = "AddAzureBlobStorage service registration"
            };
        }
            
        services.AddSingleton(blobStorageOptions);
        services.AddSingleton<IBlobStorageManager, BlobStorageManager>(service => new BlobStorageManager(blobStorageOptions.Endpoint));
        return services;
    }
}