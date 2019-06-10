namespace WebApiNHibernateCrudPagination.Entities
{
    public class Todo : TimestampedEntity
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual bool Completed { get; set; }
    }
}