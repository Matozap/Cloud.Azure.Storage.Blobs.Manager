using Cloud.Azure.Storage.Blobs.Manager.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Cloud.Azure.Storage.Blobs.Manager.Test;

public class BlobStorageTests
{
    private readonly string _endpoint;
    
    public BlobStorageTests()
    {
        _endpoint = "UseDevelopmentStorage=true";
    }
    
    [Fact]
    public void UploadBlobToStorageSuccessful()
    {
        SetupMockedStorageManager();

        var storage = new BlobStorageManager(_endpoint);
        var result = storage.UploadBlobAsync("A", "B", "C", new MemoryStream());
        result.Exception.Should().BeNull();
    }

    [Fact]
    public async Task UploadBlobToStorageInvalidMemoryStream()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.UploadBlobAsync("A", "B", "C", null!);
        
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UploadBlobToStorageInvalidBlobName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.UploadBlobAsync("A", "B", "", new MemoryStream());

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UploadBlobToStorageInvalidContainerName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.UploadBlobAsync("", "B", "C", new MemoryStream());

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public void DownloadBlobToStorageSuccessful()
    {
        SetupMockedStorageManager();

        var storage = new BlobStorageManager(_endpoint);
        var result = storage.DownloadBlobAsync("A", "B", "C");
        result.Exception.Should().BeNull();
        result.IsFaulted.Should().Be(false);
    }

    [Fact]
    public async Task DownloadBlobToStorageInvalidBlobName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.DownloadBlobAsync("A", "B", "");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task DownloadBlobToStorageInvalidContainerName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.DownloadBlobAsync("", "B", "C");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public void DownloadBlobContentFromStorageSuccessful()
    {
        SetupMockedStorageManager();

        var storage = new BlobStorageManager(_endpoint);
        var result = storage.DownloadBlobContentAsync("A", "B", "C");
        result.Exception.Should().BeNull();
        result.IsFaulted.Should().Be(false);
    }

    [Fact]
    public async Task DownloadBlobContentFromStorageInvalidBlobName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.DownloadBlobContentAsync("A", "B", "");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task DownloadBlobContentFromStorageInvalidContainerName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.DownloadBlobContentAsync("", "B", "C");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public void MoveBlobToStorageSuccessful()
    {
        SetupMockedStorageManager();

        var storage = new BlobStorageManager(_endpoint);
        var result = storage.MoveBlobAsync("A", "B", "C", "D");
        result.Exception.Should().BeNull();
        result.IsFaulted.Should().Be(false);
    }

    [Fact]
    public async Task MoveBlobToStorageInvalidSourceAndDestination()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.MoveBlobAsync("A", "B", "C", "B");

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task MoveBlobToStorageInvalidBlobName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.MoveBlobAsync("A", "B", "", "D");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task MoveBlobToStorageInvalidContainerName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.MoveBlobAsync("", "B", "C", "D");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public void DeleteBlobToStorageSuccessful()
    {
        SetupMockedStorageManager();

        var storage = new BlobStorageManager(_endpoint);
        var result = storage.DeleteBlobAsync("A", "B", "C");
        result.Exception.Should().BeNull();
        result.IsFaulted.Should().Be(false);
    }

    [Fact]
    public async Task DeleteBlobToStorageInvalidBlobName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.DeleteBlobAsync("A", "B", "");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task DeleteBlobToStorageInvalidContainerName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.DeleteBlobAsync("", "B", "C");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public void ListBlobsSuccessfully()
    {
        SetupMockedStorageManager();

        var storage = new BlobStorageManager(_endpoint);
        var result = storage.ListFilesAsync("A", "B");
        result.Exception.Should().BeNull();
        result.IsFaulted.Should().Be(false);
    }

    [Fact]
    public async Task ListBlobsInvalidContainerName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.ListFilesAsync("", "B");

        await act.Should().ThrowAsync<ArgumentException>();
    }

    private void SetupMockedStorageManager()
    {
        var repository = new Mock<IBlobStorageManager>();
        repository.Setup(m =>
                m.UploadBlobAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<MemoryStream>()))
            .Returns(() => Task.CompletedTask);

        repository.Setup(m =>
                m.DownloadBlobAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>()))
            .Returns(() => Task.FromResult(new MemoryStream()));

        repository.Setup(m =>
                m.DeleteBlobAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>()))
            .Returns(() => Task.CompletedTask);
    }
}