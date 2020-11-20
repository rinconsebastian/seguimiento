using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace seguimiento.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string Nombre { get; set; }


        public string Apellido { get; set; }


        public int IDDependencia { get; set; }


        public virtual ICollection<Nota> Notas { get; set; }

       /* public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            return userIdentity;
        }*/
    }
}
