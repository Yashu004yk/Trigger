using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.IRepository;
using WebApplication1.db;
using WMS;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private dbContext db;
        private readonly ICacheService _cacheService;
        public EmployeeController()
        {
            db = new dbContext();
            _cacheService = new CacheService();
        }

        
        //[HttpGet("Employees")]
        [HttpGet("Employees")]
        public IEnumerable<Employee> GetEmployees()
        {
            var cachekey = $"Employee";

            var cacheData = _cacheService.GetData<IEnumerable<Employee>>(cachekey);
            if (cacheData != null)
            {
                return cacheData;
               
            } 
            var expirationTime = DateTimeOffset.Now.AddMinutes(60.0);
             cacheData = db.Employees.ToList();
             _cacheService.SetData<IEnumerable<Employee>>("Employee", cacheData, expirationTime);
            return cacheData;
        }
        ////* [HttpGet("oneCustomer")]
        //public Employeed Getone(int id)
        // {
        //     Employeed filteredData;
        //     var cacheData = _cacheService.GetData<IEnumerable<Employeed>>("Employeed");
        //     if (cacheData != null)
        //     {
        //         filteredData = cacheData.Where(x => x.Employeeid == id).FirstOrDefault();
        //         return filteredData;
        //     }
        //     filteredData = .Customers.Where(x => x.Customerid == id).FirstOrDefault();
        //     return filteredData;
        // }
        //*//*


        // [HttpPost("addEmploye")]
        // public async Task<Employeed> AddEmp([FromBody]Employeed employeed)
        // {
        //     var obj = await _dbContext.Employeeds.AddAsync(employeed);
        //     _dbContext.Employeeds.Add(employeed);
        //     _dbContext.SaveChanges();
        //     return obj.Entity;
        //     var cachedata = obj;
        //     var expirationTime = DateTimeOffset.Now.AddMinutes(60.0);
        //     _cacheService.SetData("employeed",cachedata,expirationTime);
        // }


        [HttpPut("updateproduct")]
        public IActionResult Put(Employee employee)
        {
            
            var cachekey = $"Employee";
            db.Employees.Update(employee);
           // _cacheService.RemoveData(cachekey);
            db.SaveChanges();
            var expirationTime = DateTimeOffset.Now.AddMinutes(60.0);
           var cacheData = db.Employees.ToList();
            _cacheService.SetData<IEnumerable<Employee>>("Employee", cacheData, expirationTime);
            return Ok();

        }
        [HttpDelete("deleteCustomer")]
        public IActionResult Delete(int Id)
        {
            var filteredData = db.Employees.Where(x => x.Employeeid == Id).FirstOrDefault();
            db.Remove(filteredData);
            _cacheService.RemoveData("filteredData");
          db.SaveChanges();
            return Ok(filteredData);
        }
    }
}