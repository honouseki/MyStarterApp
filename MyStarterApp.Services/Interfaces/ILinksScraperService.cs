using MyStarterApp.Models.Domain;
using MyStarterApp.Models.ViewModels;

namespace MyStarterApp.Services.Services
{
    public interface ILinksScraperService
    {
        LinkDeviantArt GetDeviantArt(Url url);
    }
}