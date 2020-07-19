using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Enitites;

namespace UserManagement.API.BAL
{
    public class DoctorManager
    {
        private readonly ICosmosDBRepository<Doctor> Respository;
        public DoctorManager(ICosmosDBRepository<Doctor> _respository)
        {
            Respository = _respository;
        }

        public async Task<Doctor> CreateAsync(Doctor newdoctor)
        {
            newdoctor.CreatedDate = DateTime.UtcNow;
            newdoctor.EntityId = Guid.NewGuid();
            var item = await Respository.CreateItemAsync(newdoctor,newdoctor.Speciality.Id);
            return (Doctor)(dynamic)item;
        }

        public async Task<Doctor> UpdateAsync(string id, Doctor updateddoctor)
        {
            var item = await Respository.UpdateItemAsync(id, updateddoctor);
            return (Doctor)(dynamic)item;
        }

        public async Task DeleteAsync(string id)
        {
            Doctor item = await Respository.GetItemAsync(id);
            // just using the manager to do something extra

            if (item == null)
            {
                throw new Exception("Id can not be found");
            }
            await Respository.DeleteItemAsync(id, item.Speciality.Id);
        }

        public async Task<List<Doctor>> ListAsync()
        {
            List<Doctor> doctors = (await Respository.GetAllItemsAsync()).ToList();
            return doctors;
        }

        public async Task<List<Doctor>> FilterByNameAsync(string searchname)
        {
            List<Doctor> doctors = (await Respository.GetItemsAsync(x => x.Name.ToLower().Contains(searchname.ToLower()))).ToList();
            return doctors;
        }

        public async Task<Doctor> GetAsync(string id)
        {
            return await Respository.GetItemAsync(id);
        }

        public async Task<Doctor> GetAsyncByDoctorId(string doctorid)
        {
            var doctor = (await Respository.GetItemsAsync(x => x.ItemId == doctorid)).FirstOrDefault();
            return doctor;
        }
    }
}
