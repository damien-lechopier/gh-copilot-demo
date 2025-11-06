using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using albums_api.Models;

namespace albums_api.Tests.Models
{
    public class AlbumTests : IDisposable
    {
        public AlbumTests()
        {
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

        #region Constructor and Record Tests

        [Fact]
        public void Album_Constructor_SetsAllProperties()
        {
            // Arrange & Act
            var album = new Album(1, "Test Title", "Test Artist", 12.99, "http://test.com", 2023);

            // Assert
            album.Id.Should().Be(1);
            album.Title.Should().Be("Test Title");
            album.Artist.Should().Be("Test Artist");
            album.Price.Should().Be(12.99);
            album.Image_url.Should().Be("http://test.com");
            album.Year.Should().Be(2023);
        }

        [Fact]
        public void Album_DefaultYear_Is2023()
        {
            // Arrange & Act
            var album = new Album(1, "Test Title", "Test Artist", 12.99, "http://test.com");

            // Assert
            album.Year.Should().Be(2023);
        }

        [Fact]
        public void Album_With_CreatesNewInstanceWithChangedProperty()
        {
            // Arrange
            var originalAlbum = new Album(1, "Original Title", "Original Artist", 10.99, "http://original.com", 2022);

            // Act
            var modifiedAlbum = originalAlbum with { Title = "Modified Title", Price = 15.99 };

            // Assert
            modifiedAlbum.Id.Should().Be(originalAlbum.Id);
            modifiedAlbum.Title.Should().Be("Modified Title");
            modifiedAlbum.Artist.Should().Be("Original Artist"); // Unchanged
            modifiedAlbum.Price.Should().Be(15.99);
            modifiedAlbum.Image_url.Should().Be("http://original.com"); // Unchanged
            modifiedAlbum.Year.Should().Be(2022); // Unchanged

            // Original should be unchanged
            originalAlbum.Title.Should().Be("Original Title");
            originalAlbum.Price.Should().Be(10.99);
        }

        [Fact]
        public void Album_Equality_WorksCorrectly()
        {
            // Arrange
            var album1 = new Album(1, "Title", "Artist", 10.99, "http://test.com", 2023);
            var album2 = new Album(1, "Title", "Artist", 10.99, "http://test.com", 2023);
            var album3 = new Album(2, "Title", "Artist", 10.99, "http://test.com", 2023);

            // Assert
            album1.Should().Be(album2); // Same properties
            album1.Should().NotBe(album3); // Different ID
            album1.GetHashCode().Should().Be(album2.GetHashCode());
        }

        #endregion

        #region GetAll Tests

        [Fact]
        public void GetAll_ReturnsCorrectNumberOfAlbums()
        {
            // Act
            var albums = Album.GetAll();

            // Assert
            albums.Should().HaveCount(6);
        }

        [Fact]
        public void GetAll_ReturnsNewListInstance()
        {
            // Act
            var albums1 = Album.GetAll();
            var albums2 = Album.GetAll();

            // Assert
            albums1.Should().NotBeSameAs(albums2); // Different list instances
            albums1.Should().BeEquivalentTo(albums2); // But same content
        }

        [Fact]
        public void GetAll_ReturnsAlbumsWithCorrectData()
        {
            // Act
            var albums = Album.GetAll();

            // Assert
            albums.Should().Contain(a => a.Title == "You, Me and an App Id" && a.Artist == "Daprize");
            albums.Should().Contain(a => a.Title == "Seven Revision Army" && a.Artist == "The Blue-Green Stripes");
            albums.Should().AllSatisfy(album =>
            {
                album.Id.Should().BeGreaterThan(0);
                album.Title.Should().NotBeNullOrEmpty();
                album.Artist.Should().NotBeNullOrEmpty();
                album.Price.Should().BeGreaterThan(0);
                album.Image_url.Should().NotBeNullOrEmpty();
                album.Year.Should().BeGreaterThan(2000);
            });
        }

        #endregion

        #region GetById Tests

        [Fact]
        public void GetById_ExistingId_ReturnsCorrectAlbum()
        {
            // Arrange
            var albums = Album.GetAll();
            var expectedAlbum = albums.First();

            // Act
            var result = Album.GetById(expectedAlbum.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expectedAlbum);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetById_NonExistingId_ReturnsNull(int id)
        {
            // Act
            var result = Album.GetById(id);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetById_AfterDelete_ReturnsNull()
        {
            // Arrange
            var albums = Album.GetAll();
            var albumToDelete = albums.First();
            Album.Delete(albumToDelete.Id);

            // Act
            var result = Album.GetById(albumToDelete.Id);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetByYear Tests

        [Theory]
        [InlineData(2023, 3)]
        [InlineData(2022, 2)]
        [InlineData(2021, 1)]
        [InlineData(2020, 0)]
        public void GetByYear_ReturnsCorrectCount(int year, int expectedCount)
        {
            // Act
            var albums = Album.GetByYear(year);

            // Assert
            albums.Should().HaveCount(expectedCount);
            if (expectedCount > 0)
            {
                albums.Should().AllSatisfy(album => album.Year.Should().Be(year));
            }
        }

        [Fact]
        public void GetByYear_2023_ReturnsCorrectAlbums()
        {
            // Act
            var albums = Album.GetByYear(2023);

            // Assert
            albums.Should().HaveCount(3);
            albums.Should().Contain(a => a.Title == "You, Me and an App Id");
            albums.Should().Contain(a => a.Title == "Scale It Up");
            albums.Should().Contain(a => a.Title == "Sweet Container O' Mine");
        }

        [Fact]
        public void GetByYear_NonExistingYear_ReturnsEmptyList()
        {
            // Act
            var albums = Album.GetByYear(1999);

            // Assert
            albums.Should().BeEmpty();
            albums.Should().NotBeNull();
        }

        #endregion

        #region Create Tests

        [Fact]
        public void Create_ValidAlbum_ReturnsAlbumWithNewId()
        {
            // Arrange
            var newAlbum = new Album(0, "New Album", "New Artist", 15.99, "http://new.com", 2024);
            var initialCount = Album.GetAll().Count;

            // Act
            var result = Album.Create(newAlbum);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Id.Should().NotBe(0); // Should have been assigned a new ID
            result.Title.Should().Be("New Album");
            result.Artist.Should().Be("New Artist");
            result.Price.Should().Be(15.99);
            result.Image_url.Should().Be("http://new.com");
            result.Year.Should().Be(2024);

            Album.GetAll().Should().HaveCount(initialCount + 1);
        }

        [Fact]
        public void Create_GeneratesUniqueIds()
        {
            // Arrange
            var album1 = new Album(0, "Album 1", "Artist 1", 10.99, "http://1.com", 2024);
            var album2 = new Album(0, "Album 2", "Artist 2", 12.99, "http://2.com", 2024);

            // Act
            var result1 = Album.Create(album1);
            var result2 = Album.Create(album2);

            // Assert
            result1.Id.Should().NotBe(result2.Id);
            result1.Id.Should().BeGreaterThan(0);
            result2.Id.Should().BeGreaterThan(0);
            result2.Id.Should().BeGreaterThan(result1.Id);
        }

        [Fact]
        public void Create_IgnoresProvidedId()
        {
            // Arrange
            var albumWithId = new Album(999, "Test Album", "Test Artist", 10.99, "http://test.com", 2024);

            // Act
            var result = Album.Create(albumWithId);

            // Assert
            result.Id.Should().NotBe(999); // Should ignore the provided ID
            result.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Create_EmptyList_GeneratesId1()
        {
            // Arrange - Delete all albums
            var albums = Album.GetAll();
            foreach (var album in albums.ToList())
            {
                Album.Delete(album.Id);
            }

            var newAlbum = new Album(0, "First Album", "First Artist", 10.99, "http://first.com", 2024);

            // Act
            var result = Album.Create(newAlbum);

            // Assert
            result.Id.Should().Be(1);
        }

        #endregion

        #region Update Tests

        [Fact]
        public void Update_ExistingId_ReturnsUpdatedAlbum()
        {
            // Arrange
            var albums = Album.GetAll();
            var existingAlbum = albums.First();
            var updatedData = new Album(0, "Updated Title", "Updated Artist", 99.99, "http://updated.com", 2025);

            // Act
            var result = Album.Update(existingAlbum.Id, updatedData);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(existingAlbum.Id); // ID should remain the same
            result.Title.Should().Be("Updated Title");
            result.Artist.Should().Be("Updated Artist");
            result.Price.Should().Be(99.99);
            result.Image_url.Should().Be("http://updated.com");
            result.Year.Should().Be(2025);
        }

        [Fact]
        public void Update_NonExistingId_ReturnsNull()
        {
            // Arrange
            var updatedData = new Album(0, "Updated Title", "Updated Artist", 99.99, "http://updated.com", 2025);

            // Act
            var result = Album.Update(999, updatedData);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Update_UpdatesPersistently()
        {
            // Arrange
            var albums = Album.GetAll();
            var existingAlbum = albums.First();
            var originalTitle = existingAlbum.Title;
            var updatedData = existingAlbum with { Title = "Persistently Updated" };

            // Act
            var result = Album.Update(existingAlbum.Id, updatedData);

            // Assert
            result.Title.Should().Be("Persistently Updated");

            // Verify the change persists
            var retrievedAlbum = Album.GetById(existingAlbum.Id);
            retrievedAlbum.Should().NotBeNull();
            retrievedAlbum.Title.Should().Be("Persistently Updated");
            retrievedAlbum.Title.Should().NotBe(originalTitle);
        }

        [Fact]
        public void Update_IgnoresProvidedId()
        {
            // Arrange
            var albums = Album.GetAll();
            var existingAlbum = albums.First();
            var updatedData = new Album(888, "Updated Title", "Updated Artist", 99.99, "http://updated.com", 2025);

            // Act
            var result = Album.Update(existingAlbum.Id, updatedData);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(existingAlbum.Id); // Should use the ID from the parameter, not the album object
            result.Id.Should().NotBe(888);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public void Delete_ExistingId_ReturnsTrue()
        {
            // Arrange
            var albums = Album.GetAll();
            var albumToDelete = albums.First();
            var initialCount = albums.Count;

            // Act
            var result = Album.Delete(albumToDelete.Id);

            // Assert
            result.Should().BeTrue();
            Album.GetAll().Should().HaveCount(initialCount - 1);
            Album.GetById(albumToDelete.Id).Should().BeNull();
        }

        [Fact]
        public void Delete_NonExistingId_ReturnsFalse()
        {
            // Arrange
            var initialCount = Album.GetAll().Count;

            // Act
            var result = Album.Delete(999);

            // Assert
            result.Should().BeFalse();
            Album.GetAll().Should().HaveCount(initialCount); // Count should not change
        }

        [Fact]
        public void Delete_AllAlbums_WorksCorrectly()
        {
            // Arrange
            var albums = Album.GetAll().ToList();

            // Act & Assert
            foreach (var album in albums)
            {
                var result = Album.Delete(album.Id);
                result.Should().BeTrue();
            }

            Album.GetAll().Should().BeEmpty();
        }

        [Fact]
        public void Delete_SameIdTwice_SecondReturnsFalse()
        {
            // Arrange
            var albums = Album.GetAll();
            var albumToDelete = albums.First();

            // Act
            var firstDelete = Album.Delete(albumToDelete.Id);
            var secondDelete = Album.Delete(albumToDelete.Id);

            // Assert
            firstDelete.Should().BeTrue();
            secondDelete.Should().BeFalse();
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void FullCRUDCycle_WorksCorrectly()
        {
            // Create
            var newAlbum = new Album(0, "CRUD Test", "CRUD Artist", 20.00, "http://crud.com", 2024);
            var created = Album.Create(newAlbum);
            created.Should().NotBeNull();
            var albumId = created.Id;

            // Read
            var retrieved = Album.GetById(albumId);
            retrieved.Should().NotBeNull();
            retrieved.Title.Should().Be("CRUD Test");

            // Update
            var updated = Album.Update(albumId, retrieved with { Title = "CRUD Updated", Price = 25.00 });
            updated.Should().NotBeNull();
            updated.Title.Should().Be("CRUD Updated");
            updated.Price.Should().Be(25.00);

            // Verify update persisted
            var retrievedAfterUpdate = Album.GetById(albumId);
            retrievedAfterUpdate.Title.Should().Be("CRUD Updated");

            // Delete
            var deleted = Album.Delete(albumId);
            deleted.Should().BeTrue();

            // Verify deletion
            var retrievedAfterDelete = Album.GetById(albumId);
            retrievedAfterDelete.Should().BeNull();
        }

        [Fact]
        public void ConcurrentOperations_DoNotInterfere()
        {
            // Create multiple albums
            var album1 = Album.Create(new Album(0, "Concurrent 1", "Artist 1", 10.00, "http://1.com", 2024));
            var album2 = Album.Create(new Album(0, "Concurrent 2", "Artist 2", 15.00, "http://2.com", 2024));

            // Update one
            Album.Update(album1.Id, album1 with { Title = "Updated 1" });

            // Verify the other is unchanged
            var retrieved2 = Album.GetById(album2.Id);
            retrieved2.Title.Should().Be("Concurrent 2");

            // Delete one
            Album.Delete(album1.Id);

            // Verify the other still exists
            var retrieved2AfterDelete = Album.GetById(album2.Id);
            retrieved2AfterDelete.Should().NotBeNull();
            retrieved2AfterDelete.Title.Should().Be("Concurrent 2");
        }

        #endregion
    }
}