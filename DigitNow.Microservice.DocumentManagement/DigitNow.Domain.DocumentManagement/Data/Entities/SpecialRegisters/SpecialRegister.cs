﻿
namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class SpecialRegister : ExtendedEntity
    {
        public string Name { get; set; }
        public string Observations { get; set; }
        public int DocumentCategoryId { get; set; }
    }
}