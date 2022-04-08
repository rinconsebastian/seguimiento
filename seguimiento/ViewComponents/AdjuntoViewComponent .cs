using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.ViewComponents
{
    public class AdjuntoViewComponent : ViewComponent
    {
        private readonly IWebHostEnvironment _env;

        public AdjuntoViewComponent(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<IViewComponentResult> InvokeAsync(String fileName, String name = null)
        { 
            if(name == null) { name = Path.GetFileNameWithoutExtension(fileName); }
            var _path = Path.Combine("../", "UploadedFiles", fileName);
            var ext = Path.GetExtension(fileName);

            var icon = "fas fa-file text-dark";
            switch (ext.ToLower())
            {
                case ".doc":
                case ".docx":
                    icon = "fas fa-file-word text-primary";
                    break;
                case ".xls":
                case ".xlsx":
                    icon = "fas fa-file-excel text-success ";
                    break;
                case ".ppt":
                    icon = "fas fa-file-powerpoint text-danger ";
                    break;
                case ".pdf":
                    icon = "fas fa-file-pdf text-danger ";
                    break;
                case ".mp4":
                    icon = "fas fa-file-video text-warning ";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                    icon = "fas fa-file-image text-info ";
                    break;
            }

            ViewBag.Name = name;
            ViewBag.Url = _path;
            ViewBag.Icon = icon;

            return await Task.FromResult((IViewComponentResult)View());
        }
    }
}
