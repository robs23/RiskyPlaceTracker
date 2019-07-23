using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RiskyPlaceTracker.Models
{
    public class Place
    {
        [ScaffoldColumn(false)]
        public int ReportId { get; set; }
        [Display(Name="Nazwa")]
        public string Name { get; set; }
        [Display(Name="Opis")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [ScaffoldColumn(false)]
        public byte[] Photo { get; set; }
        [ScaffoldColumn(false)]
        public int AddedBy { get; set; }
        [ScaffoldColumn(false)]
        public DateTime AddedOn { get; set; }
        [Display(Name = "Dodał")]
        public string AddedByName { get; set; }
    }
}