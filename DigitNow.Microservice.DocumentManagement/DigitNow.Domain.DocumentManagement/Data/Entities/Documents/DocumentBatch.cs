﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data.Entities.Documents;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class DocumentBatch
    {
        public List<Document> Documents { get; set; }
    }
}
