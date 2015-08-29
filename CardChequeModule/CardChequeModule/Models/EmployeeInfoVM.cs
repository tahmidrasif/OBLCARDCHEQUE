using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardChequeModule.Models
{
    public class EmployeeInfoVM
    {
        
         public string Name { get; set; }
         public string BranchName { get; set; }
         public string Email { get; set; }
         public string JobTitle { get; set; }
         public string EmployeeId { get; set; }
         public string PreDeptName { get; set; }
    }
}