using Microsoft.AspNetCore.Identity;

namespace MedilaboSolutionsBack1.Data
{
    public static class IdentityInitializer
    {
        public static async Task Initialize(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Vérifie si la table Users contient des données
            if (userManager.Users.Any())
            {
                return; // La base de données est déjà initialisée
            }

            // Create default roles
            string[] roleNames = { "Organisateur", "Praticien" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Crée un Organisateur 
            var organisateur = new IdentityUser
            {
                UserName = "organisateur",
                Email = "organisateur@medilabo.com",
                EmailConfirmed = true
            };

            if (userManager.Users.All(u => u.UserName != organisateur.UserName))
            {
                var result = await userManager.CreateAsync(organisateur, "Organisateur@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(organisateur, "Organisateur");
                }
            }

            // Crée un Praticien
            var praticien = new IdentityUser
            {
                UserName = "praticien",
                Email = "praticien@medilabo.com",
                EmailConfirmed = true
            };

            if (userManager.Users.All(u => u.UserName != praticien.UserName))
            {
                var result = await userManager.CreateAsync(praticien, "Praticien@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(praticien, "Praticien");
                }
            }
        }
    }
}
