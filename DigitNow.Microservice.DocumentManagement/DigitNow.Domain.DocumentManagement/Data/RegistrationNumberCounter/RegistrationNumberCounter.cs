
using HTSS.Platform.Core.Domain;
using System;

namespace DigitNow.Domain.DocumentManagement.Data.RegistrationNoCounter
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
