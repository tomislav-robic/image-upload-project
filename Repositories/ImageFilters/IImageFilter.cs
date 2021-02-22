using System.Collections.Generic;

namespace Image_upload_project.Repositories.ImageFilters
{
    public interface IImageFilter
    {
        string FilterQuery(string query);

        void AddParameters(Dictionary<string, object> parameters);
    }
}