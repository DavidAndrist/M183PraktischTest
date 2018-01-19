using System;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using Pruefung_Praktisch_Musterloesung.Models;

namespace Pruefung_Praktisch_Musterloesung.Controllers
{
    public class Lab3Controller : Controller
    {

        /**
        * 
        ANTWORTEN BITTE HIER
        * 
        * 3.1 Man kann im Kommentar Javascript mitgeben
        * 
        * 3.2 Der Hacker kann im Kommentar JavaScript mitgeben welches beim nächsten Refresh von der Seite ausgeführt wird. 
        * 
        * 3.2 http://Webseite.ch/Lab3/comment?comment="\"window.alert("Sie wurden gehackt");\""
        * */

        public ActionResult Backend()
        {
            return View();
        }

        [ValidateInput(false)] // -> Hier erlaubt man das mitgeben von HTML Tags
        [HttpPost]
        public ActionResult Comment()
        {
            var comment = Request["comment"];
            var postid = Int32.Parse(Request["postid"]);

            // Gefährliche Chars entfernen
            comment = comment.Replace("'", String.Empty);
            comment = comment.Replace("\"", String.Empty);

            Lab3Postcomments model = new Lab3Postcomments();

            if (model.storeComment(postid, comment))
            {
                return RedirectToAction("Index", "Lab3");
            }
            else
            {
                ViewBag.message = "Speichern des Kommentars fehlgeschlagen";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login()
        {
            var username = Request["username"];
            var password = Request["password"];

            Lab3User model = new Lab3User();

            if (model.checkCredentials(username, password))
            {
                return RedirectToAction("Backend", "Lab3");
            }
            else
            {
                ViewBag.message = "Falsche Angaben";
                return View();
            }
        }
    }
}