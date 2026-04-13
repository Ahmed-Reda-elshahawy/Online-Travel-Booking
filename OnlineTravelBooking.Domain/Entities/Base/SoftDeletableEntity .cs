namespace OnlineTravelBooking.Domain.Entities.Base;

public abstract class SoftDeletableEntity 
{
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted => DeletedAt.HasValue;
}
