namespace EmployeeInfo.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public decimal MaxSalary { get; set; }
        public decimal MinSalary { get; set; }
    }
}
