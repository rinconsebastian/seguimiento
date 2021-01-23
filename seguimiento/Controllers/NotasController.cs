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
    public class NotasController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public NotasController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;

        }


        public async Task<int> NumeroNotasEstadoCategoriaId(int id, string estado)
        {
            var numero = await db.Nota.Where(n => n.Estado == estado && n.IdCategoria == id).CountAsync();
                
              //Database.SqlQuery<int>("SELECT COUNT(*) FROM Notas WHERE Estado= '" + estado + "' AND IdCategoria= " + id).First();

            return numero;
        }

        public async Task<ActionResult> Categoriapop(int categoriaid = 0, string tipo = "", string mensaje="")
        {
            CategoriasController controlCategoria = new CategoriasController(db, userManager);
            ResponsablesController controlResponsable = new ResponsablesController(db, userManager);
            ConfiguracionsController controlConfiguracion = new ConfiguracionsController(db, userManager);

            int ID = Convert.ToInt32(categoriaid);

            Categoria categoria = await controlCategoria.getFromId(ID);

            
            if (User.Identity.Name != null)
            {
                var userFull = await userManager.FindByEmailAsync(User.Identity.Name);

                // obtiene las categorias
                var idsx = controlResponsable.GetAllIdsFromResponsable(userFull.IDDependencia);

                var notasE = User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nota.Editar" && c.Value == "1"));
                var super = User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Super" && c.Value == "1"));



                ViewBag.tipo = tipo;
                ViewBag.mensaje = mensaje;
                ViewBag.editar = controlConfiguracion.Editable(userFull.IDDependencia, categoria, notasE, super);
                ViewBag.visible = true;
                ViewBag.configuracion = await controlConfiguracion.Get();
                ViewBag.categoria = categoria;
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


            var notas = await db.Nota.Include(n => n.Categoria).Include(n => n.User).Where(n => n.IdCategoria == ID).OrderByDescending(n => n.FechaCreacion).ToListAsync();
            return View(notas.ToList());
        }

        [Authorize(Policy = "Nota.Editar")]
        public async Task<ActionResult> Createpop(int id)
        {

            var userFull = await userManager.FindByEmailAsync(User.Identity.Name);
            //ViewBag.IdCategoria = new SelectList(db.Categorias, "id", "numero");
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Nombre");

            Categoria categoria =await db.Categoria.FindAsync(id);


            List<SelectListItem> usuarioLista = new List<SelectListItem>();
            usuarioLista.Add(new SelectListItem() { Text = userFull.Nombre + " " + userFull.Apellido, Value = userFull.Id });

            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            CategoriaLista.Add(new SelectListItem() { Text = categoria.numero + " " + categoria.nombre, Value = categoria.id.ToString() });

            List<SelectListItem> EstadoLista = new List<SelectListItem>();
            EstadoLista.Add(new SelectListItem() { Text = "Abierta", Value = "Abierta" });
            EstadoLista.Add(new SelectListItem() { Text = "Cerrada", Value = "Cerrada" });


            ViewBag.IdCategoria = new SelectList(CategoriaLista, "Value", "Text", "Vale");
            ViewBag.UserId = new SelectList(usuarioLista, "Value", "Text", "Value");
            ViewBag.Estado = new SelectList(EstadoLista, "Value", "Text", "Value");
            ViewBag.categ = id;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Nota.Editar")]
        public async Task<bool> CreatepopAsync( Nota nota)
        {
            try
            {
                db.Nota.Add(nota);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {


                Categoria categoria = await db.Categoria.FindAsync(nota.IdCategoria);
                List<SelectListItem> CategoriaLista = new List<SelectListItem>();
                CategoriaLista.Add(new SelectListItem() { Text = categoria.numero + " " + categoria.nombre, Value = categoria.id.ToString() });
                ViewBag.IdCategoria = new SelectList(CategoriaLista, "Value", "Text", "Vale");

                var userFull = await userManager.FindByEmailAsync(User.Identity.Name);
                List<SelectListItem> usuarioLista = new List<SelectListItem>();
                usuarioLista.Add(new SelectListItem() { Text = userFull.Nombre + " " + userFull.Apellido, Value = userFull.Id });
                ViewBag.UserId = new SelectList(usuarioLista, "Value", "Text", "Value");



                return false;
            }
        }

        public async Task<ActionResult> Detailspop(int id)
        {
            CategoriasController controlCategoria = new CategoriasController(db, userManager);
            ConfiguracionsController controlConfiguracion = new ConfiguracionsController(db, userManager);


            Nota nota = await db.Nota.FindAsync(id);
           
            Categoria categoria =await controlCategoria.getFromId(nota.Categoria.id);

           
            if (User.Identity.Name != null)
            {
                var userFull = await userManager.FindByEmailAsync(User.Identity.Name);
                var notasE = User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nota.Editar" && c.Value == "1"));
                var super = User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Super" && c.Value == "1"));


              
                ViewBag.editar = controlConfiguracion.Editable(userFull.IDDependencia, categoria, notasE, super);
                ViewBag.visible = true;
                ViewBag.configuracion = await controlConfiguracion.Get();
                ViewBag.categoria = categoria;
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
        public async Task<ActionResult> Editpop(int id, int idcategoria)
        {
            
            Nota nota = await db.Nota.FindAsync(id);
           

            var userFull = await userManager.FindByEmailAsync(User.Identity.Name);
            //ViewBag.IdCategoria = new SelectList(db.Categorias, "id", "numero");
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Nombre");

            Categoria categoria =await db.Categoria.FindAsync(idcategoria);


            List<SelectListItem> usuarioLista = new List<SelectListItem>();
            usuarioLista.Add(new SelectListItem() { Text = userFull.Nombre + " " + userFull.Apellido, Value = userFull.Id });

            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            CategoriaLista.Add(new SelectListItem() { Text = categoria.numero + " " + categoria.nombre, Value = categoria.id.ToString() });

            List<SelectListItem> EstadoLista = new List<SelectListItem>();
            EstadoLista.Add(new SelectListItem() { Text = "Abierta", Value = "Abierta" });
            EstadoLista.Add(new SelectListItem() { Text = "Cerrada", Value = "Cerrada" });


            ViewBag.IdCategoria = new SelectList(CategoriaLista, "Value", "Text", nota.IdCategoria);
            ViewBag.UserId = new SelectList(usuarioLista, "Value", "Text", nota.UserId);
            ViewBag.Estado = new SelectList(EstadoLista, "Value", "Text", nota.Estado);

            return View(nota);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Nota.Editar")]
        public async Task<bool> Editpop(Nota nota)
        {
            //----------------INICIO EVITA QUE UN USUARIO PUEDA MODIFICAR UNA NOTA CAMBIANDO EL DATA-ID

            string notaoldUserId = await db.Nota.Where(n => n.Id == nota.Id).Select(n => n.UserId).FirstOrDefaultAsync();

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

            Categoria categoria = await db.Categoria.FindAsync(nota.IdCategoria);
            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            CategoriaLista.Add(new SelectListItem() { Text = categoria.numero + " " + categoria.nombre, Value = categoria.id.ToString() });
            ViewBag.IdCategoria = new SelectList(CategoriaLista, "Value", "Text", "Vale");

           
            List<SelectListItem> usuarioLista = new List<SelectListItem>();
            usuarioLista.Add(new SelectListItem() { Text = userFull.Nombre + " " + userFull.Apellido, Value = userFull.Id });
            ViewBag.UserId = new SelectList(usuarioLista, "Value", "Text", "Value");
            return false;
        }

        [Authorize(Policy = "Nota.Editar")]
        public async Task<ActionResult> Deletepop(int id)
        {
            CategoriasController controlCategoria = new CategoriasController(db, userManager);
            ConfiguracionsController controlConfiguracion = new ConfiguracionsController(db, userManager);

            Nota nota = await db.Nota.FindAsync(id);
            
            Categoria categoria = await controlCategoria.getFromId(nota.Categoria.id);


            var userFull = await userManager.FindByEmailAsync(User.Identity.Name);
            var notasE = User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nota.Editar" && c.Value == "1"));
            var super = User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Super" && c.Value == "1"));



            ViewBag.editar = controlConfiguracion.Editable(userFull.IDDependencia, categoria, notasE, super);
            ViewBag.visible = true;
            ViewBag.configuracion = await controlConfiguracion.Get();
            ViewBag.categoria = categoria;
            ViewBag.userFull = userFull;

           
            return View(nota);
        }


        [HttpPost, ActionName("Deletepop")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Nota.Editar")]
        public async Task<bool> DeleteConfirmedpop(int id)
        {
            Nota nota = await db.Nota.FindAsync(id);

            var userFull = await userManager.FindByEmailAsync(User.Identity.Name);

            if(userFull.Id != nota.UserId )
            { return false; }
            //----------------INICIO EVITA QUE UN USUARIO PUEDA MODIFICAR UNA NOTA CAMBIANDO EL DATA-ID



            db.Nota.Remove(nota);
            await db.SaveChangesAsync();
            return true;
        }

    }
}
