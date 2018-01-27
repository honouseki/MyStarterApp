using MyStarterApp.Models;
using MyStarterApp.Models.Domain;
using MyStarterApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;


namespace MyStarterApp.Services.Security
{
    public static class IIdentityExtensions
    {
        // Variation of GetUserId provided as part of Microsoft.AspNet.Identity.Core package.
        public static int? GetId(this IIdentity identity)
        {
            if (identity == null) { throw new ArgumentNullException("identity"); }
            ClaimsIdentity ci = identity as ClaimsIdentity;

            int idParsed = 0;


            // FindFirstValue provided in Microsoft.AspNet.Identity.Core package.
            if (ci != null)
            {
                Claim claim = ci.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (claim != null && Int32.TryParse(claim.Value, out idParsed))
                {
                    return idParsed;
                }


            }
            return null;
        }

        public static IEnumerable<string> GetRoles(this IIdentity identity)
        {
            if (identity == null) { throw new ArgumentNullException("identity"); }
            var ci = identity as ClaimsIdentity;
            return ci.FindAll(ci.RoleClaimType).Select(c => c.Value);
        }

        // Thin wrapper.
        public static Claim FindFirst(this IIdentity identity, string claimType)
        {
            if (identity == null) { throw new ArgumentNullException("identity"); }
            var ci = identity as ClaimsIdentity;
            return ci.FindFirst(claimType);
        }


        public static IUserAuthData GetCurrentUser(this IIdentity identity)
        {
            MyStarterApp.Models.Domain.UserBase baseUser = null;

            if (identity == null) { throw new ArgumentNullException("identity"); }

            if (identity.IsAuthenticated)
            {
                ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;

                if (claimsIdentity != null)
                {
                    baseUser = ExtractUser(claimsIdentity);
                }
            }


            return baseUser;
        }

        private static UserBase ExtractUser(ClaimsIdentity identity)
        {
            MyStarterApp.Models.Domain.UserBase baseUser = new Models.Domain.UserBase();

            foreach (var claim in identity.Claims)
            {
                switch (claim.Type)
                {
                    case ClaimTypes.NameIdentifier:
                        int id = 0;

                        if (Int32.TryParse(claim.Value, out id))
                        {
                            baseUser.UserId = id;
                        }

                        break;
                    case ClaimTypes.Name:
                        baseUser.Username = claim.Value;
                        break;
                    case ClaimTypes.Actor:
                        baseUser.RoleId = claim.Value;
                        break;
                    default:
                        break;
                }

            }

            return baseUser;
        }

    }
}
