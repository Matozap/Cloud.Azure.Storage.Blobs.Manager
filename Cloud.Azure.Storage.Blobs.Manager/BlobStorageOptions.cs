namespace Cloud.Azure.Storage.Blobs.Manager;

public class BlobStorageOptions
{
    internal string? Endpoint { get; private set; }
    
    /// <summary>
    /// Set the Azure Storage endpoint to be used in the Blob manager 
    /// </summary>
    /// <param name="endpoint">The endpoint or connection string</param>
    /// <returns>BlobStorageOptions</returns>
    public BlobStorageOptions WithEndpoint(string endpoint)
    {
        Endpoint = endpoint;
        return this;
    }
}