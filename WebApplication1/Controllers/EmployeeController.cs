using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.IRepository;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly YourDbContext _dbContext;
        private readonly ICacheService _cacheService;
        public EmployeeController(YourDbContext dbContext, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        //[HttpGet("Employees")]
        [HttpGet("Employees")]
        public IEnumerable<Table2> GetEmployees()
        {
            var cachekey = $"Employee";
            var cacheData = _cacheService.GetData<IEnumerable<Table2>>(cachekey);
            if (cacheData != null)
            {
                return cacheData;
               
            } 
            var expirationTime = DateTimeOffset.Now.AddMinutes(60.0);
             cacheData = _dbContext.Table2s.ToList();
             _cacheService.SetData<IEnumerable<Table2>>("Employee", cacheData, expirationTime);
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
        public IActionResult Put(Table2 employee)
        {
            _dbContext.Table2s.Update(employee);
            _cacheService.RemoveData("employee");
            _dbContext.SaveChanges();
            //update the cache
            var expirationTime = DateTimeOffset.Now.AddMinutes(60.0);
           var cacheData = _dbContext.Table2s.ToList();
            _cacheService.SetData<IEnumerable<Table2>>("employeed", cacheData, expirationTime);
            return Ok(cacheData);

        }
        [HttpDelete("deleteCustomer")]
        public IActionResult Delete(int Id)
        {
            var filteredData = _dbContext.Table2s.Where(x => x.Id == Id).FirstOrDefault();
            _dbContext.Remove(filteredData);
            _cacheService.RemoveData("filteredData");
            _dbContext.SaveChanges();
            return Ok(filteredData);
        }
    }
}