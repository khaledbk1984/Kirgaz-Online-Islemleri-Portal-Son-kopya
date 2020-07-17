using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kirgaz_Online_Islemleri_Portal_Son_kopya.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public bool IsSelected { get; set; }
    }
}