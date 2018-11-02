using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiCrud.App_Code
{
    public class ParametrosGetTranscripcionesTO
    {
        public string Login { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
    }

}