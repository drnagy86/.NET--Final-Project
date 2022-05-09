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
        private RubricModelView rubricModelView = null;


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
                rubrics = new List<RubricVM>();
            }

            return View(rubrics);
        }

        // GET: Rubric/Details/5
        public ActionResult Details(int rubricID)
        {
            RubricModelView rubricModel = null;
            try
            {
                rubric = _rubricVMManager.RetrieveRubricByRubricID(rubricID);

                rubricModel = new RubricModelView(rubric, _scoreTypeManager.RetrieveScoreTypes());

                User currentUser = getCurrentUserAsObject();

                if (currentUser != null && currentUser.UserID.Equals(rubric.RubricCreator.UserID))
                {
                    rubricModel.CanEdit = true;
                }

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("RubricList");
            }

            return View(rubricModel);
        }

        // GET: Rubric/Create
        [Authorize]
        public ActionResult Create()
        {
            return RedirectToAction("Edit", new { rubricID = 0 });
        }


        // GET: Rubric/Edit/5
        [Authorize(Roles = "Administrator, Creator")]
        public ActionResult Edit(int rubricID)
        {

            if (rubricID != 0)
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
            else
            {
                // actually creating
                try
                {
                    
                    

                    RubricVM temp = new RubricVM(getCurrentUserAsObject());
                    List<ScoreType> scoreTypes = _scoreTypeManager.RetrieveScoreTypes();

                    rubricModelView = new RubricModelView(temp, scoreTypes);

                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = ex.Message;
                }

                return View(rubricModelView);
            }
        }

        // POST: Rubric/Edit/5
        [Authorize(Roles = "Administrator, Creator")]
        [HttpPost]
        public ActionResult Edit(int rubricID, RubricModelView newRubric)
        {
            bool result = false;
            RubricVM oldRubric = null;

            if (rubricID != 0)
            {
                try
                {
                    oldRubric = _rubricVMManager.RetrieveRubricByRubricID(rubricID);
                }
                catch (Exception ex)
                {

                    TempData["errorMessage"] = ex.Message + "<br>";
                    RedirectToAction("RubricList");
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
                        RedirectToAction("RubricList");
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
                // create
                try
                {
                    // given the number of criteria, create at least one facet with that many criteria
                    newRubric.AddBlankFacetWithCritria();

                    newRubric.RubricCreator = getCurrentUserAsObject();
                    
                    int newID = _rubricVMManager.CreateRubric(newRubric);

                    return RedirectToAction("Edit", "Facet", new { rubricID = newID, facetID = newRubric.FacetVMs[0].FacetID });
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = "There is a problem with the information being added.<br> " + ex.Message;
                    return View(newRubric);
                }
            }
        }

        // GET: Rubric/Delete/5
        [Authorize(Roles = "Administrator, Creator")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // GET: Rubric/Deactivate/5
        [Authorize(Roles = "Administrator, Creator")]
        public ActionResult Deactivate(int rubricID)
        {
            try
            {
                _rubricVMManager.DeactivateRubricByRubricID(rubricID);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return RedirectToAction("RubricList", "Rubric");
        }


        private string getCurrentUserID()
        {

            return User.Identity.GetUserId();
        }

        private bool isAdmin()
        {
            return User.IsInRole("Administrator");
        }

        private DataObjects.User getCurrentUserAsObject()
        {
            string userID = User.Identity.GetUserName();
            DataObjects.User currentUser = null;

            if (userID == "" || userID == null)
            {
                currentUser = new User();
            }
            else
            {
                currentUser = _userManager.GetUserByUserID(userID);
            }

            return currentUser;
        }
    }
}
