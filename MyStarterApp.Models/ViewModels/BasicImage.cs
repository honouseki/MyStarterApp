using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStarterApp.Models.ViewModels
{
    public class BasicImage
    {
        // Basic Image will only grab from the one main image file table
        // Food for thought: 
        //     If the image is connected to another table (image-descriptive tables, i.e. Gallery Images, Blog Images),
        //     then we would create another view model class, as well as change our Select services to accommodate the 
        //     additional information retrieved
        public int Id { get; set; }
        public string ImageFileName { get; set; }
        public string SystemFileName { get; set; }
        public int ImageFileType { get; set; }
        public string Location { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
