using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Enitites
{
    public class Doctor:BaseEntity
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Speciality Speciality { get; set; }
    }

    public class Speciality
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
