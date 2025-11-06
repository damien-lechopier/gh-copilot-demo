using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using FluentAssertions;
using albums_api.Controllers;
using albums_api.Models;

namespace albums_api.Tests.Controllers
{
    public class AlbumControllerTests : IDisposable
    {
        private readonly AlbumController _controller;

        public AlbumControllerTests()
        {
            _controller = new AlbumController();
            // Reset albums to initial state before each test
            ResetAlbumsData();
        }

        public void Dispose()
        {
            // Reset albums data after each test to avoid side effects
            ResetAlbumsData();
        }

        private void ResetAlbumsData()
        {
            // Clear all albums first
            var allAlbums = Album.GetAll().ToList();
            foreach (var album in allAlbums)
            {
                Album.Delete(album.Id);
            }

            // Re-add initial albums
            Album.Create(new Album(0, "You, Me and an App Id", "Daprize", 10.99, "https://aka.ms/albums-daprlogo", 2023));
            Album.Create(new Album(0, "Seven Revision Army", "The Blue-Green Stripes", 13.99, "https://aka.ms/albums-containerappslogo", 2022));
            Album.Create(new Album(0, "Scale It Up", "KEDA Club", 13.99, "https://aka.ms/albums-kedalogo", 2023));
            Album.Create(new Album(0, "Lost in Translation", "MegaDNS", 12.99, "https://aka.ms/albums-envoylogo", 2021));
            Album.Create(new Album(0, "Lock Down Your Love", "V is for VNET", 12.99, "https://aka.ms/albums-vnetlogo", 2022));
            Album.Create(new Album(0, "Sweet Container O' Mine", "Guns N Probeses", 14.99, "https://aka.ms/albums-containerappslogo", 2023));
        }

        #region GET Tests

        [Fact]
        public void Get_ReturnsOkObjectResultWithAlbums()
        {
            // Act
            var result = _controller.Get();

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var albums = okResult.Value.Should().BeAssignableTo<IEnumerable<Album>>().Subject;
            albums.Should().HaveCount(6);
        }

        [Fact]
        public void Get_ReturnsAllAlbumsWithCorrectProperties()
        {
            // Act
            var result = _controller.Get();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var albums = okResult.Value.Should().BeAssignableTo<IEnumerable<Album>>().Subject.ToList();
            
            albums.Should().Contain(a => a.Title == "You, Me and an App Id" && a.Artist == "Daprize");
            albums.Should().Contain(a => a.Year == 2023);
            albums.Should().AllSatisfy(a => 
            {
                a.Id.Should().BeGreaterThan(0);
                a.Title.Should().NotBeNullOrEmpty();
                a.Artist.Should().NotBeNullOrEmpty();
                a.Price.Should().BeGreaterThan(0);
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Get_ById_ExistingId_ReturnsOkResultWithCorrectAlbum(int id)
        {
            // Act
            var result = _controller.Get(id);

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var album = okResult.Value.Should().BeOfType<Album>().Subject;
            album.Id.Should().Be(id);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(0)]
        [InlineData(-1)]
        public void Get_ById_NonExistingId_ReturnsNotFound(int id)
        {
            // Act
            var result = _controller.Get(id);

            // Assert
            var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be($"Album with ID {id} not found");
        }

        #endregion

        #region GET By Year Tests

        [Theory]
        [InlineData(2023, 3)]
        [InlineData(2022, 2)]
        [InlineData(2021, 1)]
        public void GetByYear_ExistingYear_ReturnsCorrectCount(int year, int expectedCount)
        {
            // Act
            var result = _controller.GetByYear(year);

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var albums = okResult.Value.Should().BeAssignableTo<IEnumerable<Album>>().Subject;
            albums.Should().HaveCount(expectedCount);
            albums.Should().AllSatisfy(a => a.Year.Should().Be(year));
        }

        [Fact]
        public void GetByYear_NonExistingYear_ReturnsEmptyList()
        {
            // Act
            var result = _controller.GetByYear(1999);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var albums = okResult.Value.Should().BeAssignableTo<IEnumerable<Album>>().Subject;
            albums.Should().BeEmpty();
        }

        [Fact]
        public void GetByYear_2023_ReturnsCorrectAlbums()
        {
            // Act
            var result = _controller.GetByYear(2023);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var albums = okResult.Value.Should().BeAssignableTo<IEnumerable<Album>>().Subject.ToList();
            
            albums.Should().Contain(a => a.Title == "You, Me and an App Id");
            albums.Should().Contain(a => a.Title == "Scale It Up");
            albums.Should().Contain(a => a.Title == "Sweet Container O' Mine");
        }

        #endregion

        #region POST Tests

        [Fact]
        public void Post_ValidAlbum_ReturnsCreatedResult()
        {
            // Arrange
            var newAlbum = new Album(0, "Test Album", "Test Artist", 15.99, "http://test.com", 2024);

            // Act
            var result = _controller.Post(newAlbum);

            // Assert
            var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var createdAlbum = createdResult.Value.Should().BeOfType<Album>().Subject;
            
            createdAlbum.Title.Should().Be("Test Album");
            createdAlbum.Artist.Should().Be("Test Artist");
            createdAlbum.Price.Should().Be(15.99);
            createdAlbum.Id.Should().BeGreaterThan(0);
            
            createdResult.ActionName.Should().Be(nameof(AlbumController.Get));
            createdResult.RouteValues["id"].Should().Be(createdAlbum.Id);
        }

        [Fact]
        public void Post_NullAlbum_ReturnsBadRequest()
        {
            // Act
            var result = _controller.Post(null);

            // Assert
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Album data is required");
        }

        [Theory]
        [InlineData("", "Valid Artist")]
        [InlineData("Valid Title", "")]
        [InlineData(null, "Valid Artist")]
        [InlineData("Valid Title", null)]
        [InlineData("   ", "Valid Artist")]
        [InlineData("Valid Title", "   ")]
        public void Post_InvalidTitleOrArtist_ReturnsBadRequest(string title, string artist)
        {
            // Arrange
            var invalidAlbum = new Album(0, title, artist, 15.99, "http://test.com", 2024);

            // Act
            var result = _controller.Post(invalidAlbum);

            // Assert
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Title and Artist are required fields");
        }

        [Fact]
        public void Post_ValidAlbum_AlbumIsActuallyCreated()
        {
            // Arrange
            var initialCount = Album.GetAll().Count;
            var newAlbum = new Album(0, "Persistence Test", "Test Artist", 12.50, "http://test.com", 2024);

            // Act
            var result = _controller.Post(newAlbum);

            // Assert
            Album.GetAll().Should().HaveCount(initialCount + 1);
            
            var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var createdAlbum = createdResult.Value.Should().BeOfType<Album>().Subject;
            
            var retrievedAlbum = Album.GetById(createdAlbum.Id);
            retrievedAlbum.Should().NotBeNull();
            retrievedAlbum.Title.Should().Be("Persistence Test");
        }

        #endregion

        #region PUT Tests

        [Fact]
        public void Put_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var existingId = 1;
            var updatedAlbum = new Album(0, "Updated Album", "Updated Artist", 19.99, "http://updated.com", 2024);

            // Act
            var result = _controller.Put(existingId, updatedAlbum);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedAlbum = okResult.Value.Should().BeOfType<Album>().Subject;
            
            returnedAlbum.Id.Should().Be(existingId);
            returnedAlbum.Title.Should().Be("Updated Album");
            returnedAlbum.Artist.Should().Be("Updated Artist");
            returnedAlbum.Price.Should().Be(19.99);
            returnedAlbum.Year.Should().Be(2024);
        }

        [Fact]
        public void Put_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingId = 999;
            var updatedAlbum = new Album(0, "Updated Album", "Updated Artist", 19.99, "http://updated.com", 2024);

            // Act
            var result = _controller.Put(nonExistingId, updatedAlbum);

            // Assert
            var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be($"Album with ID {nonExistingId} not found");
        }

        [Fact]
        public void Put_NullAlbum_ReturnsBadRequest()
        {
            // Act
            var result = _controller.Put(1, null);

            // Assert
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Album data is required");
        }

        [Theory]
        [InlineData("", "Valid Artist")]
        [InlineData("Valid Title", "")]
        [InlineData(null, "Valid Artist")]
        [InlineData("Valid Title", null)]
        public void Put_InvalidTitleOrArtist_ReturnsBadRequest(string title, string artist)
        {
            // Arrange
            var invalidAlbum = new Album(0, title, artist, 15.99, "http://test.com", 2024);

            // Act
            var result = _controller.Put(1, invalidAlbum);

            // Assert
            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Title and Artist are required fields");
        }

        [Fact]
        public void Put_ValidUpdate_ActuallyUpdatesAlbum()
        {
            // Arrange
            var existingId = 1;
            var originalAlbum = Album.GetById(existingId);
            var updatedAlbum = new Album(0, "Actually Updated", "Actually Updated Artist", 25.99, "http://actually-updated.com", 2025);

            // Act
            var result = _controller.Put(existingId, updatedAlbum);

            // Assert
            var retrievedAlbum = Album.GetById(existingId);
            retrievedAlbum.Should().NotBeNull();
            retrievedAlbum.Title.Should().Be("Actually Updated");
            retrievedAlbum.Artist.Should().Be("Actually Updated Artist");
            retrievedAlbum.Price.Should().Be(25.99);
            retrievedAlbum.Year.Should().Be(2025);
            retrievedAlbum.Title.Should().NotBe(originalAlbum.Title);
        }

        #endregion

        #region DELETE Tests

        [Fact]
        public void Delete_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var existingId = 1;

            // Act
            var result = _controller.Delete(existingId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void Delete_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingId = 999;

            // Act
            var result = _controller.Delete(nonExistingId);

            // Assert
            var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be($"Album with ID {nonExistingId} not found");
        }

        [Fact]
        public void Delete_ExistingId_ActuallyDeletesAlbum()
        {
            // Arrange
            var existingId = 1;
            var initialCount = Album.GetAll().Count;
            Album.GetById(existingId).Should().NotBeNull(); // Ensure it exists

            // Act
            var result = _controller.Delete(existingId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            Album.GetAll().Should().HaveCount(initialCount - 1);
            Album.GetById(existingId).Should().BeNull();
        }

        [Fact]
        public void Delete_LastAlbum_WorksCorrectly()
        {
            // Arrange - Delete all but one album
            var allAlbums = Album.GetAll().ToList();
            for (int i = 1; i < allAlbums.Count; i++)
            {
                Album.Delete(allAlbums[i].Id);
            }
            
            var lastAlbumId = allAlbums[0].Id;

            // Act
            var result = _controller.Delete(lastAlbumId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            Album.GetAll().Should().BeEmpty();
        }

        #endregion

        #region Edge Cases and Integration Tests

        [Fact]
        public void CreateUpdateDelete_FullLifecycle_WorksCorrectly()
        {
            // Arrange
            var newAlbum = new Album(0, "Lifecycle Test", "Test Artist", 20.00, "http://test.com", 2024);

            // Act & Assert - Create
            var createResult = _controller.Post(newAlbum);
            var createdResult = createResult.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var createdAlbum = createdResult.Value.Should().BeOfType<Album>().Subject;
            var albumId = createdAlbum.Id;

            // Act & Assert - Read
            var getResult = _controller.Get(albumId);
            var getOkResult = getResult.Result.Should().BeOfType<OkObjectResult>().Subject;
            var retrievedAlbum = getOkResult.Value.Should().BeOfType<Album>().Subject;
            retrievedAlbum.Title.Should().Be("Lifecycle Test");

            // Act & Assert - Update
            var updatedAlbum = retrievedAlbum with { Title = "Lifecycle Updated", Price = 25.00 };
            var updateResult = _controller.Put(albumId, updatedAlbum);
            var updateOkResult = updateResult.Result.Should().BeOfType<OkObjectResult>().Subject;
            var updatedReturnedAlbum = updateOkResult.Value.Should().BeOfType<Album>().Subject;
            updatedReturnedAlbum.Title.Should().Be("Lifecycle Updated");
            updatedReturnedAlbum.Price.Should().Be(25.00);

            // Act & Assert - Delete
            var deleteResult = _controller.Delete(albumId);
            deleteResult.Should().BeOfType<NoContentResult>();

            // Verify deletion
            var getAfterDeleteResult = _controller.Get(albumId);
            getAfterDeleteResult.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void MultipleOperations_DoNotInterfereWithEachOther()
        {
            // Create multiple albums
            var album1 = new Album(0, "Album 1", "Artist 1", 10.00, "http://1.com", 2024);
            var album2 = new Album(0, "Album 2", "Artist 2", 15.00, "http://2.com", 2024);

            var result1 = _controller.Post(album1);
            var result2 = _controller.Post(album2);

            var created1 = ((CreatedAtActionResult)result1.Result).Value as Album;
            var created2 = ((CreatedAtActionResult)result2.Result).Value as Album;

            // Update one album
            var updatedAlbum1 = created1 with { Title = "Updated Album 1" };
            _controller.Put(created1.Id, updatedAlbum1);

            // Verify the other album is unchanged
            var getResult2 = _controller.Get(created2.Id);
            var retrieved2 = ((OkObjectResult)getResult2.Result).Value as Album;
            retrieved2.Title.Should().Be("Album 2"); // Should not have changed

            // Verify the updated album is changed
            var getResult1 = _controller.Get(created1.Id);
            var retrieved1 = ((OkObjectResult)getResult1.Result).Value as Album;
            retrieved1.Title.Should().Be("Updated Album 1");
        }

        #endregion
    }
}