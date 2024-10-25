namespace SharpAgent.Domain.Common;

internal class BaseEntity : IEntity
{
    public Guid Id { get; set; }
    
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    
    public bool IsDeleted { get; set; }
}
