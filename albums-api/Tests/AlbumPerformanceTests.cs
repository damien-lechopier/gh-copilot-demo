using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;
using FluentAssertions;
using albums_api.Controllers;
using albums_api.Models;

namespace albums_api.Tests.Performance
{
    public class AlbumPerformanceTests : IDisposable
    {
        private readonly AlbumController _controller;

        public AlbumPerformanceTests()
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

        [Fact]
        public void Get_AllAlbums_PerformanceTest()
        {
            // Arrange
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            var result = _controller.Get();
            stopwatch.Stop();

            // Assert
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(100, "Getting all albums should be fast");
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetById_PerformanceTest()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var albumId = 1;

            // Act
            stopwatch.Start();
            var result = _controller.Get(albumId);
            stopwatch.Stop();

            // Assert
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(50, "Getting album by ID should be very fast");
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetByYear_PerformanceTest()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var year = 2023;

            // Act
            stopwatch.Start();
            var result = _controller.GetByYear(year);
            stopwatch.Stop();

            // Assert
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(100, "Getting albums by year should be fast");
            result.Should().NotBeNull();
        }

        [Fact]
        public void Create_MultipleAlbums_PerformanceTest()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            var albums = new List<Album>();
            for (int i = 0; i < 100; i++)
            {
                albums.Add(new Album(0, $"Performance Test Album {i}", $"Artist {i}", 10.99 + i, $"http://test{i}.com", 2024));
            }

            // Act
            stopwatch.Start();
            foreach (var album in albums)
            {
                _controller.Post(album);
            }
            stopwatch.Stop();

            // Assert
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000, "Creating 100 albums should take less than 1 second");
            Album.GetAll().Should().HaveCountGreaterOrEqualTo(100);
        }

        [Fact]
        public void Update_MultipleAlbums_PerformanceTest()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            
            // Create some albums first
            var createdAlbums = new List<Album>();
            for (int i = 0; i < 50; i++)
            {
                var result = _controller.Post(new Album(0, $"Update Test Album {i}", $"Artist {i}", 10.99, $"http://test{i}.com", 2024));
                var createdAlbum = ((Microsoft.AspNetCore.Mvc.CreatedAtActionResult)result.Result).Value as Album;
                createdAlbums.Add(createdAlbum!);
            }

            // Act
            stopwatch.Start();
            foreach (var album in createdAlbums)
            {
                var updatedAlbum = album with { Title = $"Updated {album.Title}", Price = album.Price + 5.0 };
                _controller.Put(album.Id, updatedAlbum);
            }
            stopwatch.Stop();

            // Assert
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(500, "Updating 50 albums should take less than 500ms");
        }

        [Fact]
        public void Delete_MultipleAlbums_PerformanceTest()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            
            // Create some albums first
            var createdAlbumIds = new List<int>();
            for (int i = 0; i < 50; i++)
            {
                var result = _controller.Post(new Album(0, $"Delete Test Album {i}", $"Artist {i}", 10.99, $"http://test{i}.com", 2024));
                var createdAlbum = ((Microsoft.AspNetCore.Mvc.CreatedAtActionResult)result.Result).Value as Album;
                createdAlbumIds.Add(createdAlbum!.Id);
            }

            // Act
            stopwatch.Start();
            foreach (var id in createdAlbumIds)
            {
                _controller.Delete(id);
            }
            stopwatch.Stop();

            // Assert
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(500, "Deleting 50 albums should take less than 500ms");
        }

        [Fact]
        public void MixedOperations_PerformanceTest()
        {
            // Arrange
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            
            // Create
            var createResult = _controller.Post(new Album(0, "Mixed Test Album", "Mixed Artist", 19.99, "http://mixed.com", 2024));
            var createdAlbum = ((Microsoft.AspNetCore.Mvc.CreatedAtActionResult)createResult.Result).Value as Album;
            
            // Read
            _controller.Get(createdAlbum!.Id);
            _controller.Get();
            _controller.GetByYear(2024);
            
            // Update
            var updatedAlbum = createdAlbum with { Title = "Updated Mixed Album" };
            _controller.Put(createdAlbum.Id, updatedAlbum);
            
            // Delete
            _controller.Delete(createdAlbum.Id);
            
            stopwatch.Stop();

            // Assert
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(100, "Mixed CRUD operations should be fast");
        }

        [Fact]
        public void LargeDataSet_SearchPerformance()
        {
            // Arrange - Create a larger dataset
            for (int i = 0; i < 1000; i++)
            {
                var year = 2020 + (i % 5); // Years from 2020 to 2024
                Album.Create(new Album(0, $"Large Dataset Album {i}", $"Artist {i}", 10.99 + (i % 20), $"http://test{i}.com", year));
            }

            var stopwatch = new Stopwatch();

            // Act - Search in large dataset
            stopwatch.Start();
            var result2023 = _controller.GetByYear(2023);
            var result2024 = _controller.GetByYear(2024);
            var allAlbums = _controller.Get();
            stopwatch.Stop();

            // Assert
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(200, "Searching in large dataset should still be reasonably fast");
            result2023.Should().NotBeNull();
            result2024.Should().NotBeNull();
            allAlbums.Should().NotBeNull();
        }
    }
}