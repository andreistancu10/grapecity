using System;
using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class RegistrationNumberCounter : Entity
    {
        public RegistrationNumberCounter()
        {
        }
        public RegistrationNumberCounter(long id)
        {
            Id = id;
        }
        public int RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
