using Appointments.Entities;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments.API.BAL
{
    public class AppointmentManager
    {
        private readonly IDocumentDBRepository<Appointment> Respository;
        public AppointmentManager(IDocumentDBRepository<Appointment> _respository)
        {
            Respository = _respository;
        }

        public async Task<Appointment> CreateAsync(Appointment newappointment)
        {
            newappointment.CreatedDate = DateTime.UtcNow;
            newappointment.HasConflict = false;
            newappointment.EntityId = Guid.NewGuid();
            var item = await Respository.CreateItemAsync(newappointment);
            return (Appointment)(dynamic)item;
        }

        public async Task<Appointment> UpdateAsync(Guid id, Appointment updatedappointment)
        {
            var item = await Respository.UpdateItemAsync(id.ToString(), updatedappointment);
            return (Appointment)(dynamic)item;
        }

        public async Task DeleteAsync(string id)
        {
            Appointment item = await Respository.GetItemAsync(id);
            // just using the manager to do something extra

            if (item == null)
            {
                throw new Exception("Id can not be found");
            }
            await Respository.DeleteItemAsync(id);
        }

        public async Task<List<Appointment>> ListAsync()
        {
            List<Appointment> appointments = (await Respository.GetAllItemsAsync()).ToList();
            return appointments;
        }

        public async Task<List<Appointment>> FilterByDoctorIdPatientIdAsync(string doctorid,string patientid)
        {
            List<Appointment> appointments;
            if (doctorid!="" && patientid!="")
            {
                appointments = (await Respository.GetItemsAsync(x => x.DoctorId==doctorid && x.PatientId == patientid)).ToList();
            }
            else if(doctorid != "")
            {
                appointments = (await Respository.GetItemsAsync(x => x.DoctorId == doctorid)).ToList();
            }
            else if (patientid != "")
            {
                appointments = (await Respository.GetItemsAsync(x => x.PatientId == patientid)).ToList();
            }
            else
            {
                appointments = (await Respository.GetAllItemsAsync()).ToList();
            }
            return appointments;
        }

        public async Task<Appointment> GetAsync(string id)
        {
            return await Respository.GetItemAsync(id);
        }
    }
}
