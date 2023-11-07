namespace Models.Interfaces
{
    public interface IUnitOfWork
    {
        IBlogRepository BlogRepository { get; }
        ITagRepository TagRepository { get; }
    }
}
