using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicLayer;
using DataObjects;

namespace RubricMVC.Controllers
{
    public class RubricDetailController : Controller
    {

        IUserManager _userManager = null;
        private Rubric rubric = null;
        IRubricManager<RubricVM> _rubricManager = null;

        public RubricDetailController(IRubricManager<RubricVM> rubricManager, IUserManager userManager)
        {
            _rubricManager = rubricManager;
            _userManager = userManager;
        }

        // GET: RubricDetail
        public ActionResult RubricDetail(int rubricID)
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