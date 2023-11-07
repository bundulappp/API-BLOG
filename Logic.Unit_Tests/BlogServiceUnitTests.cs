using Data.Data;
using Logic.Services;
using Models.Domain;
using Models.Interfaces;
using Moq;

namespace Logic.Unit_Tests
{

    [TestFixture]
    public class BlogServiceUnitTests
    {
        private Mock<IBlogRepository> _blogRepositoryMock;
        private Mock<BlogDbContext> _blogDbContextMock;
        private Mock<ITagRepository> _tagRepositoryMock;
        [SetUp]
        public void Setup()
        {
            _blogDbContextMock = new Mock<BlogDbContext>();
            _blogRepositoryMock = new Mock<IBlogRepository>();
            _tagRepositoryMock = new Mock<ITagRepository>();
        }

        [Test]
        public async Task UserOwnsBlogAsync_WhenUserOwnsIt_ShouldReturnTrue()
        {
            // Arrange
            var blogId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            _blogRepositoryMock.Setup(repo => repo.GetById(blogId.ToString()))
                   .Returns(new Blog { UserId = userId });

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.UserOwnsBlogAsync(blogId, userId);

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task UserOwnsBlogAsync_WhenUserDoesNotOwnIt_ShouldReturnFalse()
        {
            // Arrange
            var blogId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            _blogRepositoryMock.Setup(repo => repo.GetById(blogId.ToString()))
                   .Returns(new Blog());

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.UserOwnsBlogAsync(blogId, userId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task UserOwnsBlogAsync_WhenBlogIsNotFound_ShouldReturnFalse()
        {
            // Arrange
            var blogId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            _blogRepositoryMock.Setup(repo => repo.GetById(blogId.ToString()))
                   .Returns((Blog)null);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.UserOwnsBlogAsync(blogId, userId);

            // Assert
            Assert.IsFalse(result);
        }

    }
}
