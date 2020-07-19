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
        private readonly ICosmosDBRepository<Appointment> Respository;
        public AppointmentManager(ICosmosDBRepository<Appointment> _respository)
        {
            Respository = _respository;
        }

        public async Task<Appointment> CreateAsync(Appointment newappointment)
        {
            newappointment.CreatedDate = DateTime.UtcNow;
            newappointment.HasConflict = false;
            newappointment.EntityId = Guid.NewGuid();
            var item = await Respository.CreateItemAsync(newappointment, newappointment.DoctorId);
            return (Appointment)item;
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
            await Respository.DeleteItemAsync(id, item.DoctorId);
        }

        public async Task<List<Appointment>> ListAsync()
        {
            List<Appointment> appointments = (await Respository.GetAllItemsAsync()).ToList();
            return appointments;
        }

        public async Task<List<Appointment>> FilterByDoctorIdPatientIdAsync(string doctorid,string patientid,string from="",string to="")
        {
            DateTime fromDate, toDate;
            if (!DateTime.TryParse(from,out fromDate))
            {
                fromDate = DateTime.UtcNow.Date;
            }
            if (!DateTime.TryParse(to, out toDate))
            {
                toDate = DateTime.UtcNow.Date.AddDays(2);
            }
            List<Appointment> appointments;
            if (doctorid!="" && patientid!="")
            {
                appointments = (await Respository.GetItemsAsync(x => x.DoctorId==doctorid && x.PatientId == patientid && x.AppointmentDate>=fromDate && x.AppointmentDate <=toDate)).ToList();
            }
            else if(doctorid != "")
            {
                appointments = (await Respository.GetItemsAsync(x => x.DoctorId == doctorid && x.AppointmentDate >= fromDate && x.AppointmentDate <= toDate)).ToList();
            }
            else if (patientid != "")
            {
                appointments = (await Respository.GetItemsAsync(x => x.PatientId == patientid && x.AppointmentDate >= fromDate && x.AppointmentDate <= toDate)).ToList();
            }
            else
            {
                appointments = (await Respository.GetItemsAsync(x=> x.AppointmentDate >= fromDate && x.AppointmentDate <= toDate)).ToList();
            }
            return appointments;
        }

        public async Task<List<Appointment>> GetAllItemsAsync()
        {
            return (await Respository.GetAllItemsAsync()).ToList();
        }

        public async Task<Appointment> GetAsync(string id)
        {
            return await Respository.GetItemAsync(id);
        }
    }
}
