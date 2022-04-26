using DataObjects;
using LogicLayer;
using RubricMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace RubricMVC.Controllers
{
    public class RubricController : Controller
    {
        IRubricManager<RubricVM> _rubricVMManager = null;
        IUserManager _userManager = null;
        IScoreTypeManager _scoreTypeManager = null;

        private List<RubricVM> rubrics = null;
        private RubricVM rubric = null;
        

        public RubricController(IRubricManager<RubricVM> rubricManager, IUserManager userManager, IScoreTypeManager scoreTypeManager)
        {

            _rubricVMManager = rubricManager;
            _userManager = userManager;
            _scoreTypeManager = scoreTypeManager;

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
        [Authorize(Roles = "Administrator, Creator")]
        public ActionResult Edit(int rubricID)
        {            
            RubricModelView rubricModel = null;

            try
            {
                rubric = _rubricVMManager.RetrieveRubricByRubricID(rubricID);
                List<ScoreType> scoreTypes = _scoreTypeManager.RetrieveScoreTypes();

                rubricModel = new RubricModelView(rubric, scoreTypes);

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            if (getCurrentUserID() == rubric.RubricCreator.UserID ||
                User.IsInRole("Administrator"))
            {
                return View(rubricModel);
            }
            else
            {
                TempData["errorMessage"] = "Sorry, you are not authorized to edit this rubric";
                return RedirectToAction("Details", new { rubricID = rubricID });
            }
            
        }

        // POST: Rubric/Edit/5
        [Authorize(Roles = "Administrator, Creator")]
        [HttpPost]
        public ActionResult Edit(int rubricID, RubricModelView newRubric)
        {
            bool result = false;
            RubricVM oldRubric = null;
            

            if (ModelState.IsValid)
            {
                try
                {
                    oldRubric = _rubricVMManager.RetrieveRubricByRubricID(rubricID);
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = ex.Message + "<br>";
                }

                if (getCurrentUserID() == oldRubric.RubricCreator.UserID ||
                    User.IsInRole("Administrator"))
                {
                    try
                    {
                        result = _rubricVMManager.UpdateRubricByRubricID
                            (
                            rubricID,
                            oldRubric.Name,
                            newRubric.Name,
                            oldRubric.Description,
                            newRubric.Description,
                            oldRubric.ScoreTypeID,
                            newRubric.ScoreTypeID
                            );
                    }
                    catch (Exception ex)
                    {
                        TempData["errorMessage"] = ex.Message + "<br>";

                    }
                    if (!result)
                    {
                        TempData["errorMessage"] = "Update was not successful. Please try again.<br>";

                        return View(new RubricModelView(oldRubric, newRubric.ScoreTypes));
                    }
                    else
                    {
                        return RedirectToAction("Details", new { rubricID = rubricID });
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Sorry, you are not authorized to edit this rubric.<br>";
                    return RedirectToAction("Details", new { rubricID = rubricID });
                }

            }
            else
            {
                TempData["errorMessage"] = "There is a problem with the information being added.<br>";

                return View(new RubricModelView(oldRubric, newRubric.ScoreTypes));
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


        private string getCurrentUserID()
        {
            return User.Identity.GetUserId();
        }

        private bool isAdmin()
        {
            return User.IsInRole("Administrator");
        }
    }
}
