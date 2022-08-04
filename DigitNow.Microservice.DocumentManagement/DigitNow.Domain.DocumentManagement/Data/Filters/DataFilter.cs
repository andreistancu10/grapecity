using System.Reflection;

namespace DigitNow.Domain.DocumentManagement.Data.Filters
{
    public abstract class DataFilter
    {
        public virtual bool IsEmpty()
        {
            foreach (var property in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if(property.GetValue(this) != null)
                    return false;
            }
            return true;
        }
    }
}
