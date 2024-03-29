﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RiskyPlaceTracker.Models
{
    public class Report
    {
        [ScaffoldColumn(false)]
        public int ReportId { get; set; }
        [Display(Name="Nazwa")]
        [Required(ErrorMessage ="Pole nazwa nie może być puste!")]
        public string Name { get; set; }
        [Display(Name="Opis")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name="Zdjęcie")]
        public string Photo { get; set; }
        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "Wybierz użytkownika!")]
        [Display(Name="Użytkownik")]
        public int? AddedBy { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name="Data")]
        public DateTime AddedOn { get; set; }
        [Display(Name = "Dodał")]
        public string AddedByName { get; set; }
    }
}