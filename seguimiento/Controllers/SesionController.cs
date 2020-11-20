using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
    public class SesionController : Controller
    {
        public void Set(string email)
        {

            AccountController usuarioControl = new AccountController();
            ResponsablesController responsableControl = new ResponsablesController();
            PermisoRolsController permisosControl = new PermisoRolsController();
            ConfiguracionsController configuracionControl = new ConfiguracionsController();

            ApplicationUser usuario = usuarioControl.getFromEmail(email);
            Responsable dependencia = responsableControl.get(usuario);
            var rol = usuario.Roles;

            var rol2 = rol.FirstOrDefault();

            Configuracion configuracion = configuracionControl.Get();
            PermisoRol permisos = permisosControl.get(rol2.RoleId);

            if (permisos == null)
            {
                permisos = new PermisoRol();
            }


            Sesion sesion = new Sesion();
            sesion.permisos = permisos;
            sesion.responsable = dependencia;
            sesion.usuario = usuario;
            sesion.configuracion = configuracion;
            System.Web.HttpContext.Current.Session["sessionString"] = "hola";
            System.Web.HttpContext.Current.Session["sesion"] = sesion;
        }
    }
}
