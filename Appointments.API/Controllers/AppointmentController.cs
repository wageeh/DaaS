using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appointments.API.BAL;
using Appointments.Entities;
using Core.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly ICosmosDBRepository<Appointment> respository;
        private AppointmentManager appointmentsManager;
        public AppointmentController(ICosmosDBRepository<Appointment> _respository)
        {
            respository = _respository;
            appointmentsManager = new AppointmentManager(respository);
        }
        
        [HttpGet]
        public async Task<ActionResult> ListAsync([FromQuery]string doctorId = "",string patientId="", string from = "", string to = "")
        {
            List<Appointment> appointments = await appointmentsManager.FilterByDoctorIdPatientIdAsync(doctorId,patientId,from,to);
            return new OkObjectResult(appointments);
        }


        [HttpPost]
        [ActionName("Create")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Appointment item)
        {
            if (ModelState.IsValid)
            {
                Appointment newdoctor = await appointmentsManager.CreateAsync(item);
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
        public async Task<ActionResult> EditAsync([Bind("Id,Name")] Appointment item)
        {
            if (ModelState.IsValid)
            {
                Appointment updateddoctor = await appointmentsManager.UpdateAsync(item.EntityId, item);
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
                await appointmentsManager.DeleteAsync(id);

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
            Appointment item = await appointmentsManager.GetAsync(id);
            return new OkObjectResult(item);
        }
    }
}
