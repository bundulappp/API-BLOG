using Data.Data;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
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
                   .ReturnsAsync(new Blog { UserId = userId });

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
                   .ReturnsAsync(new Blog());

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
                   .ReturnsAsync((Blog)null);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.UserOwnsBlogAsync(blogId, userId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllAsync_WithoutAnySearchParam_ShouldReturnListOfBlogs()
        {
            //Arrange
            var blogList = new List<Blog>
                {
                    new Blog { Id = Guid.NewGuid().ToString()},
                    new Blog { Id = Guid.NewGuid().ToString()},
                    new Blog { Id = Guid.NewGuid().ToString()},
                };

            _blogRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<PaginationFilter>())).ReturnsAsync(blogList);


            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            //Act
            var result = await blogService.GetAllAsync();

            //Assert 
            Assert.That(result, Is.EquivalentTo(blogList));
        }

        [Test]
        public async Task GetAllAsync_MadeByTheCurrentUser_ShouldReturnListOfBlogs()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var blogList = new List<Blog>
                {
                    new Blog { Id = Guid.NewGuid().ToString(), UserId = userId },
                    new Blog { Id = Guid.NewGuid().ToString(),UserId = userId},
                    new Blog { Id = Guid.NewGuid().ToString(),UserId = userId},
                };

            _blogRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<PaginationFilter>())).ReturnsAsync(blogList);


            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            //Act
            var result = await blogService.GetAllAsync(userId);

            //Assert 
            Assert.That(result, Is.EquivalentTo(blogList));
        }

        [Test]
        public async Task GetAllAsync_FilteredByPagination_ShouldReturnsAsyncubsetOfBlogs()
        {
            //Arrange
            var paginationFilter = new PaginationFilter { PageNumber = 1, PageSize = 5 };
            var userId = Guid.NewGuid().ToString();
            var blogList = new List<Blog>
                {
                    new Blog { Id = Guid.NewGuid().ToString()},
                    new Blog { Id = Guid.NewGuid().ToString()},
                    new Blog { Id = Guid.NewGuid().ToString()},
                };

            _blogRepositoryMock.Setup(repo => repo.GetAll(paginationFilter)).ReturnsAsync(blogList);


            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            //Act
            var result = await blogService.GetAllAsync(null, paginationFilter);

            //Assert 
            Assert.That(result, Is.EquivalentTo(blogList));
        }

        [Test]
        public async Task GetAllAsync_BlogsTableIsEmpty_ShouldReturnEmptyListOfBlogs()
        {
            //Arrange
            var blogList = new List<Blog>();

            _blogRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<PaginationFilter>())).ReturnsAsync(blogList);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);
            //Act
            var result = await blogService.GetAllAsync();
            //Assert
            Assert.That(result, Is.EquivalentTo(blogList));
        }

        [Test]
        public async Task GetByIdAsync_BlogNotFound_ShouldReturnNull()
        {
            //Arrange
            var blogId = Guid.NewGuid().ToString();
            _blogRepositoryMock.Setup(repo => repo.GetById(blogId)).ReturnsAsync((Blog)null);
            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            //Act
            var result = await blogService.GetByIdAsync(blogId);
            //Assert
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public async Task GetByIdAsync_BlogHasFound_ShouldReturnBlogEntity()
        {
            //Arrange
            var blogId = Guid.NewGuid().ToString();
            var blogEntity = new Blog { Id = blogId };
            _blogRepositoryMock.Setup(repo => repo.GetById(blogId)).ReturnsAsync(blogEntity);
            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);
            //Act
            var result = await blogService.GetByIdAsync(blogId);
            //Assert
            Assert.That(result, Is.EqualTo(blogEntity));
        }

        [Test]
        public async Task CreateBlogAsync_SomeOfThePropertiesMissing_ShouldReturnFalse()
        {
            // Arrange
            var incompleteBlog = new Blog
            {
                // Missing userId
                Name = "Sample blog content",
                Body = "Blog test content",
                Tags = new List<BlogTag>()
            };

            _blogRepositoryMock.Setup(repo => repo.Insert(incompleteBlog)).ReturnsAsync(false);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            //Act
            var result = await blogService.CreateBlogAsync(incompleteBlog);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateBlogAsync_WithTags_ShouldReturnTrue()
        {
            // Arrange
            var blog = new Blog
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Teszt Blog",
                UserId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Body = "Ez egy teszt blog bejegyzés.",
                Tags = new List<BlogTag>
                    {
                        new BlogTag
                        {
                            Id = Guid.NewGuid().ToString(),
                            TagId = "Tag1",
                            Tag = new Tag
                            {
                                Name = "Tag1",
                                UserId = Guid.NewGuid().ToString(),
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                            }
                        },
                        new BlogTag
                        {
                            Id = Guid.NewGuid().ToString(),
                            TagId = "Tag2",
                            Tag = new Tag
                            {
                                Name = "Tag2",
                                UserId = Guid.NewGuid().ToString(),
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                            }
                        }
                    }
            };

            _blogRepositoryMock.Setup(repo => repo.Insert(blog)).ReturnsAsync(true);
            foreach (var tag in blog.Tags)
            {
                _tagRepositoryMock.Setup(repo => repo.GetById(tag.TagId)).ReturnsAsync((Tag)null);
                _tagRepositoryMock.Setup(repo => repo.Insert(It.IsAny<Tag>())).Verifiable();
            }
            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.CreateBlogAsync(blog);

            // Assert
            Assert.IsTrue(result);
            foreach (var tag in blog.Tags)
            {
                _tagRepositoryMock.Verify(repo => repo.Insert(It.Is<Tag>(t => t.Name == tag.TagId)), Times.Once);
            }
        }


        [Test]
        public async Task CreateBlogAsync_TagsThatHaveAlreadyBeenAddedToTheDb_ShouldReturnTrue()
        {
            // Arrange
            var blog = new Blog
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Teszt Blog",
                UserId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Body = "Ez egy teszt blog bejegyzés.",
                Tags = new List<BlogTag>
        {
            new BlogTag
            {
                Id = Guid.NewGuid().ToString(),
                TagId = "ExistingTag1",
                Tag = new Tag
                {
                    Name = "ExistingTag1",
                    UserId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            },
            new BlogTag
            {
                Id = Guid.NewGuid().ToString(),
                TagId = "ExistingTag2",
                Tag = new Tag
                {
                    Name = "ExistingTag2",
                    UserId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            }
        }
            };

            _blogRepositoryMock.Setup(repo => repo.Insert(blog)).ReturnsAsync(true);
            foreach (var tag in blog.Tags)
            {
                _tagRepositoryMock.Setup(repo => repo.GetById(tag.TagId)).ReturnsAsync(new Tag { Name = tag.TagId });
            }

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            //Act
            var result = await blogService.CreateBlogAsync(blog);

            //Assert
            Assert.IsTrue(result);
        }


        [Test]
        public async Task CreateBlogAsync_DbInsertFails_ShouldReturnFalse()
        {
            // Arrange
            var blog = new Blog
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Teszt Blog",
                UserId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Body = "Ez egy teszt blog bejegyzés.",
                Tags = new List<BlogTag>
        {
            new BlogTag
            {
                Id = Guid.NewGuid().ToString(),
                TagId = "Tag1",
                Tag = new Tag
                {
                    Name = "Tag1",
                    UserId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            }
        }
            };

            // Beállítjuk, hogy a blog beszúrása sikertelen legyen
            _blogRepositoryMock.Setup(repo => repo.Insert(blog)).ReturnsAsync(false);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.CreateBlogAsync(blog);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateBlogAsync_ExistingBlog_ShouldReturnTrue()
        {
            // Arrange
            var existingBlogId = Guid.NewGuid().ToString();
            var existingBlog = new Blog
            {
                Id = existingBlogId,
                Name = "Existing Blog",
                UserId = Guid.NewGuid().ToString(),
                Body = "Original blog content",
            };
            var updatedBlog = new Blog
            {
                Id = existingBlogId,
                Name = "Updated Blog",
                UserId = existingBlog.UserId,
                Body = "Updated blog content",
            };

            _blogRepositoryMock.Setup(repo => repo.GetById(existingBlogId)).ReturnsAsync(existingBlog);
            _blogRepositoryMock.Setup(repo => repo.Update(updatedBlog)).ReturnsAsync(true);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.UpdateBlogAsync(updatedBlog);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateBlogAsync_NonExistingBlog_ShouldReturnFalse()
        {
            // Arrange
            var nonExistingBlogId = Guid.NewGuid().ToString();
            var nonExistingBlog = new Blog
            {
                Id = nonExistingBlogId,
                Name = "Non-existing Blog",
                UserId = Guid.NewGuid().ToString(),
                Body = "Non-existing blog content",
                // További szükséges tulajdonságok inicializálása
            };

            _blogRepositoryMock.Setup(repo => repo.GetById(nonExistingBlogId)).ReturnsAsync((Blog)null);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.UpdateBlogAsync(nonExistingBlog);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateBlogAsync_DbUpdateFails_ShouldReturnFalse()
        {
            // Arrange
            var existingBlogId = Guid.NewGuid().ToString();
            var existingBlog = new Blog
            {
                Id = existingBlogId,
                Name = "Existing Blog",
                UserId = Guid.NewGuid().ToString(),
                Body = "Original blog content",
                // További szükséges tulajdonságok inicializálása
            };
            var updatedBlog = new Blog
            {
                Id = existingBlogId,
                Name = "Updated Blog",
                UserId = existingBlog.UserId,
                Body = "Updated blog content",
                // Frissített tulajdonságok
            };

            _blogRepositoryMock.Setup(repo => repo.GetById(existingBlogId)).ReturnsAsync(existingBlog);
            _blogRepositoryMock.Setup(repo => repo.Update(updatedBlog)).ReturnsAsync(false); // Sikertelen frissítés szimulálása

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.UpdateBlogAsync(updatedBlog);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteBlogAsync_ExistingBlog_ShouldReturnTrue()
        {
            // Arrange
            var existingBlogId = Guid.NewGuid().ToString();
            var existingBlog = new Blog
            {
                Id = existingBlogId,
                Name = "Existing Blog",
                UserId = Guid.NewGuid().ToString(),
                Body = "Blog content",
            };

            _blogRepositoryMock.Setup(repo => repo.GetById(existingBlogId)).ReturnsAsync(existingBlog);
            _blogRepositoryMock.Setup(repo => repo.Delete(existingBlog)).ReturnsAsync(true);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.DeleteBlogAsync(existingBlogId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteBlogAsync_NonExistingBlog_ShouldReturnFalse()
        {
            // Arrange
            var nonExistingBlogId = Guid.NewGuid().ToString();

            _blogRepositoryMock.Setup(repo => repo.GetById(nonExistingBlogId)).ReturnsAsync((Blog)null);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.DeleteBlogAsync(nonExistingBlogId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteBlogAsync_DbDeleteFails_ShouldReturnFalse()
        {
            // Arrange
            var existingBlogId = Guid.NewGuid().ToString();
            var existingBlog = new Blog
            {
                Id = existingBlogId,
                Name = "Existing Blog",
                UserId = Guid.NewGuid().ToString(),
                Body = "Blog content"
                // További szükséges tulajdonságok inicializálása
            };

            _blogRepositoryMock.Setup(repo => repo.GetById(existingBlogId)).ReturnsAsync(existingBlog);
            _blogRepositoryMock.Setup(repo => repo.Delete(existingBlog)).ReturnsAsync(false); // Sikertelen törlés szimulálása

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.DeleteBlogAsync(existingBlogId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetTagByIdAsync_ExistingTag_ShouldReturnTag()
        {
            // Arrange
            var tagId = Guid.NewGuid().ToString();
            var expectedTag = new Tag { Name = tagId };
            _tagRepositoryMock.Setup(repo => repo.GetById(tagId)).ReturnsAsync(expectedTag);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.GetTagByIdAsync(tagId);

            // Assert
            Assert.AreEqual(expectedTag, result);
        }

        [Test]
        public async Task GetTagByIdAsync_NonExistingTag_ShouldReturnNull()
        {
            // Arrange
            var tagId = Guid.NewGuid().ToString();
            _tagRepositoryMock.Setup(repo => repo.GetById(tagId)).ReturnsAsync((Tag)null);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.GetTagByIdAsync(tagId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetTagByIdAsync_EmptyId_ShouldReturnNull()
        {
            // Arrange
            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.GetTagByIdAsync(string.Empty);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllTagsAsync_ShouldReturnAllTags()
        {
            // Arrange
            var tags = new List<Tag> { new Tag { Name = "Tag1" }, new Tag { Name = "Tag2" } };
            _tagRepositoryMock.Setup(repo => repo.GetAll(null)).ReturnsAsync(tags);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.GetAllTagsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
            CollectionAssert.AreEqual(tags, result); // CollectionAssert use for asser collections
        }

        [Test]
        public async Task GetAllTagsAsync_NoTagsInDb_ShouldReturnEmptyList()
        {
            // Arrange
            var tags = new List<Tag>();
            _tagRepositoryMock.Setup(repo => repo.GetAll(null)).ReturnsAsync(tags);
            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.GetAllTagsAsync();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetAllTagsAsync_DbError_ShouldThrowException()
        {
            // Arrange
            _tagRepositoryMock.Setup(repo => repo.GetAll(null)).Throws(new Exception("Database error"));
            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await blogService.GetAllTagsAsync());
        }

        [Test]
        public async Task CreateSingleTagAsync_NonExistingTag_ShouldReturnTrue()
        {
            // Arrange
            var newTag = new Tag { Name = "NewTag" };
            _tagRepositoryMock.Setup(repo => repo.GetById("NewTag")).ReturnsAsync((Tag)null);
            _tagRepositoryMock.Setup(repo => repo.Insert(newTag)).ReturnsAsync(true);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);


            // Act
            var result = await blogService.CreateSingleTagAsync(newTag);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CreateSingleTagAsync_ExistingTag_ShouldReturnFalse()
        {
            // Arrange
            var existingTag = new Tag { Name = "ExistingTag" };
            _tagRepositoryMock.Setup(repo => repo.GetById("ExistingTag")).ReturnsAsync(existingTag);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.CreateSingleTagAsync(existingTag);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateSingleTagAsync_DbInsertFails_ShouldReturnFalse()
        {
            // Arrange
            var newTag = new Tag { Name = "NewTag" };
            _tagRepositoryMock.Setup(repo => repo.GetById("NewTag")).ReturnsAsync((Tag)null);
            _tagRepositoryMock.Setup(repo => repo.Insert(newTag)).ReturnsAsync(false);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.CreateSingleTagAsync(newTag);

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task DeleteTagAsync_ExistingTag_ShouldReturnTrue()
        {
            // Arrange
            var existingTagId = "ExistingTag";
            var existingTag = new Tag { Name = existingTagId };
            _tagRepositoryMock.Setup(repo => repo.GetById(existingTagId)).ReturnsAsync(existingTag);
            _tagRepositoryMock.Setup(repo => repo.Delete(existingTag)).ReturnsAsync(true);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.DeleteTagAsync(existingTagId);

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task DeleteTagAsync_NonExistingTag_ShouldReturnFalse()
        {
            // Arrange
            var nonExistingTagId = "NonExistingTag";
            _tagRepositoryMock.Setup(repo => repo.GetById(nonExistingTagId)).ReturnsAsync((Tag)null);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.DeleteTagAsync(nonExistingTagId);

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task DeleteTagAsync_DbDeleteFails_ShouldReturnFalse()
        {
            // Arrange
            var existingTagId = "ExistingTag";
            var existingTag = new Tag { Name = existingTagId };
            _tagRepositoryMock.Setup(repo => repo.GetById(existingTagId)).ReturnsAsync(existingTag);
            _tagRepositoryMock.Setup(repo => repo.Delete(existingTag)).ReturnsAsync(false);

            var blogService = new BlogService(_blogDbContextMock.Object, _blogRepositoryMock.Object, _tagRepositoryMock.Object);

            // Act
            var result = await blogService.DeleteTagAsync(existingTagId);

            // Assert
            Assert.IsFalse(result);
        }

    }
}
