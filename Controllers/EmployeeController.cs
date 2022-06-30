using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRUDAPPLIWithPhoto.Models;

namespace CRUDAPPLIWithPhoto.Controllers
{
    public class EmployeeController : Controller
    {
        private ExampleDbEntities db = new ExampleDbEntities();

        public ExampleDbEntities ExampleDbEntities { get; private set; }

        public EmployeeController()
        {
            ExampleDbEntities = new ExampleDbEntities();
        }

        // GET: Employee
        public ActionResult Index()
        {
            return View(db.Employee12.ToList());
        }

        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee12 employee12 = db.Employee12.Find(id);
            if (employee12 == null)
            {
                return HttpNotFound();
            }
            return View(employee12);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {


            List<Employee12> emplist = db.Employee12.ToList();
            ViewBag.EmployeeTbl = new SelectList(emplist,"City","City");
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee12 e)
        {
            if(ModelState.IsValid==true)
            {
                string fileName = Path.GetFileNameWithoutExtension(e.ImageFile.FileName);
                string extension = Path.GetExtension(e.ImageFile.FileName);
                HttpPostedFileBase postedFile = e.ImageFile;
                int length = postedFile.ContentLength;

                if(extension.ToLower() ==".jpg" ||extension.ToLower() ==".jpeg" || extension.ToLower() ==".png")
                {
                    if(length <= 1000000)
                    {
                        fileName = fileName + extension;
                        e.Photo = "~/images/"+fileName;
                        fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                        e.ImageFile.SaveAs(fileName);
                        db.Employee12.Add(e);
                        int a =db.SaveChanges();
                        if (a>0)
                        {
                            TempData["CreateMessage"] = "<script>alert('Data  Inserted successfull ')</script";
                            ModelState.Clear();
                            return RedirectToAction("Index", "Employee");
                        }
                        else
                        {
                            TempData["UpdateMessage"] = "<script>alert('Data not inserted')</script";
                        }

                    }
                    else
                    {
                        TempData["SizeMessage"] = "<script>alert('Image Size should be less than 1 MB')</script";
                    }
                }
                else
                {
                    TempData["ExtensionMessage"] = "<script>alert('Formet Not Support')</script";
                }
            }
            

            return View();
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
           
            var EmployeeRow = db.Employee12.Where(model => model.Id == id).FirstOrDefault();
            Session["Image"] = EmployeeRow.Photo;
            return View(EmployeeRow);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee12 e)
        {

            if(ModelState.IsValid==true)
            {
                if(e.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(e.ImageFile.FileName);
                    string extension = Path.GetExtension(e.ImageFile.FileName);
                    HttpPostedFileBase postedFile = e.ImageFile;
                    int length = postedFile.ContentLength;

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (length <= 1000000)
                        {
                            fileName = fileName + extension;
                            e.Photo = "~/images/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                            e.ImageFile.SaveAs(fileName);
                            db.Entry(e).State=EntityState.Modified;
                            int a = db.SaveChanges();
                            if (a > 0)
                            {
                                TempData["UpdateMessage"] = "<script>alert('Data update successfull ')</script";
                                ModelState.Clear();
                                return RedirectToAction("Index", "Employee");
                            }

                            else
                            {
                                TempData["UpdateMessage"] = "<script>alert('Data not update')</script";
                            }

                        }
                        else
                        {
                            TempData["SizeMessage"] = "<script>alert('Image Size should be less than 1 MB')</script";
                        }
                    }
                    else
                    {
                        TempData["ExtensionMessage"] = "<script>alert('Formet Not Support')</script";
                    }

                }
                else
                {
                    e.Photo = Session["Image"].ToString();
                    db.Entry(e).State = EntityState.Modified;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["UpdateMessage"] = "<script>alert('Data update successfull ')</script";
                        ModelState.Clear();
                        return RedirectToAction("Index", "Employee");
                    }

                    else
                    {
                        TempData["UpdateMessage"] = "<script>alert('Data not update')</script";
                    }
                }
            }
            return View();
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
       
        
        
        

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee12 employee12 = db.Employee12.Find(id);
            if (employee12 == null)
            {
                return HttpNotFound();
            }
            return View(employee12);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee12 employee12 = db.Employee12.Find(id);
            db.Employee12.Remove(employee12);
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
