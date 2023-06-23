namespace WorkLoad
{
    public class Task
    {
        public int Id { get; set; }
        public string description { get; set; }
        public int priority { get; set; }
        public int time { get; set; }
        public bool isDone { get; set; }
        public int? employeeID {get; set; }
        public Employee? employee { get; set; }
    }
}
