namespace Cloud.Azure.Storage.Blobs.Manager.Interfaces;

public interface IBlobStorageManager
{
    /// <summary>
    /// Upload a blob to a container given the name and directory.
    /// </summary>
    /// <param name="containerName">The container name</param>
    /// <param name="containerDirectory">The container directory (optional)</param>
    /// <param name="blobName">The blob name</param>
    /// <param name="stream">The stream to be used</param>
    /// <returns>Task</returns>
    public Task UploadBlobAsync(string containerName, string containerDirectory, string blobName, MemoryStream stream);
    /// <summary>
    /// Moved a blob from one directory to another
    /// </summary>
    /// <param name="containerName">The container name</param>
    /// <param name="sourceContainerDirectory">The source directory</param>
    /// <param name="sourceBlobName">The blob name</param>
    /// <param name="destinationDirectory">The destination directory</param>
    /// <returns>Task</returns>
    public Task MoveBlobAsync(string containerName, string sourceContainerDirectory, string sourceBlobName, string destinationDirectory);
    /// <summary>
    /// downloads a blob as a memory stream
    /// </summary>
    /// <param name="containerName">The container name</param>
    /// <param name="containerDirectory">The container directory</param>
    /// <param name="blobName">The blob name</param>
    /// <returns>MemoryStream</returns>
    Task<MemoryStream> DownloadBlobAsync(string containerName, string containerDirectory, string blobName);
    /// <summary>
    /// Downloads the blob contents as a string  
    /// </summary>
    /// <param name="containerName">The container name</param>
    /// <param name="containerDirectory">The container directory</param>
    /// <param name="blobName">The blob name</param>
    /// <returns>string</returns>
    Task<string> DownloadBlobContentAsync(string containerName, string containerDirectory, string blobName);
    /// <summary>
    /// Deletes a blob from a container
    /// </summary>
    /// <param name="containerName">The container name</param>
    /// <param name="containerDirectory">The container directory</param>
    /// <param name="blobName">The blob name</param>
    /// <returns>Task</returns>
    Task DeleteBlobAsync(string containerName, string containerDirectory, string blobName);
    /// <summary>
    /// List al the blob or files from a storage location
    /// </summary>
    /// <param name="containerName">The container name</param>
    /// <param name="containerDirectory">The container directory</param>
    /// <returns>List of strings</returns>
    Task<List<string>> ListFilesAsync(string containerName, string containerDirectory);
}