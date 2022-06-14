using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Data.RegistrationNumberCounter.Seed
{
    public class Data
    {
        public static IEnumerable<RegistrationNoCounter.RegistrationNumberCounter> GetRegistrationNumberInitialValue()
        {
            return new[]
            {
                new RegistrationNoCounter.RegistrationNumberCounter(1)
                {
                    RegistrationNumber = 0,
                    RegistrationDate = DateTime.Now
                }
            };
        }
    }
}
