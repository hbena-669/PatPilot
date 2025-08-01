
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PatPilot.Models;

namespace PatPilot.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relation Enseigne - Gateau
            modelBuilder.Entity<Enseigne>()
                .HasMany(e => e.Gateaux)
                .WithOne(g => g.Enseigne)
                .HasForeignKey(g => g.EnseigneId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relation Gateau - Ingredient
            modelBuilder.Entity<Gateau>()
                .HasMany(g => g.Ingredients)
                .WithOne(i => i.Gateau)
                .HasForeignKey(i => i.GateauId)
                .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<Commande>()
               .HasOne(c => c.Client)
               .WithMany(cl => cl.Commandes)
               .HasForeignKey(c => c.ClientId)
               .OnDelete(DeleteBehavior.Restrict);

            // Relation entre Commande et Enseigne
            modelBuilder.Entity<Commande>()
                    .HasOne(c => c.Enseigne)
                    .WithMany(e => e.Commandes)
                    .HasForeignKey(c => c.EnseigneId)
                    .OnDelete(DeleteBehavior.Restrict);

            // Relation entre CommandeDetail et Commande
            modelBuilder.Entity<CommandeDetail>()
                    .HasOne(cd => cd.Commande)
                    .WithMany(c => c.commandeDetails)
                    .HasForeignKey(cd => cd.CommandeId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relation entre CommandeDetail et Gateau
                modelBuilder.Entity<CommandeDetail>()
                    .HasOne(cd => cd.Gateau)
                    .WithMany()
                    .HasForeignKey(cd => cd.GateauId)
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<ApplicationUser>()
                   .HasOne(u => u.Enseigne) // Navigation property
                   .WithMany(e => e.Users)  // Reverse navigation property
                   .HasForeignKey(u => u.EnseigneId) // Foreign key
                   .OnDelete(DeleteBehavior.Cascade); // Configure cascading behavior
        }

        /// <summary>
        /// Méthode pour seed les rôles et utilisateurs
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Définir les rôles
            string[] roles = { "Admin", "AdminEnseigne" , "Customer"};

            // Création des rôles
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Créer l'utilisateur "Admin"
            string adminEmail = "hbenats@free.fr";
            string adminPassword = "Admin@123"; // Remplacez par un mot de passe sécurisé

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                    //EnseigneId = Guid.Empty
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        /// <summary>
        /// Méthode pour seed les rôles et utilisateurs
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public DbSet<PatPilot.Models.Enseigne>? Enseigne { get; set; }
        public DbSet<Gateau> Gateaux { get; set; }

        public DbSet<Commande> Commandes { get; set; }
        public DbSet<CommandeDetail> CommandeDetails { get; set; }
        public DbSet<Client> Clients { get; set; }

        public DbSet<Evenement> Evenement { get; set; }

    }
}
