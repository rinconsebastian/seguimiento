using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seguimiento.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
    public class CamposController : Controller
    {

        private readonly ApplicationDbContext db;
        
        public CamposController(ApplicationDbContext context)
        {
            db = context;
           
        }

        [Authorize(Policy = "Indicador.Editar")]
        public async Task<bool> BorrarDeIndicador(int id)
        {
            bool error = true;
            var valorCampo = await db.ValorCampo.Where(n => n.IndicadorPadre.id == id).ToListAsync();

            try
            {
                db.RemoveRange(valorCampo);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                error = false;
            }



            return error;

        }
    }
}

