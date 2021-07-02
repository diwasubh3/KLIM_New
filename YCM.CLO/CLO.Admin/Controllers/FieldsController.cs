using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YCM.CLO.DataAccess.Models;

namespace CLO.Admin.Controllers
{
    public class FieldsController : Controller
    {
        private CLOContext db = new CLOContext();

        // GET: Fields
        public ActionResult Index()
        {
            var fields = db.Fields.Include(f => f.FieldGroup);
            return View(fields.ToList());
        }

        // GET: Fields/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Field field = db.Fields.Find(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            return View(field);
        }

        // GET: Fields/Create
        public ActionResult Create()
        {
            ViewBag.FieldGroupId = new SelectList(db.FieldGroups, "FieldGroupId", "FieldGroupName");
            return View();
        }

        // POST: Fields/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FieldId,FieldGroupId,FieldName,JsonPropertyName,FieldTitle,JsonFormatString,DisplayWidth,IsPercentage,SortOrder,FieldType,HeaderCellClass,CellClass,CellTemplate,Hidden,PinnedLeft,IsSecurityOverride")] Field field)
        {
            if (ModelState.IsValid)
            {
                db.Fields.Add(field);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FieldGroupId = new SelectList(db.FieldGroups, "FieldGroupId", "FieldGroupName", field.FieldGroupId);
            return View(field);
        }

        // GET: Fields/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Field field = db.Fields.Find(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            ViewBag.FieldGroupId = new SelectList(db.FieldGroups, "FieldGroupId", "FieldGroupName", field.FieldGroupId);
            return View(field);
        }

        // POST: Fields/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FieldId,FieldGroupId,FieldName,JsonPropertyName,FieldTitle,JsonFormatString,DisplayWidth,IsPercentage,SortOrder,FieldType,HeaderCellClass,CellClass,CellTemplate,Hidden,PinnedLeft,IsSecurityOverride")] Field field)
        {
            if (ModelState.IsValid)
            {
                db.Entry(field).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FieldGroupId = new SelectList(db.FieldGroups, "FieldGroupId", "FieldGroupName", field.FieldGroupId);
            return View(field);
        }

        // GET: Fields/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Field field = db.Fields.Find(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            return View(field);
        }

        // POST: Fields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Field field = db.Fields.Find(id);
            db.Fields.Remove(field);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
