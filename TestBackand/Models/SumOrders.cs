namespace TestBackand.Models
{
    public class SumOrders
    {
        public int Sum { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public SumOrders(int sum, string name, DateTime birthday)
        {
            Sum = sum;
            Name = name;
            Birthday = birthday;
        }
    }
}
