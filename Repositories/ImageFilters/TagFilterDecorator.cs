using System;
using System.Collections.Generic;

namespace Image_upload_project.Repositories.ImageFilters
{
    public class TagFilterDecorator : AbstractFilterDecorator
    {
        private readonly string[] _tags;
        
        public TagFilterDecorator(IImageFilter baseFilter,  string tags) : base(baseFilter)
        {
            _tags = tags.Split('#', StringSplitOptions.RemoveEmptyEntries);
        }

        public override string FilterQuery(string query)
        {
            query = base.FilterQuery(query);
            query += " Tags LIKE @tagFilter0";
            for (int i = 1; i < _tags.Length; i++)
            {
                query += $" AND Tags LIKE @tagFilter{i}";
            }

            return query;
        }

        public override void AddParameters(Dictionary<string, object> parameters)
        {
            base.AddParameters(parameters);
            for (int i = 0; i < _tags.Length; i++)
            {
                parameters[$"tagFilter{i}"] = $"%#{_tags[i]}#%";
            }
        }
    }
}