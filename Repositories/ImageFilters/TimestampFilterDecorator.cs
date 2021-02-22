using System;
using System.Collections.Generic;

namespace Image_upload_project.Repositories.ImageFilters
{
    public class TimestampFilterDecorator : AbstractFilterDecorator
    {
        private readonly DateTime? _start;
        private readonly DateTime? _end;

        public TimestampFilterDecorator(IImageFilter baseFilter, DateTime? start, DateTime? end)
        :base(baseFilter)
        {
            if (start == null && end == null)
                throw new ArgumentException("At least one value must be set.");
            
            _start = start;
            _end = end;
        }

        public override string FilterQuery(string query)
        {
            query = base.FilterQuery(query);
            if (_start != null)
            {
                query += " Timestamp >= @start";
            }

            if (_end != null)
            {
                if (_start != null)
                    query += " AND";
                query += " Timestamp <= @end";
            }

            return query;
        }

        public override void AddParameters(Dictionary<string, object> parameters)
        {
            base.AddParameters(parameters);
            if (_start != null)
            {
                parameters["start"] = _start.Value;
            }

            if (_end != null)
            {
                parameters["end"] = _end.Value;
            }
        }
    }
}