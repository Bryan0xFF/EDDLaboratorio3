using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LAB3_1252016_1053016.Models;
using System.IO;
using Newtonsoft.Json;
using AVL; 

namespace LAB3_1252016_1053016.Controllers
{
    public class PartidoController : Controller
    {
        AVLTree <Partido> AVLPartido = new AVLTree<Partido>(); 

        // GET: Partido
        public ActionResult Index()
        {
            Session["AVLPartido"] = Session["AVLPartido"] ?? AVLPartido; 
            return View();
        }

        [HttpGet]
        public ActionResult LecturaArchivo()
        {
            //aqui se abre una vista para poder subir el archivo
            return View();
        }

        private bool IsValidContentType(HttpPostedFileBase contentType)
        {
            return contentType.FileName.EndsWith(".json");
        }

        [HttpPost]
        public ActionResult LecturaArchivo(HttpPostedFileBase File)
        {
            if (File == null || File.ContentLength == 0)
            {
                ViewBag.Error = "El archivo seleccionado está vacío o no hay archivo seleccionado";
                return View("Index");
            }
            else
            {
                if (!IsValidContentType(File))
                {
                    ViewBag.Error = "Solo archivos Json son válidos para la entrada";
                    return View("Index");
                }

                if (File.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(File.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/JsonFiles/" + fileName));
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    File.SaveAs(path);
                    using (StreamReader reader = new StreamReader(path))
                    {
                        if (reader != null)
                        {
                            AVLPartido = (AVLTree<Partido>)Session["AVLPartido"];
                            string info = reader.ReadToEnd();
                            List<Partido> lista = JsonConvert.DeserializeObject<List<Partido>>(info);
                            for (int i = 0; i < lista.Count; i++)
                            {
                                AVLPartido.Insert(lista.ElementAt(i));
                            }
                            Session["AVLPartido"] = AVLPartido;
                        }
                    }
                }
            }
            return View("Index");
        }


        
              
        
    }
}