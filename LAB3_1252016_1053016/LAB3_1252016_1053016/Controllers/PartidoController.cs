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
        List<string> aux = new List<string>();
        Nodo<Partido> searched = new Nodo<Partido>();  
        // GET: Partido
        public ActionResult Index()
        {            
            return View();
        }

        /// <summary>
        /// Crear DropDownList para seleccionar campo a buscar
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult Menu(FormCollection form)
        {
            string searchBy = form["comboBox"];
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
                                logs.Add("Se ha insertado un nuevo partido " + partido);
                                aux = AVLPartido.Rotaciones(); 
                            }

                            for (int i = 0; i < aux.Count; i++)
                            {
                                logs.Add(aux.ElementAt(i));
                            }
                            Session["AVLPartido"] = AVLPartido;
                            Session["Logs"] = logs;
                            List<Nodo<Partido>> tempList = AVLPartido.InOrden(AVLPartido.cabeza);                             
                            return View("PartidoSuccess", tempList);
                        }
                    }
                }
            }
            return View("Menu"); 
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
            string Partido = partido.Pais1 + " vs " + partido.Pais2;
            logs.Add("Se ha insertado un nuevo partido " + Partido);
            aux = AVLPartido.Rotaciones();
            for (int i = 0; i < aux.Count; i++)
            {
                logs.Add(aux.ElementAt(i));
            }
            Session["AVLPartido"] = AVLPartido;
            Session["Logs"] = logs;
            return View("InsertarManualView"); 
        }

        public ActionResult Delete(int id)
        {
            AVLPartido = (AVLTree<Partido>)Session["AVLPartido"];
            logs = (List<string>)Session["Logs"]; 
            AVLPartido.Limpiar();
            List<Nodo<Partido>> tempList = AVLPartido.InOrden(AVLPartido.cabeza);
            AVLPartido.Delete(tempList.ElementAt(id).Value, AVLPartido.cabeza);            
            logs.Add("Se ha eliminado un partido " + tempList.ElementAt(id).Value.Pais1 + "vs" + tempList.ElementAt(id).Value.Pais2);
            aux = AVLPartido.RotacionesDelete();
            for (int i = 0; i < aux.Count; i++)
            {
                logs.Add(aux.ElementAt(i));
            }
            Session["AVLPartido"] = AVLPartido;
            Session["Logs"] = logs;
            AVLPartido.Limpiar();
            tempList = AVLPartido.InOrden(AVLPartido.cabeza);
            return View("PartidoSuccess", tempList); 
        }

        public ActionResult DeleteRedirect()
        {
            AVLPartido = (AVLTree<Partido>)Session["AVLPartido"];
            AVLPartido.Limpiar();
            List<Nodo<Partido>> tempList = AVLPartido.InOrden(AVLPartido.cabeza);           
            Session["AVLPartido"] = AVLPartido;
            AVLPartido.Limpiar();
            tempList = AVLPartido.InOrden(AVLPartido.cabeza);
            return View("PartidoSuccess", tempList);
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View(); 
        }

        [HttpPost]
        public ActionResult Search(FormCollection form)
        {
            string param = form["searching"];
            Partido search = new Partido
            {
                NoPartido = Convert.ToInt32(param)
            };
            AVLPartido = (AVLTree<Partido>)Session["AVLPartido"];
            searched = AVLPartido.Search(search, AVLPartido.cabeza);
            search = (Partido)searched.Value;
            return View("SearchSuccess", search); 
        }

        public ActionResult Download()
        {
            PrintLogs(); 
            return View("DownloadSuccess"); 
        }

       
        private void PrintLogs()
        {
            logs = (List<string>)Session["Logs"]; 
            StringWriter sw = new StringWriter();
            sw.WriteLine("Logs de " + DateTime.Now.ToShortDateString());
            Response.ClearContent();
            Response.AddHeader("Content-disposition", "Attachment;filename=Logs.txt");
            Response.ContentType = "text,out";

            for (int i = 0; i < this.logs.Count; i++)
            {
                sw.WriteLine(this.logs.ElementAt(i));
            }         

            Response.Write(sw.ToString());
            Response.End();
            sw.Close();
        }  


    }    
}