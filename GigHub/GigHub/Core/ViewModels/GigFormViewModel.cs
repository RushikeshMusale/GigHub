using GigHub.Controllers;
using GigHub.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace GigHub.Core.ViewModels
{
    public class GigFormViewModel
    {
        [Required]
        public string Venue { get; set; }

        public int  id { get; set; }

        [Required]
        [FutureDate]
        public string Date { get; set; }

        [Required]
        [ValidTime]
        public string Time { get; set; }

        [Required]
        public byte Genre { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        public string Heading { get; set; }

        public string Action {
            get {
                //old problamatic way
                //return id == 0 ? "Create" : "Update";
                Expression<Func<GigsController, ActionResult>> update = (c => c.Modify(this));
                Expression<Func<GigsController, ActionResult>> create = (c => c.Create(this));

                var action = id == 0 ? create : update;
                return (action.Body as MethodCallExpression).Method.Name;
            }
        }

        public DateTime GetDatetime()
        {
            return DateTime.Parse(string.Format("{0} {1}", Date, Time));
        }
    }
}