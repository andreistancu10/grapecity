using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Data.RegistrationNumberCounter.Seed
{
    public class Data
    {
        public static IEnumerable<RegistrationNumberCounter> GetRegistrationNumberInitialValue()
        {
            return new[]
            {
                new RegistrationNumberCounter(1)
                {
                    RegistrationNumber = 0,
                    RegistrationDate = DateTime.Now
                }
            };
        }
    }
}
