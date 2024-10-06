namespace Khuyaway.EntityFrameworkCore;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
}