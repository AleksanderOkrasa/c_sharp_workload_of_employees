using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Model
{
    public class EmployeeModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
