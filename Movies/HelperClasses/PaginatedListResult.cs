namespace Movies.HelperClasses
{
    public class PaginatedListResult<T>
    {
        public List<T> Items { get; set; }
        public int CountAll { get; set; }
    }
}
