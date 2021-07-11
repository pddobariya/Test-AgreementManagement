using AgreementManagement.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgreementManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new ProductGroupMap(modelBuilder.Entity<ProductGroup>());
            new ProductMap(modelBuilder.Entity<Product>());
            new AgreementMap(modelBuilder.Entity<Agreement>());
        }

        DbSet<ProductGroup> ProductGroup { get; set; }
        DbSet<Product> Product { get; set; }
        DbSet<Agreement> Agreement { get; set; }
    }
}
