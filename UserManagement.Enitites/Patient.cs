using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Enitites
{
    public class Patient:BaseEntity
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public Address Address { get; set; }
    }

    public class Address:BaseEntity
    {
        public string ZipCode { get; set; }        
    }
}
