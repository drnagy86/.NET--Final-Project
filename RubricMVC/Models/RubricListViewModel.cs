using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataObjects;
namespace RubricMVC.Models
{
    public class RubricListViewModel
    {
        public IEnumerable<RubricVM> Rubrics { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}