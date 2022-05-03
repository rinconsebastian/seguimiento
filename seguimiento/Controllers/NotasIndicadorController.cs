using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
    public class NotasIndicadorController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public NotasIndicadorController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;

        }


        public async Task<int> NumeroNotasEstadoIndicadorId(int id, string estado)
        {
            var numero = await db.NotaIndicador.Where(n => n.Estado == estado && n.IdIndicador == id).CountAsync();
                
              //Database.SqlQuery<int>("SELECT COUNT(*) FROM Notas WHERE Estado= '" + estado + "' AND IdCategoria= " + id).First();

            return numero;
        }

        public async Task<ActionResult> Indicadorpop(int indicadorid = 0, string tipo = "", string mensaje="")
        {
            IndicadorsController controlIndicador = new  IndicadorsController(db, userManager);
            ResponsablesController controlResponsable = new ResponsablesController(db, userManager);
            ConfiguracionsController controlConfiguracion = new ConfiguracionsController(db, userManager);

            int ID = Convert.ToInt32(indicadorid);

            Indicador indicador = await controlIndicador.getFromId(ID);

            
            if (User.Identity.Name != null)
            {
                var userFull = await userManager.FindByEmailAsync(User.Identity.Name);

                // obtiene las categorias
                var idsx = controlResponsable.GetAllIdsFromResponsable(userFull.IDDependencia);

                var notasE = User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nota.Editar" && c.Value == "1"));
                var super = User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Super" && c.Value == "1"));



                ViewBag.tipo = tipo;
                ViewBag.mensaje = mensaje;
                ViewBag.editar = controlConfiguracion.Editable(userFull.IDDependencia, indicador.Categoria, notasE, super);
                ViewBag.visible = true;
                ViewBag.configuracion = await controlConfiguracion.Get();
                ViewBag.indicador = indicador;
                ViewBag.userFull = userFull;

                ViewBag.PermisoResponsable = idsx;
            }
            else
            {
                Configuracion config = await controlConfiguracion.Get();
                ViewBag.tipo = tipo;
                ViewBag.mensaje = mensaje;


                ViewBag.editar = false;
                ViewBag.userFull = false;
                ViewBag.configuracion = config;
                ViewBag.visible = config.libre;
                ViewBag.PermisoResponsable = false;

            }


            var notas = await db.NotaIndicador.Include(n => n.Indicador).Include(n => n.User).Where(n => n.IdIndicador == ID).OrderByDescending(n => n.FechaCreacion).ToListAsync();
            return View(notas.ToList());
        }

        [Authorize(Policy = "Nota.Editar")]
        public async Task<ActionResult> Createpop(int id)
        {

            var userFull = await userManager.FindByEmailAsync(User.Identity.Name);
            //ViewBag.IdCategoria = new SelectList(db.Categorias, "id", "numero");
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Nombre");

            Indicador indicador =await db.Indicador.FindAsync(id);


            List<SelectListItem> usuarioLista = new List<SelectListItem>();
            usuarioLista.Add(new SelectListItem() { Text = userFull.Nombre + " " + userFull.Apellido, Value = userFull.Id });

            List<SelectListItem> IndicadorLista = new List<SelectListItem>();
            IndicadorLista.Add(new SelectListItem() { Text = indicador.codigo + " " + indicador.nombre, Value = indicador.id.ToString() });

            List<SelectListItem> EstadoLista = new List<SelectListItem>();
            EstadoLista.Add(new SelectListItem() { Text = "Abierta", Value = "Abierta" });
            EstadoLista.Add(new SelectListItem() { Text = "Cerrada", Value = "Cerrada" });


            ViewBag.IdIndicador = new SelectList(IndicadorLista, "Value", "Text", "Vale");
            ViewBag.UserId = new SelectList(usuarioLista, "Value", "Text", "Value");
            ViewBag.Estado = new SelectList(EstadoLista, "Value", "Text", "Value");
            ViewBag.categ = id;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Nota.Editar")]
        public async Task<bool> CreatepopAsync( NotaIndicador nota)
        {
            try
            {
                db.NotaIndicador.Add(nota);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {


                Indicador indicador = await db.Indicador.FindAsync(nota.IdIndicador);
                List<SelectListItem> IndicadorLista = new List<SelectListItem>();
                IndicadorLista.Add(new SelectListItem() { Text = indicador.codigo + " " + indicador.nombre, Value = indicador.id.ToString() });
                ViewBag.IdIndicador = new SelectList(IndicadorLista, "Value", "Text", "Vale");

                var userFull = await userManager.FindByEmailAsync(User.Identity.Name);
                List<SelectListItem> usuarioLista = new List<SelectListItem>();
                usuarioLista.Add(new SelectListItem() { Text = userFull.Nombre + " " + userFull.Apellido, Value = userFull.Id });
                ViewBag.UserId = new SelectList(usuarioLista, "Value", "Text", "Value");



                return false;
            }
        }

        public async Task<ActionResult> Detailspop(int id)
        {
            IndicadorsController controlIndicador = new IndicadorsController(db, userManager);
            ConfiguracionsController controlConfiguracion = new ConfiguracionsController(db, userManager);


            NotaIndicador nota = await db.NotaIndicador.FindAsync(id);
           
            Indicador indicador =await controlIndicador.getFromId(nota.Indicador.id);

           
            if (User.Identity.Name != null)
            {
                var userFull = await userManager.FindByEmailAsync(User.Identity.Name);
                var notasE = User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nota.Editar" && c.Value == "1"));
                var super = User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Super" && c.Value == "1"));


              
                ViewBag.editar = controlConfiguracion.Editable(userFull.IDDependencia, indicador.Categoria, notasE, super);
                ViewBag.visible = true;
                ViewBag.configuracion = await controlConfiguracion.Get();
                ViewBag.indicador = indicador;
                ViewBag.userFull = userFull;

            }
            else
            {
                Configuracion config = await controlConfiguracion.Get();



                ViewBag.editar = false;
                ViewBag.userFull = false;
                ViewBag.configuracion = config;
                ViewBag.visible = config.libre;

            }




            return View(nota);
        }

        [Authorize(Policy = "Nota.Editar")]
        public async Task<ActionResult> Editpop(int id, int idindicador)
        {

            NotaIndicador nota = await db.NotaIndicador.FindAsync(id);
           

            var userFull = await userManager.FindByEmailAsync(User.Identity.Name);
            //ViewBag.IdCategoria = new SelectList(db.Categorias, "id", "numero");
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Nombre");

            Indicador indicador =await db.Indicador.FindAsync(idindicador);


            List<SelectListItem> usuarioLista = new List<SelectListItem>();
            usuarioLista.Add(new SelectListItem() { Text = userFull.Nombre + " " + userFull.Apellido, Value = userFull.Id });

            List<SelectListItem> IndicadorLista = new List<SelectListItem>();
            IndicadorLista.Add(new SelectListItem() { Text = indicador.codigo + " " + indicador.nombre, Value = indicador.id.ToString() });

            List<SelectListItem> EstadoLista = new List<SelectListItem>();
            EstadoLista.Add(new SelectListItem() { Text = "Abierta", Value = "Abierta" });
            EstadoLista.Add(new SelectListItem() { Text = "Cerrada", Value = "Cerrada" });


            ViewBag.IdIndicador = new SelectList(IndicadorLista, "Value", "Text", nota.IdIndicador);
            ViewBag.UserId = new SelectList(usuarioLista, "Value", "Text", nota.UserId);
            ViewBag.Estado = new SelectList(EstadoLista, "Value", "Text", nota.Estado);

            return View(nota);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Nota.Editar")]
        public async Task<bool> Editpop(NotaIndicador nota)
        {
            //----------------INICIO EVITA QUE UN USUARIO PUEDA MODIFICAR UNA NOTA CAMBIANDO EL DATA-ID

            string notaoldUserId = await db.NotaIndicador.Where(n => n.Id == nota.Id).Select(n => n.UserId).FirstOrDefaultAsync();

            var userFull = await userManager.FindByEmailAsync(User.Identity.Name);

            if (User.Identity.Name != null)
            {
               

                if (userFull.Id == notaoldUserId)
                {
                    //----------------INICIO EVITA QUE UN USUARIO PUEDA MODIFICAR UNA NOTA CAMBIANDO EL DATA-ID

                    if (ModelState.IsValid)
                    {
                        db.Entry(nota).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return true;
                    }
                }
            }

            Indicador indicador = await db.Indicador.FindAsync(nota.IdIndicador);
            List<SelectListItem> IndicadorLista = new List<SelectListItem>();
            IndicadorLista.Add(new SelectListItem() { Text = indicador.codigo + " " + indicador.nombre, Value = indicador.id.ToString() });
            ViewBag.IdIndicador = new SelectList(IndicadorLista, "Value", "Text", "Vale");

           
            List<SelectListItem> usuarioLista = new List<SelectListItem>();
            usuarioLista.Add(new SelectListItem() { Text = userFull.Nombre + " " + userFull.Apellido, Value = userFull.Id });
            ViewBag.UserId = new SelectList(usuarioLista, "Value", "Text", "Value");
            return false;
        }

        [Authorize(Policy = "Nota.Editar")]
        public async Task<ActionResult> Deletepop(int id)
        {
            IndicadorsController controlIndicador = new IndicadorsController(db, userManager);
            ConfiguracionsController controlConfiguracion = new ConfiguracionsController(db, userManager);

            NotaIndicador nota = await db.NotaIndicador.FindAsync(id);
            
            Indicador indicador = await controlIndicador.getFromId(nota.Indicador.id);


            var userFull = await userManager.FindByEmailAsync(User.Identity.Name);
            var notasE = User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nota.Editar" && c.Value == "1"));
            var super = User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Super" && c.Value == "1"));



            ViewBag.editar = controlConfiguracion.Editable(userFull.IDDependencia, indicador.Categoria, notasE, super);
            ViewBag.visible = true;
            ViewBag.configuracion = await controlConfiguracion.Get();
            ViewBag.indicador = indicador;
            ViewBag.userFull = userFull;

           
            return View(nota);
        }


        [HttpPost, ActionName("Deletepop")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Nota.Editar")]
        public async Task<bool> DeleteConfirmedpop(int id)
        {
            NotaIndicador nota = await db.NotaIndicador.FindAsync(id);

            var userFull = await userManager.FindByEmailAsync(User.Identity.Name);

            if(userFull.Id != nota.UserId )
            { return false; }
            //----------------INICIO EVITA QUE UN USUARIO PUEDA MODIFICAR UNA NOTA CAMBIANDO EL DATA-ID



            db.NotaIndicador.Remove(nota);
            await db.SaveChangesAsync();
            return true;
        }

    }
}
