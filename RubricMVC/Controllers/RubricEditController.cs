using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicLayer;
using DataObjects;

namespace RubricMVC.Controllers
{
    public class RubricEditController : Controller
    {
        IUserManager _userManager = null;
        private Rubric rubric = null;
        IRubricManager<Rubric> _rubricManager = null;

        public RubricEditController(IRubricManager<Rubric> rubricManager, IUserManager userManager)
        {
            _rubricManager = rubricManager;
            _userManager = userManager;
        }

        // GET: RubricEdit
        [HttpGet]
        public ActionResult RubricEdit(int rubricID)
        {
            try
            {
                rubric = _rubricManager.RetrieveRubricByRubricID(rubricID);
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
            }

            return View(rubric);
        }
    }
}