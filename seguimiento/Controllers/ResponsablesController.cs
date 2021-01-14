using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
    public class ResponsablesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public ResponsablesController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        [Authorize(Policy = "Configuracion.Responsable")]
        public async Task<IActionResult> Index2()
        {
            
            var responsables = await db.Responsable.Where(n => n.IdJefe == 0).ToListAsync();
            return View(responsables);
        }


        public List<int> GetAllIdsFromResponsable(int id)
        {
            Responsable responsable = db.Responsable.Find(id);
            List<int> ids = new List<int>();
            ids.Add(responsable.Id);

            List<Responsable> responsablesIn = new List<Responsable>();
            List<Responsable> responsablesOut = new List<Responsable>();
            responsablesIn.Add(responsable);

            while (responsablesIn.Count() > 0)
            {
                responsablesOut.Clear();
                foreach (Responsable respIn in responsablesIn)
                {
                    var responsablesX = db.Responsable.Where(n => n.IdJefe == respIn.Id).ToList();
                    foreach (var respX in responsablesX)
                    {
                        responsablesOut.Add(respX);
                        ids.Add(respX.Id);
                    }
                }
                responsablesIn.Clear();
                responsablesIn.AddRange(responsablesOut);

            }

            return ids;
        }
    }
}
