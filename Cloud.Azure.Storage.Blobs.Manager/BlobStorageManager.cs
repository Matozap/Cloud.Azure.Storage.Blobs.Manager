using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Cloud.Azure.Storage.Blobs.Manager.Interfaces;

namespace Cloud.Azure.Storage.Blobs.Manager;

public class BlobStorageManager : IBlobStorageManager
    {
        private readonly BlobServiceClient _blobClient;
      
        public BlobStorageManager(string endpoint)
        {
            _blobClient = new BlobServiceClient(endpoint);
        }
        
        public async Task UploadBlobAsync(string containerName, string containerDirectory, string blobName, MemoryStream stream)
        {
            ArgumentException.ThrowIfNullOrEmpty(containerName);
            ArgumentException.ThrowIfNullOrEmpty(blobName);
            ArgumentNullException.ThrowIfNull(stream);
            
            var container = _blobClient.GetBlobContainerClient(containerName);
            var filename = !string.IsNullOrEmpty(containerDirectory) ? $"{containerDirectory}/{blobName}" : blobName;
            var newFileClient = container.GetBlobClient(filename);
            await newFileClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = "application/json" });
        }
        
        public async Task<MemoryStream> DownloadBlobAsync(string containerName, string containerDirectory, string blobName)
        {
            ArgumentException.ThrowIfNullOrEmpty(containerName);
            ArgumentException.ThrowIfNullOrEmpty(blobName);
            
            var container = _blobClient.GetBlobContainerClient(containerName);
            
            var sourceFilename = !string.IsNullOrEmpty(containerDirectory) ? $"{containerDirectory}/{blobName}" : blobName;
            var existingFileClient = container.GetBlobClient(sourceFilename);
            
            var stream = new MemoryStream();
            await existingFileClient.DownloadToAsync(stream);
            stream.Position = 0;
            return stream;
        }
        
        public async Task<string> DownloadBlobContentAsync(string containerName, string containerDirectory, string blobName)
        {
            ArgumentException.ThrowIfNullOrEmpty(containerName);
            ArgumentException.ThrowIfNullOrEmpty(blobName);
            
            var container = _blobClient.GetBlobContainerClient(containerName);
            
            var sourceFilename = !string.IsNullOrEmpty(containerDirectory) ? $"{containerDirectory}/{blobName}" : blobName;
            var existingFileClient = container.GetBlobClient(sourceFilename);
            
            var stream = new MemoryStream();
            await existingFileClient.DownloadToAsync(stream);
            
            stream.Position = 0;
            using var streamReader = new StreamReader(stream);
            return await streamReader.ReadToEndAsync();
        }
        
        public async Task DeleteBlobAsync(string containerName, string containerDirectory, string blobName)
        {
            ArgumentException.ThrowIfNullOrEmpty(containerName);
            ArgumentException.ThrowIfNullOrEmpty(blobName);
            
            var container = _blobClient.GetBlobContainerClient(containerName);
            
            var sourceFilename = !string.IsNullOrEmpty(containerDirectory) ? $"{containerDirectory}/{blobName}" : blobName;
            var existingFileClient = container.GetBlobClient(sourceFilename);
            await existingFileClient.DeleteAsync();
        }

        public async Task MoveBlobAsync(string containerName, string sourceContainerDirectory, string blobName, string destinationDirectory)
        {
            ArgumentException.ThrowIfNullOrEmpty(containerName);
            ArgumentException.ThrowIfNullOrEmpty(blobName);
            
            if (sourceContainerDirectory == destinationDirectory)
            {
                throw new InvalidOperationException("Destination directory must be different then source directory");
            }
            
            
            var stream = await DownloadBlobAsync(containerName, sourceContainerDirectory, blobName);
            await UploadBlobAsync(containerName, destinationDirectory, blobName, stream);
            await DeleteBlobAsync(containerName, sourceContainerDirectory, blobName);
        }
        
        public async Task<List<string>> ListFilesAsync(string containerName, string containerDirectory)
        {
            ArgumentException.ThrowIfNullOrEmpty(containerName);
            
            var blobNames = new List<string>();
            var container = _blobClient.GetBlobContainerClient(containerName);
            var blobHierarchyItems = string.IsNullOrEmpty(containerDirectory) ? 
                container.GetBlobsByHierarchyAsync() :
                container.GetBlobsByHierarchyAsync(prefix:$"{containerDirectory}/", delimiter: "/");
            
            await foreach (var blobHierarchyItem in blobHierarchyItems)
            {
                if (!blobHierarchyItem.IsPrefix)
                {
                    blobNames.Add(blobHierarchyItem.Blob.Name.Replace($"{containerDirectory}/", ""));
                }
            }

            return blobNames;
        }
    }