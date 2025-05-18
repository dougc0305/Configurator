namespace Common
{
    public interface IChangeTracker : IChangeTrackerDates
    {
        int CreatedById { get; set; }
        int ModifiedById { get; set; }
    }

    public interface IChangeTrackerDates
    {
        DateTime CreatedDate { get; set; }
        DateTime ModifiedDate { get; set; }
    }


    public interface IChangeTrackerNullableDates
    {
        DateTime? CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
    }
}
