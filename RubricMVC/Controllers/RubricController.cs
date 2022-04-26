using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RubricMVC.Controllers
{
    public class RubricController : Controller
    {
        IRubricManager<RubricVM> _rubricVMManager = null;
        IUserManager _userManager = null;

        private List<RubricVM> rubrics = null;
        private Rubric rubric = null;
        

        public RubricController(IRubricManager<RubricVM> rubricManager, IUserManager userManager)
        {

            _rubricVMManager = rubricManager;
            _userManager = userManager;

        }

        // GET: Rubric
        public ActionResult RubricList()
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

        // GET: Rubric/Details/5
        public ActionResult Details(int rubricID)
        {
            try
            {
                rubric = _rubricVMManager.RetrieveRubricByRubricID(rubricID);
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
            }

            return View(rubric);
        }

        // GET: Rubric/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rubric/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Rubric/Edit/5
        public ActionResult Edit(int rubricID)
        {
            try
            {
                rubric = _rubricVMManager.RetrieveRubricByRubricID(rubricID);
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
            }

            return View(rubric);
        }

        // POST: Rubric/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Rubric/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Rubric/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
