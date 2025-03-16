﻿using DataAccess.Repositories;
using DataAccess.Services;
using Moq;
using Tests.Integration.Fixtures;

namespace Tests.Integration.Repositories;

public class ImagesRepositoryTests(ConfigurationFixture configuration) : IClassFixture<ConfigurationFixture>
{
    private readonly ConfigurationService configurationService = new(configuration.Configuration);
    private readonly string testDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestImages");

    [Fact]
    public async Task GetAsyncThrowWhenUrlIsEmptyTest()
    {
        var httpClientMock = new Mock<HttpClient>();
        var repository = new ImagesRepository(httpClientMock.Object, testDirectory);

        await Assert.ThrowsAsync<ArgumentNullException>(() => repository.GetAsync(string.Empty, CancellationToken.None));
    }

    [Fact]
    public async Task GetAsyncWhenFileExistsTest()
    {
        try
        {
            var fileName = "test.jpg";
            var filePath = Path.Combine(testDirectory, fileName);

            CreateTestDirectory(testDirectory);
            await File.WriteAllBytesAsync(filePath, [1, 2, 3]);

            var httpClientMock = new Mock<HttpClient>();
            var repository = new ImagesRepository(httpClientMock.Object, testDirectory);
            var result = await repository.GetAsync(filePath, CancellationToken.None);

            Assert.Equal(filePath, result);
        }
        finally
        {
            DeleteTestDirectory(testDirectory);
        }
    }

    [Fact]
    public async Task GetAsyncTest()
    {
        try
        {
            var imageUrl = configurationService.TheCatImagesUri + "/0XYvRd7oD.jpg";
            var fileName = Path.GetFileName(imageUrl);
            var filePath = Path.Combine(testDirectory, fileName);
            var repository = new ImagesRepository(configuration.HttpClient, testDirectory);

            var result = await repository.GetAsync(imageUrl, CancellationToken.None);

            Assert.Equal(filePath, result);
            Assert.True(File.Exists(filePath));
        }
        finally
        {
            DeleteTestDirectory(testDirectory);
        }
    }

    private static void CreateTestDirectory(string testDirectory)
    {
        if (Directory.Exists(testDirectory) == false)
        {
            Directory.CreateDirectory(testDirectory);
        }
    }

    private static void DeleteTestDirectory(string testDirectory)
    {
        if (Directory.Exists(testDirectory))
        {
            Directory.Delete(testDirectory, true);
        }
    }
}
