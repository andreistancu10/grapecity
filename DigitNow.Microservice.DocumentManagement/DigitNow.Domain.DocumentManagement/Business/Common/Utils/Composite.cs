using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters.Abstractions;
using DigitNow.Domain.DocumentManagement.Data.Filters.RightsPreprocessBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Utils
{
    internal class Test<T> : List<Expression<Func<T, bool>>>
        where T : ExtendedEntity
    {
        public IList<Expression<Func<T, bool>>> Expression => this;
    }

    internal sealed class PreprocessFilterOrchestrator
    {
        private PreprocessFilterComponent _departmentFilterComponent;
        private PreprocessFilterComponent _rolesFilterComponent;

        public PreprocessFilterOrchestrator()
        {
            
        }

        public void ExtractFilters()
        {
            if(this._departmentFilterComponent.IsAplicable())
            {
                return _departmentFilterComponent.ExtractPredicates();
            }
        }
    }

    internal abstract class PreprocessFilterComponent
    {
        public abstract bool IsAplicable();
        public abstract void ExtractPredicates();
    }

    internal class PreprocessFilterComposite : PreprocessFilterComponent
    {        
        private IDataExpressionGenericFilterBuilder<Document> _departmentFilterBuilder;

        public PreprocessFilterComposite(IServiceProvider serviceProvider, DocumentDepartmentRightsFilter filter)
        {
            _departmentFilterBuilder = new DocumentDepartmentRightsPreprocessFilterBuilder(serviceProvider, filter);
        }

        public override bool IsAplicable()
        {
            
        }

        public override void ExtractPredicates()
        {
            return _departmentFilterBuilder.Build();
        }
    }

    internal class DocumentDepartmentRightsPreprocessFilterComposite : PreprocessFilterComposite
    {
        public DocumentDepartmentRightsPreprocessFilterComposite()
        {
        }
    }

    internal class DocumentUserRightsPreprocessFilterComposite : PreprocessFilterComposite
    {
        public DocumentUserRightsPreprocessFilterComposite()
        {
            var filterContext = new Do
        }
    }



    // ------------------------------------------------

    public class FilterTester
    {
        public FilterTester()
        {
            var x = new FilterXBuilder()
                .AddComponent(new DocumentDefaultFilterComponent())
                .AddComponent(new DocumentDepartmentRightsPreprocessFilterBuilder())
                .ExtractFilters();
        }
    }

    public abstract class FilterXBuilder
    {
        List<FilterComponent> _components;

        public FilterXBuilder AddComponent(FilterComponent component)
        {
            _components.Add(component);
            return this;
        }

        public IList<Expression<Func<T, bool>>> Build<T>() where T : class
        {
            List<Expression<Func<T, bool>>> x = new List<Expression<Func<T, bool>>>();

            foreach (var component in _components)
            {
                if(component.IsAplicable())
                {
                    x.AddRange(component.ExtractPredicates());
                }
            }

            return x;
        }
    }


    public abstract class FilterComponent
    {
        public abstract bool IsAplicable();
        public abstract void ExtractPredicates();
    }



}
