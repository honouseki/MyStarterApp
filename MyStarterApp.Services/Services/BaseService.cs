using MyStarterApp.Data;
using MyStarterApp.Data.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStarterApp.Services.Services
{
    public abstract class BaseService
    {
        public IDataProvider DataProvider { get; set; }

        public BaseService(IDataProvider dataProvider)
        {
            this.DataProvider = dataProvider;
        }

        public BaseService()
        {
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            this.DataProvider = new SqlDataProvider(connStr);
        }
    }
}
