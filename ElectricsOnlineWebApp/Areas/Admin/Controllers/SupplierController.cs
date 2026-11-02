using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectricsOnlineWebApp.Areas.Admin.Controllers
{
    public class SupplierController : BaseController
    {
        public SupplierController()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Supplier,Supplier>();
            }); 
        }

        // GET: Admin/Supplier
        public ActionResult Index()
        {
            var suppliers = _ctx.Suppliers.ToList();
            var model = Mapper.Map<List<Supplier>, IEnumerable<Supplier>>(suppliers);
            return View(model);
        }

        private Supplier _get(int id)
        {
            var supplier = _ctx.Suppliers.FirstOrDefault(s => s.SID == id);
            return supplier;
        }

        // GET: Admin/Supplier/Details/5
        public ActionResult Details(int id)
        {
            var model = _get(id);
            return View(model);
        }

        // GET: Admin/Supplier/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Supplier/Create
        [HttpPost]
        public ActionResult Create(Supplier supplier)
        {
            try
            {
                // TODO: Add insert logic here
                _ctx.Suppliers.Add(supplier);
                _ctx.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(supplier);
            }
        }

        // GET: Admin/Supplier/Edit/5
                public ActionResult Edit(int id)
                {
                    var model = _get(id);
                    return View(model);
                }


        // POST: Admin/Supplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Supplier model)
        {
            // Show a clear message when the required postcode is missing.
            if (string.IsNullOrWhiteSpace(model.Postcode))
            {
                ModelState.AddModelError(
                    "Postcode",
                    "Please enter the supplier's actual postcode.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Find the supplier using the ID in the URL, e.g. Edit/2.
            var supplier = _ctx.Suppliers.FirstOrDefault(s => s.SID == id);

            if (supplier == null)
            {
                return HttpNotFound();
            }

            try
            {
                // Copy the submitted values into the existing record.
                supplier.SName = model.SName;
                supplier.Address1 = model.Address1;
                supplier.Address2 = model.Address2;
                supplier.Email = model.Email;
                supplier.Phone = model.Phone;
                supplier.Postcode = model.Postcode;
                supplier.State = model.State;
                supplier.Suburb = model.Suburb;

                // Save the changes to SQL Server.
                _ctx.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Display individual Entity Framework validation errors.
                foreach (var entityErrors in ex.EntityValidationErrors)
                {
                    foreach (var error in entityErrors.ValidationErrors)
                    {
                        ModelState.AddModelError(
                            error.PropertyName,
                            error.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                // Show the underlying error during local debugging.
                ModelState.AddModelError(
                    "",
                    "Supplier could not be saved: "
                    + ex.GetBaseException().Message);
            }

            return View(model);
        }

        // GET: Admin/Supplier/Delete/5
        public ActionResult Delete(int id)
        {
            var supplier = _get(id);
            _ctx.Suppliers.Remove(supplier);
            _ctx.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
