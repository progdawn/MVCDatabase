using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MVCDatabase.Models;

namespace MVCDatabase.Controllers
{
    public class EmployeeController : Controller
    {
        SqlConnection dbcon = new SqlConnection(ConfigurationManager.ConnectionStrings["demodb"].ConnectionString.ToString());

        // GET: Employee
        public ActionResult Index()
        {
            List<Employee> emplist;
            try
            {
                dbcon.Open();
                emplist = Employee.GetEmployeeList(dbcon, "");
                dbcon.Close();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return View(emplist);
        }

        public ActionResult Create()
        {
            Employee emp = new Employee();
            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee emp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dbcon.Open();
                    int intresult = Employee.CUDEmployee(dbcon, "create", emp);
                    dbcon.Close();
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.errormsg = "Invalid data in Create Post action method";
            return View("Error");
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue && (id.Value > 0 && id.Value <= 99))
            {
                try
                {
                    dbcon.Open();
                    Employee emp = Employee.GetEmployeeSingle(dbcon, id.Value);
                    dbcon.Close();
                    if (emp.EmpId > 0) return View(emp);
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.errormsg = "Invalid data in the Edit Page";
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee emp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dbcon.Open();
                    int intresult = Employee.CUDEmployee(dbcon, "update", emp);
                    dbcon.Close();
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.errormsg = "Invalid data in Edit Post action method";
            return View("Error");
        }

        public ActionResult Delete(int? id)
        {
            if (id.HasValue && (id.Value > 0 && id.Value <= 99))
            {
                try
                {
                    dbcon.Open();
                    Employee emp = Employee.GetEmployeeSingle(dbcon, id.Value);
                    dbcon.Close();
                    if (emp.EmpId > 0) return View(emp);
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.errormsg = "Invalid data in the Delete Page";
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection fc)
        {
            Employee emp = new Models.Employee();
            emp.EmpId = id;
            try
            {
                dbcon.Open();
                int intresult = Employee.CUDEmployee(dbcon, "delete", emp);
                dbcon.Close();
                return RedirectToAction("Index");
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}