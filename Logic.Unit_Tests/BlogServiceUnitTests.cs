using Data.Data;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.Interfaces;
using Moq;
using NUnit.Framework;
using System;

namespace Logic.Unit_Tests
{
    [TestFixture]
    public class BlogServiceUnitTests
    {
        [Test]
        public async Task UserOwnsPostAsync_WhenUserOwnsIt_ShouldReturnTrue()
        {
            // Arrange
            var blogId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();

            // Create a mock of IBlogRepository
            var blogRepositoryMock = new Mock<IBlogRepository>();

            // Configure the mock to return a blog with the specified user ID when GetById is called
            blogRepositoryMock.Setup(repo => repo.GetById(blogId.ToString()))
                   .Returns(new Blog { UserId = userId });


            // Create a mock of BlogDbContext (use DbContextOptions if needed)
            var dbContextMock = new Mock<BlogDbContext>();

            // Create the BlogService instance with both dependencies
            var blogService = new BlogService(dbContextMock.Object, blogRepositoryMock.Object);

            // Act
            var result = await blogService.UserOwnsPostAsync(blogId, userId);

            // Assert
            Assert.IsTrue(result);
        }


    }
}
