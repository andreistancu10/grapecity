﻿using DigitNow.Domain.DocumentManagement.Data.Entities.Forms;
using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class FormField : Entity
    {
        public FieldType FieldType { get; set; }
        public string Name { get; set; }
        public string Context { get; set; }

        #region [ References ]

        //public List<FormFieldMapping> FormFieldMappings { get; set; }

        #endregion

        public FormField()
        {
        }

        public FormField(long id)
        {
            Id = id;
        }
    }
}