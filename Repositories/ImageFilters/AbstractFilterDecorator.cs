using System.Collections.Generic;

namespace Image_upload_project.Repositories.ImageFilters
{
    public abstract class AbstractFilterDecorator : IImageFilter
    {
        private readonly IImageFilter _baseFilter;

        protected AbstractFilterDecorator(IImageFilter baseFilter = null)
        {
            _baseFilter = baseFilter;
        }
        
        
        public virtual string FilterQuery(string query)
        {
            if (_baseFilter == null)
            {
                query += " WHERE";
                return query;
            }

            return " AND"+ _baseFilter.FilterQuery(query);
        }

        public virtual void AddParameters(Dictionary<string, object> parameters)
        {
            _baseFilter?.AddParameters(parameters);
        }
    }
}