using Kendo.Mvc.Examples.Models;

namespace Kendo.Mvc.Examples.Models
{
    public abstract class BaseService
    {
        public virtual SampleEntitiesDataContext GetContext() 
        {
            return new SampleEntitiesDataContext();
        }
    }
}
