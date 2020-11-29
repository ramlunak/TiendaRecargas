﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaRecargas.Models;

namespace TiendaRecargas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Cuenta> RT_Cuentas { get; set; }
        public DbSet<RecargaValor> RT_RecargaValores { get; set; }
        public DbSet<Recarga> RT_Recargas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecargaValor>().Property(e => e.tipoRecarga).HasConversion<string>();
        }
    }
}
