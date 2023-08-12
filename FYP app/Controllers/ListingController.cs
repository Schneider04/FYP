using FYPfinalWEBAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RP.SOI.DotNet.Utils;
using System.Data;
using System.Security.Claims;

namespace FYPfinalWEBAPP.Controllers;

public class ListingController : Controller
{
   [Authorize]
    public IActionResult Index()
    {
        
        DataTable dt = DBUtl.GetTable("SELECT * FROM ListingTable");
        _ = User.Identity!.IsAuthenticated;
        return View("Index", dt.Rows);
    }

    [Authorize]
    public IActionResult Details(int id)
    {
        string sql =
           @"SELECT h.*, u.FullName AS SubmittedBy
             FROM ListingTable h, TravelUser u
             WHERE h.UserId = u.UserId
             AND Id={0}";

        string select = string.Format(sql, id);
        List<List> lstList = DBUtl.GetList<List>(select);
        if (lstList.Count == 1)
        {
            List list = lstList[0];
            return View("Details", list);
        }
        else
        {
            TempData["Message"] = "Listing does not exist";
            TempData["MsgType"] = "warning";
            return RedirectToAction("Index");
        }
    }

    [Authorize]
    public IActionResult MyListing()
    {
        
        string userid = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        string select = string.Format(@"SELECT * FROM ListingTable 
                                        WHERE UserId = '{0}'", userid);
        List<List> list = DBUtl.GetList<List>(select);
        return View("MyListing", list);
    }

    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create(List list)
    {

        ModelState.Remove("Picture");      
        ModelState.Remove("SubmittedBy"); 
        
        if (!ModelState.IsValid)
        {
            return View("Create");
        }
        else
        {
            string userid = User.FindFirst(ClaimTypes.NameIdentifier)!.Value; ; 
            string picfilename = DoPhotoUpload(list.Photo);
            if{
                list.ExpiryDate < DateTime.Now();
            }
            string sql = @"INSERT INTO ListingTable(FoodName, Brand, ExpiryDate, Quantity, Cost, Description, Picture, UserId, HPno) VALUES ('{0}', '{1}', '{2:yyyy-MM-dd}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')";
            
            string insert = string.Format(sql, list.FoodName, list.Brand, list.ExpiryDate, list.Quantity, list.Cost, list.Description.EscQuote(), picfilename, userid, list.HPno);

            if (DBUtl.ExecSQL(insert) == 1)
            {
                TempData["Message"] = "Listing Successfully Added.";
                TempData["MsgType"] = "success";
                return RedirectToAction("MyListing");
            }
            else
            {
                ViewData["Message"] = DBUtl.DB_Message;
                ViewData["ExecSQL"] = DBUtl.DB_SQL;
                ViewData["MsgType"] = "danger";
                return View("Create");
            }
        }
    }

    [Authorize]
    public IActionResult Update(int id)
    {
        string userid = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        string sql = @"SELECT * FROM ListingTable 
                       WHERE Id={0} AND UserId='{1}'";

        string select = string.Format(sql, id, userid);
        List<List> lstList = DBUtl.GetList<List>(select);
        if (lstList.Count == 1)
        {
            List list = lstList[0];
            return View(list);
        }
        else
        {
            TempData["Message"] = "Listing does not exist";
            TempData["MsgType"] = "warning";
            return RedirectToAction("MyListing");
        }
    }

    [Authorize]
    [HttpPost]
    public IActionResult Update(List list)
    {
        ModelState.Remove("Photo");     
        ModelState.Remove("SubmittedBy"); 
        if (!ModelState.IsValid)
        {
            return View("Update", list);
        }
        else
        {
            string userid = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            string sql = @"UPDATE ListingTable  
                              SET FoodName='{2}', Brand='{3}', Description='{4}',
                                  ExpiryDate='{5:yyyy-MM-dd}', Quantity={6}, Cost={7} 
                            WHERE Id={0} AND UserId='{1}'";
            string update = string.Format(sql, list.Id, userid,
                                          list.FoodName.EscQuote(),
                                          list.Brand.EscQuote(),
                                          list.Description.EscQuote(),
                                          list.ExpiryDate, list.Quantity, list.Cost);
            if (DBUtl.ExecSQL(update) == 1)
            {
                TempData["Message"] = "Listing Updated";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Message"] = DBUtl.DB_Message;
                ViewData["ExecSQL"] = DBUtl.DB_SQL;
                TempData["MsgType"] = "danger";
            }
            return RedirectToAction("MyListing");
        }
    }

    [Authorize]
    public IActionResult Delete(int id)
    {
        string userid = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        string sql = @"SELECT * FROM ListingTable 
                       WHERE id={0} AND UserId='{1}'";
        string select = string.Format(sql, id, userid);
        DataTable ds = DBUtl.GetTable(select);
        if (ds.Rows.Count != 1)
        {
            TempData["Message"] = "Listing does not exist";
            TempData["MsgType"] = "warning";
        }
        else
        {
            string photoFile = ds.Rows[0]["picture"].ToString()!;
            string fullpath = Path.Combine(_env.WebRootPath, "images/" + photoFile);
            System.IO.File.Delete(fullpath);

            int res = DBUtl.ExecSQL(string.Format("DELETE FROM ListingTable WHERE id={0}", id));
            if (res == 1)
            {
                TempData["Message"] = "Listing Deleted";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Message"] = DBUtl.DB_Message;
                TempData["ExecSQL"] = DBUtl.DB_SQL;
                TempData["MsgType"] = "danger";
            }
        }
        return RedirectToAction("MyListing");
    }

    private string DoPhotoUpload(IFormFile photo)
    {
        string fext = Path.GetExtension(photo.FileName);
        string uname = Guid.NewGuid().ToString();
        string fname = uname + fext;
        string fullpath = Path.Combine(_env.WebRootPath, "images/" + fname);
        using (FileStream fs = new(fullpath, FileMode.Create))
        {
            photo.CopyTo(fs);
        }
        return fname;
    }

    private readonly IWebHostEnvironment _env;
    public ListingController(IWebHostEnvironment environment)
    {
        _env = environment;
    }

}
