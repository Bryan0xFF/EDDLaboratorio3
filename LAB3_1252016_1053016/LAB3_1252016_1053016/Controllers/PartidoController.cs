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
        List<string> logs = new List<string>();
        List<string> logsRotaciones = new List<string>();

        // GET: Partido
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Menu(FormCollection form)
        {            
            Session["AVLPartido"] = Session["AVLPartido"] ?? AVLPartido;
            Session["Logs"] = Session["Logs"] ?? logs;
            return View("Menu");
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
            logs = (List<string>)Session["Logs"];

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
                                string partido = lista.ElementAt(i).Pais1 + " vs " + lista.ElementAt(i).Pais2;
                                logsRotaciones = AVLPartido.rotaciones(); 
                                logs.Add("Se ha insertado un nuevo partido " + partido); 
                            }
                            Session["AVLPartido"] = AVLPartido;                            
                            PrintLogs(logsRotaciones);
                            PrintLogs(logs);
                            logsRotaciones.Clear();
                        }
                    }
                }
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult InsertarManual()
        {
            return View(); 
        }

        [HttpPost]
        public ActionResult InsertarManual(Partido partido)
        {
            logs = (List<string>)Session["Logs"];
            AVLPartido = (AVLTree<Partido>)Session["AVLPartido"];
            AVLPartido.Insert(partido);
            logsRotaciones = AVLPartido.rotaciones(); 
            string Partido = partido.Pais1 + " vs " + partido.Pais2;
            logs.Add("Se ha insertado un nuevo partido " + Partido); 
            Session["AVLPartido"] = AVLPartido;
            PrintLogs(logsRotaciones);
            PrintLogs(logs);
            logsRotaciones.Clear();
            Session["Logs"] = logs;
            Session["AVLPartido"] = AVLPartido;
            return View("InsertarManualView"); 
        }

        private void PrintLogs(List<string> logs)
        {
            //Se debe redefinir ruta si se está utilizando en otro ordenador
            StreamWriter writer = new StreamWriter(@"D:\Alex Rodríguez\Desktop\EDDLaboratorio3\logs.txt", false);

            for (int i = 0; i < logs.Count; i++)
            {
                writer.WriteLine(logs.ElementAt(i));
            }
            writer.Close();
        }

        public void DownloadLogs()
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("Logs de: " + DateTime.Today.ToShortDateString() );
            Response.ClearContent();
            Response.AddHeader("Content-disposition", "Attachment;filename=Logs.txt");
            Response.ContentType = "text,out";

            for (int i = 0; i < logs.Count; i++)
            {
                sw.WriteLine(logs.ElementAt(i));
            }
            sw.Close();

            Response.Write(sw.ToString());
            Response.End();
        }
    }
}