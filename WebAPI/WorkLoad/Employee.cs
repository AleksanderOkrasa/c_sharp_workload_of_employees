using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkLoad
{
    
    public class Employee
    {
       
        public int Id { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }

        public ICollection<Task> tasks { get; } = new List<Task>();


    }
}
