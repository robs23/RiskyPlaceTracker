using RiskyPlaceTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiskyPlaceTracker.ViewModels
{
    public class AddReportViewModel
    {
        public List<User> Users { get; set; }
        public RD_Reports ThisReport { get; set; }
        public int? SelectedUser { get; set; }
    }

}