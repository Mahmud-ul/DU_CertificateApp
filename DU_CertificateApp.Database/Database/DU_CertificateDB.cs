using DU_CertificateApp.Database.Migrations;
using DU_CertificateApp.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DU_CertificateApp.Database.Database
{
    public class DU_CertificateDB : DbContext
    {
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OrderCertificate> OrderCertificates { get; set; }

        //Release        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"Server=DU_CertificateAppDB.mssql.somee.com;
                                Database=DU_CertificateAppDB;
                                User Id=tipu1995_SQLLogin_1;
                                Password=2y94apg8yu;
                                TrustServerCertificate=True;
                                Encrypt=False;
                                Persist Security Info=False;";
            optionsBuilder.UseSqlServer(connectionString);
        }

        //Develop
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    string connectionString = @"Data Source = TIPU\SQLEXPRESS;
        //                                Database = DU_CertificateDB;
        //                                Trusted_Connection = True;
        //                                Encrypt = False;";
        //    optionsBuilder.UseSqlServer(connectionString);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certificate>().HasIndex(b => b.Name).IsUnique();
            modelBuilder.Entity<Department>().HasIndex(b => b.Name).IsUnique();
            modelBuilder.Entity<PaymentMethod>().HasIndex(b => b.Name).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(b => b.Name).IsUnique();
            modelBuilder.Entity<Student>().HasIndex(b => b.Registration).IsUnique();
            modelBuilder.Entity<Student>().HasIndex(b => b.ExamRoll).IsUnique();
            modelBuilder.Entity<User>().HasIndex(b => b.UserName).IsUnique();
            modelBuilder.Entity<User>().HasIndex(b => b.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(b => b.Phone).IsUnique();

            modelBuilder.Entity<OrderCertificate>().HasKey(c => new { c.OrderID, c.CertificateID });

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
