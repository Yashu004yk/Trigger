using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class YourDbContext : DbContext
{
    public YourDbContext()
    {
    }

    public YourDbContext(DbContextOptions<YourDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Table1> Table1s { get; set; }

    public virtual DbSet<Table2> Table2s { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=Linq;Username=postgres;Password=Root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Table1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("table1_pkey");

            entity.ToTable("table1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Column1)
                .HasMaxLength(50)
                .HasColumnName("column_1");
            entity.Property(e => e.Column2)
                .HasMaxLength(50)
                .HasColumnName("column_2");
            entity.Property(e => e.Column3)
                .HasMaxLength(50)
                .HasColumnName("column_3");
            entity.Property(e => e.Column4)
                .HasMaxLength(50)
                .HasColumnName("column_4");
            entity.Property(e => e.CommonColumn)
                .HasMaxLength(50)
                .HasColumnName("common_column");
        });

        modelBuilder.Entity<Table2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("table2_pkey");

            entity.ToTable("table2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Column5)
                .HasMaxLength(50)
                .HasColumnName("column_5");
            entity.Property(e => e.Column6)
                .HasMaxLength(50)
                .HasColumnName("column_6");
            entity.Property(e => e.Column7)
                .HasMaxLength(50)
                .HasColumnName("column_7");
            entity.Property(e => e.Column8)
                .HasMaxLength(50)
                .HasColumnName("column_8");
            entity.Property(e => e.CommonColumn)
                .HasMaxLength(50)
                .HasColumnName("common_column");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
