using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Web.Mvc;
using System.Linq;

namespace Pruefung_Praktisch_Musterloesung.Controllers
{
    public class Lab1Controller : Controller
    {
        /**
        * ANTWORTEN BITTE HIER
         * 
         * 1.1 In der Url wird der DBdatei-Name mitgegeben. Ein Hacker kann dies nutzen um eine falsche DB anzubinden
         *     In der Url wird der Name des Bildes mitgegeben
         * 
         * 1.2 http://Webseite.ch/Lab1/index?file="gehackte_DB"
         *     http://Webseite.ch/Lab1/index?file="DeineDaten.doc"
         * 
         * 1.3 Mit der Url könnte der Hacker eine DBdatei ansprechen. Dadurch könnte er sich Zugriff auf die Daten verschaffen, obwohl er keine Rechte dazu hat.
         *     Der Hacker könnte durch die URL eine andere Datei ansprechen.
         * */



        public ActionResult Index()
        {
            var type = Request.QueryString["type"];

            if (string.IsNullOrEmpty(type))
            {
                type = "lions";                
            }

            var path = "~/Content/images/" + type;

            List<List<string>> fileUriList = new List<List<string>>();

            if (Directory.Exists(Server.MapPath(path)))
            {
                var scheme = Request.Url.Scheme; 
                var host = Request.Url.Host; 
                var port = Request.Url.Port;
                
                string[] fileEntries = Directory.GetFiles(Server.MapPath(path));
                foreach (var filepath in fileEntries)
                {
                    var filename = Path.GetFileName(filepath);
                    var imageuri = scheme + "://" + host + ":" + port + path.Replace("~", "") + "/" + filename;

                    var urilistelement = new List<string>();
                    urilistelement.Add(filename);
                    urilistelement.Add(imageuri);
                    urilistelement.Add(type);

                    fileUriList.Add(urilistelement);
                }
            }
            
            return View(fileUriList);
        }

        public ActionResult Detail()
        {
            var file = Request.QueryString["file"];
            var type = Request.QueryString["type"];

            if (string.IsNullOrEmpty(file))
            {
                file = "Lion1.jpg";
            }
            if (string.IsNullOrEmpty(type))
            {
                file = "lions";
            }

            var relpath = "~/Content/images/" + type + "/" + file;

            List<List<string>> fileUriItem = new List<List<string>>();
            var path = Server.MapPath(relpath);

            if (System.IO.File.Exists(path))
            {
                var scheme = Request.Url.Scheme;
                var host = Request.Url.Host;
                var port = Request.Url.Port;
                var absolutepath = Request.Url.AbsolutePath;

                var filename = Path.GetFileName(file);
                var imageuri = scheme + "://" + host + ":" + port + "/Content/images/" + type + "/" + filename;

                var urilistelement = new List<string>();
                urilistelement.Add(filename);
                urilistelement.Add(imageuri);
                urilistelement.Add(type);

                fileUriItem.Add(urilistelement);
            }
            
            return View(fileUriItem);
        }
    }
}