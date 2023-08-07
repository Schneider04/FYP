using FYPfinalWEBAPP.Models;
using Microsoft.AspNetCore.Mvc;
using RP.SOI.DotNet.Utils;

namespace FYPfinalWEBAPP.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult CreateUser(User usr)
        {
            
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid input";
                ViewData["MsgType"] = "warning";
                return View("CreateUser");
            }
            else
            {
                string sql = @"INSERT INTO TravelUser(UserId, UserPw, FullName)
                       VALUES('{0}', HASHBYTES('SHA1','{1}'), '{2}')"
                ;
                if (DBUtl.ExecSQL(sql, usr.UserId, usr.UserPw, usr.FullName) == 1)
                {
                    return View("~/Views/Account/Login.cshtml");
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["ExecSQL"] = DBUtl.DB_SQL;
                    ViewData["MsgType"] = "danger";
                    return View("CreateUser");
                }
            }
        }
    }
}
