using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using seguimiento.Controllers;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.ViewComponents
{
    public class CategoriaRutaViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public CategoriaRutaViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(String numero, String id, String periodo, string alto, string ancho, string titulo, string tipo)
        {
            CategoriasController controlCategoria = new CategoriasController(db, userManager);

            var Numero = Int32.Parse(numero);
            var IdCategoria = Int32.Parse(id);
            var IdPeriodo = Int32.Parse(periodo);

            List<Categoria> Categorias = new List<Categoria>();


            Categoria categoria = await controlCategoria.getFromId(IdCategoria);
            Categorias.Add(categoria);

            while (categoria.CategoriaPadre != null)
            {
                Categorias.Add(categoria.CategoriaPadre);
                categoria = await controlCategoria.getFromId(categoria.CategoriaPadre.id);
            }



            ViewBag.Numero = Numero;
            ViewBag.alto = alto;
            ViewBag.ancho = ancho;
            ViewBag.titulo = titulo;
            ViewBag.periodo = periodo;
            Categorias.Reverse();
            ViewBag.tipo = tipo;

            //listado de periodos
            ViewBag.Periodos = new SelectList(await db.Periodo.Where(n => n.tipo == "periodo" || n.tipo == "subtotal" || n.tipo == "Total").OrderBy(n=>n.orden).ToListAsync(), "id", "nombre", System.Convert.ToInt32(periodo));

            return View(Categorias);

        }
    }
}
