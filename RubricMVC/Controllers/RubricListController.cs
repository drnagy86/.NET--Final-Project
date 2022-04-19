using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataObjects;
using LogicLayer;

namespace RubricMVC.Controllers
{
    public class RubricListController : Controller
    {
        //IRubricManager<RubricVM> _rubricManager = null;
        IUserManager _userManager = null;
        private List<Rubric> rubrics = null;
        IRubricManager<Rubric> _rubricManager = null;

        public RubricListController(IRubricManager<Rubric> rubricManager, IUserManager userManager)
        {

            _rubricManager = rubricManager;
            _userManager = userManager;

        }

        // GET: RubricList
        public ViewResult RubricList()
        {

            try
            {
                rubrics = _rubricManager.RetrieveActiveRubrics();
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
            }

            return View(rubrics);
        }

    }
}