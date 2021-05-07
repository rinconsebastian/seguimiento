using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using seguimiento.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using seguimiento.Data;
using Microsoft.AspNetCore.Identity;

namespace seguimiento.Controllers
{
    public class EjecucionAdjuntoController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> userManager;

        public EjecucionAdjuntoController(ApplicationDbContext context, IWebHostEnvironment env, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            _env = env;
            userManager = _userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file, string id, string time, string name = null)
        {
            String error = "";

            if (file != null && file.Length > 0)
            {
                var idInt = Int32.Parse(id);
                var ejecucion = await db.Ejecucion.FindAsync(idInt);
                if (ejecucion != null)
                {
                    try
                    {
                        var _path = Path.Combine(_env.ContentRootPath, "UploadedFiles");
                        //Crea la carpeta
                        var _pathFolder = Path.Combine(_path, "Adjuntos", id);
                        if (!Directory.Exists(_pathFolder))
                        {
                            Directory.CreateDirectory(_pathFolder);
                        }
                        //Carga el archivo
                        var _fileName = id + "-" + time + "-" + Path.GetFileName(file.FileName);
                        var _pathFile = Path.Combine(_pathFolder, _fileName);
                        using (var fileStream = new FileStream(_pathFile, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        //Crea la ejecucion
                        EjecucionAdjunto adjunto = new EjecucionAdjunto()
                        {
                            idejecucion = idInt,
                            Ejecucion = ejecucion,
                            nombre = name,
                            adjunto = Path.Combine("Adjuntos", id, _fileName)
                        };

                        db.EjecucionAdjunto.Add(adjunto);
                        db.SaveChanges();

                    }
                    catch (Exception e) { error = "Error: " + e.Message; }
                    
                }
                else { error = "Error: la ejecución no existe."; }

            }
            else { error = "Error: El archivo no es válido."; }
            return Json(error);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(String id)
        {
            String error = "";

            var idInt = Int32.Parse(id);
            var adjunto = await db.EjecucionAdjunto.FindAsync(idInt);
            if (adjunto != null)
            {
                ConfiguracionsController controlConfiguracion = new ConfiguracionsController(db, userManager);
                try
                {
                    var _path = Path.Combine(_env.ContentRootPath, "UploadedFiles");
                    var filepath = Path.Combine(_path, adjunto.adjunto);
                    System.IO.File.Delete(filepath);

                    db.EjecucionAdjunto.Remove(adjunto);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    error = "Error: " + controlConfiguracion.SqlErrorHandler(ex);
                }
            }
            else { error = "Error: el adjunto no existe."; }

            return Json(error);
        }
    }
}
