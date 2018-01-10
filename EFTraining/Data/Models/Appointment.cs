using System;
using Newtonsoft.Json;

namespace EFTraining.Data.Models
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }

        public DateTime AppointmentDate { get; set; }
        [JsonIgnore]
        public Contact Guest { get; set; }
        [JsonIgnore]       
        public Employee Host { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
