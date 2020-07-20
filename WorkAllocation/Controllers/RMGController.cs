using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkAllocation.Services;
using WorkAllocation.Models;

namespace WorkAllocation.Controllers
{
    public class RMGController : Controller
    {
        // GET: RMG
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Project()
        {
            return View();
        }
        public ActionResult GetDomain()
        {
            List<VM_Domain> result = DDL.DomainList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InsUpdProject(VM_Porject Request_Data)
        {
            bool result = new ProjectService().INS_UPD_PROJECT(Request_Data);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult get_project_List(VM_Porject Request_Data)
        {
            List<VM_Porject> result = new ProjectService().Project_Gridlist(Request_Data);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteProject(String PROJECT_CODE)
        {
            bool result = new ProjectService().DELETE_PROJECT(PROJECT_CODE);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetActivities()
        {
            return View();
        }
        public ActionResult GetProjectName()
        {
            List<VM_Porject> result = DDL.ProjectName();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult (FormCollection formCollection)
        {
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var user = new User();
                            user.SNo = Convert.ToInt32(workSheet.Cells[rowIterator, 1].Value);
                            user.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                            user.Age = Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value);
                            usersList.Add(user);
                        }
                    }
                }
            }
        }
    }
}