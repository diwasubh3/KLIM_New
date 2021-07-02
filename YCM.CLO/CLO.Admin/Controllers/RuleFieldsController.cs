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
    public class RuleFieldsController : Controller
    {
        private CLOContext db = new CLOContext();

        // GET: RuleFields
        public ActionResult Index()
        {
            var ruleFields = db.RuleFields.Include(r => r.Field).Include(r => r.Rule).Include(r => r.RuleSectionType);
            return View(ruleFields.ToList());
        }

        // GET: RuleFields/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RuleField ruleField = db.RuleFields.Find(id);
            if (ruleField == null)
            {
                return HttpNotFound();
            }
            return View(ruleField);
        }

        // GET: RuleFields/Create
        public ActionResult Create()
        {
            ViewBag.FieldId = new SelectList(db.Fields, "FieldId", "FieldName");
            ViewBag.RuleId = new SelectList(db.Rules, "RuleId", "RuleName");
            ViewBag.RuleSectionTypeId = new SelectList(db.RuleSectionTypes, "RuleSectionTypeId", "RuleSectionName");
            return View();
        }

        // POST: RuleFields/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RuleFieldId,RuleId,RuleSectionTypeId,FieldId,SortOrder")] RuleField ruleField)
        {
            if (ModelState.IsValid)
            {
                db.RuleFields.Add(ruleField);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FieldId = new SelectList(db.Fields, "FieldId", "FieldName", ruleField.FieldId);
            ViewBag.RuleId = new SelectList(db.Rules, "RuleId", "RuleName", ruleField.RuleId);
            ViewBag.RuleSectionTypeId = new SelectList(db.RuleSectionTypes, "RuleSectionTypeId", "RuleSectionName", ruleField.RuleSectionTypeId);
            return View(ruleField);
        }

        // GET: RuleFields/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RuleField ruleField = db.RuleFields.Find(id);
            if (ruleField == null)
            {
                return HttpNotFound();
            }
            ViewBag.FieldId = new SelectList(db.Fields, "FieldId", "FieldName", ruleField.FieldId);
            ViewBag.RuleId = new SelectList(db.Rules, "RuleId", "RuleName", ruleField.RuleId);
            ViewBag.RuleSectionTypeId = new SelectList(db.RuleSectionTypes, "RuleSectionTypeId", "RuleSectionName", ruleField.RuleSectionTypeId);
            return View(ruleField);
        }

        // POST: RuleFields/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RuleFieldId,RuleId,RuleSectionTypeId,FieldId,SortOrder")] RuleField ruleField)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ruleField).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FieldId = new SelectList(db.Fields, "FieldId", "FieldName", ruleField.FieldId);
            ViewBag.RuleId = new SelectList(db.Rules, "RuleId", "RuleName", ruleField.RuleId);
            ViewBag.RuleSectionTypeId = new SelectList(db.RuleSectionTypes, "RuleSectionTypeId", "RuleSectionName", ruleField.RuleSectionTypeId);
            return View(ruleField);
        }

        // GET: RuleFields/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RuleField ruleField = db.RuleFields.Find(id);
            if (ruleField == null)
            {
                return HttpNotFound();
            }
            return View(ruleField);
        }

        // POST: RuleFields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RuleField ruleField = db.RuleFields.Find(id);
            db.RuleFields.Remove(ruleField);
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
