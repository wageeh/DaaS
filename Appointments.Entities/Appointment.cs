using System;
using System.Collections.Generic;
using System.Text;

namespace Appointments.Entities
{
    public class Appointment : BaseEntity
    {
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        // assuming that each time slot is 30 min as an average for the appointment and the working day is 12 hours shifts so 24 slot
        public int AppointmentTimeSlot { get; set; }
        public bool HasConflict { get; set; }
    }
}
