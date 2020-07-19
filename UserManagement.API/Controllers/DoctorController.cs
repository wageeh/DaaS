using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.API.BAL;
using UserManagement.Enitites;

namespace UserManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly ICosmosDBRepository<Doctor> respository;
        private DoctorManager doctorManager;
        public DoctorController(ICosmosDBRepository<Doctor> _respository)
        {
            respository = _respository;
            doctorManager = new DoctorManager(respository);
        }
        
        [HttpGet]
        public async Task<ActionResult> ListAsync([FromQuery]string name = "")
        {
            List<Doctor> doctors = await doctorManager.FilterByNameAsync(name);
            return new OkObjectResult(doctors);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync( Doctor item)
        {
            if (ModelState.IsValid)
            {
                Doctor newdoctor = await doctorManager.CreateAsync(item);
                return new OkObjectResult(newdoctor);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync( Doctor item)
        {
            if (ModelState.IsValid)
            {
                Doctor updateddoctor = await doctorManager.UpdateAsync(item.ItemId.ToString(), item);
                return new OkObjectResult(updateddoctor);
            }
            else
            {
                return BadRequest();
            }
        }


        // for the sake of creating some call without using validation 
        [HttpDelete]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                await doctorManager.DeleteAsync(id);

                return new OkResult();
            }
            catch (Exception ex)
            {
                //Log(ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            Doctor item = await doctorManager.GetAsyncByDoctorId(id);
            return new OkObjectResult(item);
        }
    }
}
