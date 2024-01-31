namespace TestBackand.Models
{
    public class AvgOrders
    {
        public TimeSpan Date { get; set; }
        public double Avg { get; set; }
        public AvgOrders(TimeSpan date, double avg)
        {
            Date = date;
            Avg = avg;
        }
    }
}
