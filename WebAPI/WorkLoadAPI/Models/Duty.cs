namespace WorkLoadAPI.Models
{
    public class Duty
    {
        public int Id { get; set; }
        public string DutyDescription { get; set; }
        public double Time { get; set; }
        public int Priority { get; set; }

        public int? EmployeeId { get; set; }
       

    }
}
