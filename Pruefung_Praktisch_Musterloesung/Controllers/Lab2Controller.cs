using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using Pruefung_Praktisch_Musterloesung.Models;

namespace Pruefung_Praktisch_Musterloesung.Controllers
{
    public class Lab2Controller : Controller
    {

        /**
        * 
        * ANTWORTEN BITTE HIER
        * 
        * 2.1 Die Session-ID des Users wird nicht gecheckt
        *     Der Browser, welcher der User benutzt, wird ebenfalls nicht überprüft
        * 
        * 2.2 Wenn man die Session-ID nicht bei jeder Request verändert, kann ein Hacker die Session-ID eines anderen Users klauen
        *     Wenn man den Browser nicht überprüft, kann man nichts unternehmen, wenn ein komischer Browser sich einloggt.
        * 
        * */
    public ActionResult Index()
    {

        var sessionid = Request.QueryString["sid"];

        if (string.IsNullOrEmpty(sessionid))
        {
            var hash2 = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString()));
            sessionid = string.Join("", hash2.Select(b => b.ToString("x2")).ToArray());
           }

        // Hier session ID neu generieren für folgende Requests
        var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString()));
        sessionid = string.Join("", hash.Select(b => b.ToString("x2")).ToArray());

        ViewBag.sessionid = sessionid;

        return View();
    }

       [HttpPost]
    public ActionResult Login()
    {
        var username = Request["username"];
        var password = Request["password"];
        var sessionid = Request.QueryString["sid"];

        // Browser und IP überprüfen
        var used_browser = Request.Browser.Platform;
        var ip = Request.UserHostAddress;

        if (used_browser == "Bekannter Broweser" && ip == "Häufig genutzte IP-Adresse")
        {
            Lab2Userlogin model = new Lab2Userlogin();

            if (model.checkCredentials(username, password))
            {
                model.storeSessionInfos(username, password, sessionid);

                HttpCookie c = new HttpCookie("sid");
                c.Expires = DateTime.Now.AddMonths(2);
                c.Value = sessionid;
                Response.Cookies.Add(c);

                return RedirectToAction("Backend", "Lab2");
            }
            else
            {
                ViewBag.message = "Falsche Angaben";
                return View();
            }
        }
        else
        {
            ViewBag.message = "Browser/IP-Adresse verdächtig";
            return View();
        }


    }

    public ActionResult Backend()
    {
        var sessionid = "";

        if (Request.Cookies.AllKeys.Contains("sid"))
        {
            sessionid = Request.Cookies["sid"].Value.ToString();
        }

        if (!string.IsNullOrEmpty(Request.QueryString["sid"]))
        {
            sessionid = Request.QueryString["sid"];
        }

        //var used_browser = Request.Browser.Platform;
        //var ip = Request.UserHostAddress;

        Lab2Userlogin model = new Lab2Userlogin();

        if (model.checkSessionInfos(sessionid))
        {
            return View();
        }
        else
        {
            return RedirectToAction("Index", "Lab2");
        }
    }
}

}