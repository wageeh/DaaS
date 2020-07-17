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
    public class PatientController : ControllerBase
    {
        private readonly IDocumentDBRepository<Patient> respository;
        private PatientManager patientManager;
        public PatientController(IDocumentDBRepository<Patient> _respository)
        {
            respository = _respository;
            patientManager = new PatientManager(respository);
        }
        
        [HttpGet]
        public async Task<ActionResult> ListAsync([FromQuery]string name = "")
        {
            List<Patient> patients = await patientManager.FilterByNameAsync(name);
            return new OkObjectResult(patients);
        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync( Patient item)
        {
            if (ModelState.IsValid)
            {
                Patient newpatient = await patientManager.CreateAsync(item);
                return new OkObjectResult(newpatient);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(Patient item)
        {
            if (ModelState.IsValid)
            {
                Patient updatedpatient = await patientManager.UpdateAsync(item.ItemId, item);
                return new OkObjectResult(updatedpatient);
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
                await patientManager.DeleteAsync(id);

                return new OkResult();
            }
            catch (Exception ex)
            {
                //Log(ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("~/Details")]
        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            Patient item = await patientManager.GetAsync(id);
            return new OkObjectResult(item);
        }
    }
}
