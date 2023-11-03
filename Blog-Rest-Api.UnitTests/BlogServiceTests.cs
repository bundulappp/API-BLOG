using blog_rest_api.Data;
using blog_rest_api.Domain;
using blog_rest_api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;
using Xunit;

namespace Blog_Rest_Api.UnitTests
{
    public class BlogServiceTests
    {
        private readonly DbContextOptions<BlogDbContext> _dbContextOptions;
        public BlogServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        }
        [Fact]
        public async void CreateSingleTagAsync_IsAlreadyExist_ReturnsFalse()
        {
            // Arrange
            using (var context = new BlogDbContext(_dbContextOptions))
            {
                var service = new BlogService(context);
                var tag = new Tag { Name = "ExistingTag", UserId = "TestUserId" };
                context.Tags.Add(tag);
                await context.SaveChangesAsync();
            }

            // Act & Assert
            using (var context = new BlogDbContext(_dbContextOptions))
            {
                var service = new BlogService(context);
                var newTag = new Tag { Name = "ExistingTag", UserId = "TestUserId" };

                // Act
                var result = await service.CreateSingleTagAsync(newTag);

                // Assert
                Assert.False(result);
            }
        }
    }
}