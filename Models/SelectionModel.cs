using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Kirgaz_Online_Islemleri_Portal_Son_kopya.Models
{
    public class SelectionModel
    {
        public byte[] ImageDsiplay
        {
            get;
            set;
        }


        public MemoryStream ImageStream
        {
            get;
            set;
        }
    }
}