using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataObjects;
using LogicLayer;
using RubricMVC.Models;

namespace RubricMVC.Controllers
{
    public class FacetController : Controller
    {
        IFacetManager _facetManager = null;
        IUserManager _userManager = null;
        IFacetTypeManager _facetTypeManager = null;
        ICriteriaManager _criteriaManager = null;
        IRubricManager<Rubric> _rubricManager = null;

        private Facet facet = null;
        private FacetModelView facetMV = null;

        public FacetController(IFacetManager facetManager, IUserManager userManager, IFacetTypeManager facetTypeManager, ICriteriaManager criteriaManager, IRubricManager<Rubric> rubricManager)
        {
            _facetManager = facetManager;
            _userManager = userManager;
            _facetTypeManager = facetTypeManager;
            _criteriaManager = criteriaManager;
            _rubricManager = rubricManager;
        }
        

        // GET: Facet/Edit/5
        public ActionResult Edit(int rubricID, string facetID)
        {
            facetMV = new FacetModelView();

            try
            {
                facetMV = new FacetModelView(_facetManager.RetrieveFacetVM(rubricID, facetID));
                facetMV.FacetTypeList = _facetTypeManager.RetrieveFacetTypes();
                facetMV.Rubric = _rubricManager.RetrieveRubricByRubricID(facetMV.RubricID);
                
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }


            return View(facetMV);
        }

        // POST: Facet/Edit/5
        [HttpPost]
        public ActionResult Edit(FacetModelView newFacet)
        {
            try
            {
                FacetVM oldFacet = _facetManager.RetrieveFacetVM(newFacet.RubricID, newFacet.OldFacetID);

                _facetManager.UpdateFacetAndCriteria(oldFacet, newFacet);


                return RedirectToAction("Details", "Rubric", new { rubricID = newFacet.RubricID });
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(newFacet);
            }

        }

        // GET: Facet/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Facet/Delete/5
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
