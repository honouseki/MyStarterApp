using System.Collections.Generic;
using MyStarterApp.Models.ViewModels;
using MyStarterApp.Models.Domain;

namespace MyStarterApp.Services
{
    public interface IImageUploadService
    {
        int Insert(ImageFile model);
        List<BasicImage> SelectAll();
        BasicImage SelectById(int id);
        void Delete(BasicImage model);
    }
}