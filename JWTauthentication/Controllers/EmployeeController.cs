using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTauthentication.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTauthentication.Controllers
{
    //The [Authorize] attribute indicates that only authenticated users can invoke Get() action.
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        //Now let's create an API that requires security. Add another API in the Controllers folder - EmployeeController. For the sake of simplicity we will have only GET method in it.
        public IActionResult Get()
        {
            //List<Employee> empdata = new List<Employee>()
            //{
            //    new Employee{ EmployeeID=1,FirstName="Nancy", LastName="Davolio"},
            //    new Employee{ EmployeeID=2,FirstName="Andrew", LastName="Smith"},
            //    new Employee{ EmployeeID=3,FirstName="Janet", LastName="Rollings"},
            //};

            List<Employee> empdata = new List<Employee>();
            empdata.Add(new Employee() { EmployeeID = 1, FirstName = "Nancy", LastName = "Davolio" });
            empdata.Add(new Employee() { EmployeeID = 2, FirstName = "Andrew", LastName = "Smith" });
            empdata.Add(new Employee() { EmployeeID = 3, FirstName = "Janet", LastName = "Rollings" });

            return new ObjectResult(empdata);
        }
    }
}