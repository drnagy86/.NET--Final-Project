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
        
        IUserManager _userManager = null;
        private List<RubricVM> rubrics = null;
        IRubricManager<RubricVM> _rubricVMManager = null;

        public RubricListController(IRubricManager<RubricVM> rubricManager, IUserManager userManager)
        {

            _rubricVMManager = rubricManager;
            _userManager = userManager;

        }

        // GET: RubricList
        public ViewResult RubricList()
        {

            try
            {                
                rubrics = _rubricVMManager.RetrieveActiveRubrics();
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
            }

            return View(rubrics);
        }


    }
}