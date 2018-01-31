using HtmlAgilityPack;
using MyStarterApp.Models.Domain;
using MyStarterApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStarterApp.Services.Services
{
    public class LinksScraperService : BaseService, ILinksScraperService
    {
        public LinkDeviantArt GetDeviantArt(Url url)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            // Loads in html document from URL input
            LinkDeviantArt model = new LinkDeviantArt();
            model.Url = url.UrlString;
            var htmlWeb = new HtmlWeb();
            HtmlDocument document = null;
            document = htmlWeb.Load(url.UrlString);

            // Grabs info for UserIconUrl from document
            var anchortag1 = document.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("authorative-avatar")).ToList();
            model.UserIconUrl = anchortag1[0].Descendants("img").FirstOrDefault().GetAttributeValue("src", "");

            // Grabs Username / TagLine from document
            var anchortag2 = document.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("gruserbadge")).ToList();
            model.Username = anchortag2[0].Descendants("a").FirstOrDefault().InnerText;
            model.TagLine = anchortag2[0].LastChild.InnerText;

            return model;
        }
    }
}
