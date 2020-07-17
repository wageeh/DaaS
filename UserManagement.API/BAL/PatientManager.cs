using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Enitites;

namespace UserManagement.API.BAL
{
    public class PatientManager
    {
        private readonly IDocumentDBRepository<Patient> Respository;
        public PatientManager(IDocumentDBRepository<Patient> _respository)
        {
            Respository = _respository;
        }

        public async Task<Patient> CreateAsync(Patient newpatient)
        {
            newpatient.CreatedDate = DateTime.UtcNow;
            var item = await Respository.CreateItemAsync(newpatient);
            return (Patient)(dynamic)item;
        }

        public async Task<Patient> UpdateAsync(string id, Patient updatedpatient)
        {
            var item = await Respository.UpdateItemAsync(id, updatedpatient);
            return (Patient)(dynamic)item;
        }

        public async Task DeleteAsync(string id)
        {
            Patient item = await Respository.GetItemAsync(id);
            // just using the manager to do something extra

            if (item == null)
            {
                throw new Exception("Id can not be found");
            }
            await Respository.DeleteItemAsync(id);
        }

        public async Task<List<Patient>> ListAsync()
        {
            List<Patient> patients = (await Respository.GetAllItemsAsync()).ToList();
            return patients;
        }

        public async Task<List<Patient>> FilterByNameAsync(string searchname)
        {
            List<Patient> patients = (await Respository.GetItemsAsync(x => x.Name.ToLower().Contains(searchname.ToLower()))).ToList();
            return patients;
        }

        public async Task<Patient> GetAsync(string id)
        {
            return await Respository.GetItemAsync(id);
        }
    }
}
