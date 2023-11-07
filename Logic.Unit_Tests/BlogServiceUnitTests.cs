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
        private Mock<IBlogRepository> _blogRepositoryMock;
        private Mock<BlogDbContext> _blogDbContextMock;
        [SetUp]
        public void Setup()
        {
            _blogDbContextMock = new Mock<BlogDbContext>();
            _blogRepositoryMock = new Mock<IBlogRepository>();
        }

        [Test]
        public async Task UserOwnsPostAsync_WhenUserOwnsIt_ShouldReturnTrue()
        {
            // Arrange
            var blogId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            _blogRepositoryMock.Setup(repo => repo.GetById(blogId.ToString()))
                   .Returns(new Blog { UserId = userId });

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object);

            // Act
            var result = await blogService.UserOwnsPostAsync(blogId, userId);

            // Assert
            Assert.IsTrue(result);
        }


    }
}
