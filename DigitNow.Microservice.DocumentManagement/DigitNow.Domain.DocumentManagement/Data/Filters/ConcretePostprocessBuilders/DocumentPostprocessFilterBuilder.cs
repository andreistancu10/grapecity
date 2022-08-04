﻿using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteBuilders
{
    internal class DocumentPostprocessFilterBuilder<T> : ExpressionFilterBuilder<T, DocumentPostprocessFilter>
        where T : VirtualDocument
    {
        public DocumentPostprocessFilterBuilder(DocumentManagementDbContext dbContext, DocumentPostprocessFilter filter)
            : base(filter) { }

        private void BuildFilterByCategory()
        {
            if (EntityFilter == null || EntityFilter.CategoryFilter == null)
                return;

            var categoriesIds = EntityFilter.CategoryFilter.CategoryIds;
            var targetType = typeof(T);

            if (targetType == typeof(InternalDocument))
            {
                GeneratedFilters.Add(x => categoriesIds.Contains(((InternalDocument)(object)x).InternalDocumentTypeId));                
            }
            else if (targetType == typeof(IncomingDocument))
            {
                GeneratedFilters.Add(x => categoriesIds.Contains(((IncomingDocument)(object)x).DocumentTypeId));
            }
            else if (targetType == typeof(OutgoingDocument))
            {
                GeneratedFilters.Add(x => categoriesIds.Contains(((OutgoingDocument)(object)x).DocumentTypeId));
            }
        } 
        
        private void BuildFilterByIdentificationNumbery()
        {
            if (EntityFilter == null || EntityFilter.IdentificationNumberFilter == null)
                return;

            var identificationNumber = EntityFilter.IdentificationNumberFilter.IdentificationNumber;
            var targetType = typeof(T);
            
            if (targetType == typeof(IncomingDocument))
            {
                GeneratedFilters.Add(x => identificationNumber == ((IncomingDocument)(object)x).IdentificationNumber);
            }
            else if (targetType == typeof(InternalDocument))
            {
                GeneratedFilters.Add(x => false);
            }
            else if (targetType == typeof(OutgoingDocument))
            {
                GeneratedFilters.Add(x => identificationNumber == ((OutgoingDocument)(object)x).IdentificationNumber);
            }
        }

        protected override void InternalBuild()
        {
            BuildFilterByCategory();
            BuildFilterByIdentificationNumbery();
        }
    }
}
