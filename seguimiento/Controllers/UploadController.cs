using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using seguimiento.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace seguimiento.Controllers
{
    public class UploadController : Controller
    {

        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }
     
        // GET: Upload  
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, string id)
        {
            Archivo archivo = new Archivo();
            try
            {
                if (file != null && file.Length > 0)
                {
                    string _fileName = id + "-" + Path.GetFileName(file.FileName);
                    var _path = Path.Combine(_env.WebRootPath, "UploadedFiles", _fileName);
                    using (var fileStream = new FileStream(_path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    archivo.Loaded = true;
                    archivo.Nombre = _fileName;
                    archivo.Ruta = _path;
                }
                return Json(archivo);
            }
            catch (Exception e)
            {
                archivo.Loaded = false;
                return Json(archivo);
            }
        }
    }
}
