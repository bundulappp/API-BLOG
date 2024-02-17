using Logic.Services;
using Models.Domain;
using Models.Interfaces;
using Moq;

namespace Logic.Unit_Tests
{
    [TestFixture]
    public class CommentServiceUnitTests
    {
        private Mock<IBlogRepository> _blogRepositoryMock;
        private Mock<ICommentRepository> _commentRepositoryMock;
        [SetUp]
        public void Setup()
        {
            _blogRepositoryMock = new Mock<IBlogRepository>();
            _commentRepositoryMock = new Mock<ICommentRepository>();
        }

        [Test]
        public async Task CreateCommentAsync_SomeOfThePropertiesMissing_ShouldReturnFalse()
        {
            // Arrange
            var incompleteComment = new Comment
            {
                // Missing properties...
                Body = "Blog test content",
            };

            _commentRepositoryMock.Setup(repo => repo.Insert(incompleteComment)).ReturnsAsync(false);

            var commentService = new CommentService(_commentRepositoryMock.Object, _blogRepositoryMock.Object);

            //Act
            var result = await commentService.CreateCommentAsnyc(incompleteComment);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateCommentAsync_WithRequiredBody_ShouldReturnTrue()
        {
            // Arrange
            var comment = new Comment
            {
                BlogId = Guid.NewGuid().ToString(),
                Body = "Blog test content",
                UserId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                LikesCounter = 0
            };

            _commentRepositoryMock.Setup(repo => repo.Insert(comment)).ReturnsAsync(true);

            var commentService = new CommentService(_commentRepositoryMock.Object, _blogRepositoryMock.Object);

            //Act
            var result = await commentService.CreateCommentAsnyc(comment);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CreateCommentAsync_DbInsertFails_ShouldReturnFalse()
        {
            // Arrange
            var comment = new Comment
            {
                BlogId = Guid.NewGuid().ToString(),
                Body = "Blog test content",
                UserId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                LikesCounter = 0
            };

            _commentRepositoryMock.Setup(repo => repo.Insert(comment)).ReturnsAsync(false);

            var commentService = new CommentService(_commentRepositoryMock.Object, _blogRepositoryMock.Object);

            //Act
            var result = await commentService.CreateCommentAsnyc(comment);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllBlogsCommentAsnyc_BlogIsNotExist_ThrowException()
        {
            //Arrange
            var blogId = Guid.NewGuid().ToString();
            var commentService = new CommentService(_commentRepositoryMock.Object, _blogRepositoryMock.Object);
            _blogRepositoryMock.Setup(repo => repo.GetById(blogId.ToString()))
                   .ReturnsAsync((Blog)null);

            //Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(
                   async () => await commentService.GetAllBlogsCommentAsnyc(blogId, It.IsAny<PaginationFilter>())
               );
            Assert.That(ex.Message, Is.EqualTo($"Blog is not found with id: {blogId}"));
        }

        [Test]
        public async Task GetAllBlogsCommentAsnyc_ExistingBlog_ShouldReturnComments()
        {
            // Arrange
            var blogId = Guid.NewGuid().ToString();
            var comments = new List<Comment>
            {
                new Comment { Id = Guid.NewGuid().ToString(), BlogId = blogId },
                new Comment { Id = Guid.NewGuid().ToString(), BlogId = blogId }
            };
            _blogRepositoryMock.Setup(repo => repo.GetById(blogId)).ReturnsAsync(new Blog { Id = blogId });
            _commentRepositoryMock.Setup(repo => repo.GetAllBlogsCommentAsync(blogId, It.IsAny<PaginationFilter>())).ReturnsAsync(comments);

            var commentService = new CommentService(_commentRepositoryMock.Object, _blogRepositoryMock.Object);

            // Act
            var result = await commentService.GetAllBlogsCommentAsnyc(blogId, It.IsAny<PaginationFilter>());

            // Assert
            Assert.AreEqual(2, result.Count());
            CollectionAssert.AreEqual(comments, result);
        }

        [Test]
        public async Task GetAllBlogsCommentAsnyc_ExistingBlogWithNoComments_ShouldReturnEmptyList()
        {
            // Arrange
            var blogId = Guid.NewGuid().ToString();
            var comments = new List<Comment>();
            _blogRepositoryMock.Setup(repo => repo.GetById(blogId)).ReturnsAsync(new Blog { Id = blogId });
            _commentRepositoryMock.Setup(repo => repo.GetAllBlogsCommentAsync(blogId, It.IsAny<PaginationFilter>())).ReturnsAsync(comments);

            var commentService = new CommentService(_commentRepositoryMock.Object, _blogRepositoryMock.Object);

            // Act
            var result = await commentService.GetAllBlogsCommentAsnyc(blogId, It.IsAny<PaginationFilter>());

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetAllBlogsCommentAsnyc_DbError_ThrowsException()
        {
            // Arrange
            var blogId = Guid.NewGuid().ToString();
            _blogRepositoryMock.Setup(repo => repo.GetById(blogId)).ReturnsAsync(new Blog { Id = blogId });
            _commentRepositoryMock.Setup(repo => repo.GetAllBlogsCommentAsync(blogId, It.IsAny<PaginationFilter>())).ThrowsAsync(new Exception("Database error"));

            var commentService = new CommentService(_commentRepositoryMock.Object, _blogRepositoryMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => commentService.GetAllBlogsCommentAsnyc(blogId, It.IsAny<PaginationFilter>()));
        }

        [Test]
        public async Task DeleteCommentAsync_ExistingComment_ShouldReturnTrue()
        {
            // Arrange
            var commentId = Guid.NewGuid().ToString();
            var comment = new Comment { Id = commentId };
            _commentRepositoryMock.Setup(repo => repo.GetById(commentId)).ReturnsAsync(comment);
            _commentRepositoryMock.Setup(repo => repo.Delete(comment)).ReturnsAsync(true);

            var commentService = new CommentService(_commentRepositoryMock.Object, _blogRepositoryMock.Object);

            // Act
            var result = await commentService.DeleteCommentAsync(commentId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteCommentAsync_NonExistingComment_ShouldReturnFalse()
        {
            // Arrange
            var commentId = Guid.NewGuid().ToString();
            _commentRepositoryMock.Setup(repo => repo.GetById(commentId)).ReturnsAsync((Comment)null);

            var commentService = new CommentService(_commentRepositoryMock.Object, _blogRepositoryMock.Object);

            // Act
            var result = await commentService.DeleteCommentAsync(commentId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteCommentAsync_DbDeleteFails_ShouldReturnFalse()
        {
            // Arrange
            var commentId = Guid.NewGuid().ToString();
            var comment = new Comment { Id = commentId };
            _commentRepositoryMock.Setup(repo => repo.GetById(commentId)).ReturnsAsync(comment);
            _commentRepositoryMock.Setup(repo => repo.Delete(comment)).ReturnsAsync(false);
            var commentService = new CommentService(_commentRepositoryMock.Object, _blogRepositoryMock.Object);

            // Act
            var result = await commentService.DeleteCommentAsync(commentId);

            // Assert
            Assert.IsFalse(result);
        }



    }
}
