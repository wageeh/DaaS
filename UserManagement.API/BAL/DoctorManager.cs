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
        private readonly IDocumentDBRepository<Doctor> Respository;
        public DoctorManager(IDocumentDBRepository<Doctor> _respository)
        {
            Respository = _respository;
        }

        public async Task<Doctor> CreateAsync(Doctor newdoctor)
        {
            newdoctor.CreatedDate = DateTime.UtcNow;
            var item = await Respository.CreateItemAsync(newdoctor);
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
            await Respository.DeleteItemAsync(id);
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
    }
}
