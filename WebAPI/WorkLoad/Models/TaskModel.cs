namespace WorkLoad.Models
{
    public class TaskModel
    {
        public int Id { get; set; } 
        public string TaskDescription { get; set; }
        public double Time { get; set; }
        public int Priorty { get; set; }  

        //OneToMany relation with employee
        public int? EmployeeId { get; set; } 
        public EmployeeModel? Employee { get; set; } 
    }
}
