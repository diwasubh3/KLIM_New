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
    public class FieldGroupsController : Controller
    {
        private CLOContext db = new CLOContext();

        // GET: FieldGroups
        public ActionResult Index()
        {
            return View(db.FieldGroups.ToList());
        }

        // GET: FieldGroups/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FieldGroup fieldGroup = db.FieldGroups.Find(id);
            if (fieldGroup == null)
            {
                return HttpNotFound();
            }
            return View(fieldGroup);
        }

        // GET: FieldGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FieldGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FieldGroupId,FieldGroupName,SortOrder,DisplayIcon,ShowOnPositions")] FieldGroup fieldGroup)
        {
            if (ModelState.IsValid)
            {
                db.FieldGroups.Add(fieldGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fieldGroup);
        }

        // GET: FieldGroups/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FieldGroup fieldGroup = db.FieldGroups.Find(id);
            if (fieldGroup == null)
            {
                return HttpNotFound();
            }
            return View(fieldGroup);
        }

        // POST: FieldGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FieldGroupId,FieldGroupName,SortOrder,DisplayIcon,ShowOnPositions")] FieldGroup fieldGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fieldGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fieldGroup);
        }

        // GET: FieldGroups/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FieldGroup fieldGroup = db.FieldGroups.Find(id);
            if (fieldGroup == null)
            {
                return HttpNotFound();
            }
            return View(fieldGroup);
        }

        // POST: FieldGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            FieldGroup fieldGroup = db.FieldGroups.Find(id);
            db.FieldGroups.Remove(fieldGroup);
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
