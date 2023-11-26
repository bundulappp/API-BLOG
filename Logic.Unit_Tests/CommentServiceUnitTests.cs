using Models.Interfaces;
using Moq;

namespace Logic.Unit_Tests
{
    [TestFixture]
    public class CommentServiceUnitTests
    {
        private Mock<IBlogRepository> _blogRepositoryMock;
        private Mock<ICommentRepository> _commentRepository;
        [SetUp]
        public void Setup()
        {
            _blogRepositoryMock = new Mock<IBlogRepository>();
            _commentRepository = new Mock<ICommentRepository>();
        }

    }
}
