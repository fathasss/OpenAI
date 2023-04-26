using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HasGenerator.API_v2.Models
{
    public class APIReferances
    {
        public string API_KEY { get; set; }
        public APIReferances()
        {
            API_KEY = "YOUR_API_KEY";
        }
    }
}
