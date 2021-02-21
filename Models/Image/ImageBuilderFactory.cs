namespace Image_upload_project.Models.Image
{
    public class ImageBuilderFactory
    {
        public IImageBuilder CreateImageBuilder()
        {
            //trenutno imamo samo jednu varijantu buildera
            return new ImageSharpImageBuilder();
        }
    }
}