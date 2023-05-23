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
    public void UploadBlobToStorageInvalidMemoryStream()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.UploadBlobAsync("A", "B", "C", null).Wait();

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void UploadBlobToStorageInvalidBlobName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.UploadBlobAsync("A", "B", "", new MemoryStream()).Wait();

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void UploadBlobToStorageInvalidContainerName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.UploadBlobAsync("", "B", "C", new MemoryStream()).Wait();

        act.Should().Throw<ArgumentException>();
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
    public void DownloadBlobToStorageInvalidBlobName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.DownloadBlobAsync("A", "B", "").Wait();

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void DownloadBlobToStorageInvalidContainerName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.DownloadBlobAsync("", "B", "C").Wait();

        act.Should().Throw<ArgumentException>();
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
    public void DownloadBlobContentFromStorageInvalidBlobName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.DownloadBlobContentAsync("A", "B", "").Wait();

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void DownloadBlobContentFromStorageInvalidContainerName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.DownloadBlobContentAsync("", "B", "C").Wait();

        act.Should().Throw<ArgumentException>();
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
    public void MoveBlobToStorageInvalidSourceAndDestination()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.MoveBlobAsync("A", "B", "C", "B").Wait();

        act.Should().Throw<AggregateException>();
    }

    [Fact]
    public void MoveBlobToStorageInvalidBlobName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.MoveBlobAsync("A", "B", "", "D").Wait();

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void MoveBlobToStorageInvalidContainerName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.MoveBlobAsync("", "B", "C", "D").Wait();

        act.Should().Throw<ArgumentException>();
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
    public void DeleteBlobToStorageInvalidBlobName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.DeleteBlobAsync("A", "B", "").Wait();

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void DeleteBlobToStorageInvalidContainerName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.DeleteBlobAsync("", "B", "C").Wait();

        act.Should().Throw<ArgumentException>();
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
    public void ListBlobsInvalidContainerName()
    {
        var storage = new BlobStorageManager(_endpoint);
        var act = () => storage.ListFilesAsync("", "B").Wait();

        act.Should().Throw<ArgumentException>();
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