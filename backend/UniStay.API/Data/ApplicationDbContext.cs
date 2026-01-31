using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UniStay.API.Helper;
using UniStay.API.Helper.BaseClasses;
using UniStay.API.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;


namespace UniStay.API.Data;

public class ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : DbContext(options)
{

    public DbSet<SecurityQuestion> SecurityQuestions { get; set; }
    public DbSet<UserSecurityAnswer> UserSecurityAnswers { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    public DbSet<Announcements> Announcement { get; set; }
    public DbSet<MyAuthInfo> MyAuthInfo { get; set; }
    public DbSet<TwoFactorCode> TwoFactorCode { get; set; }
    public DbSet<TwoFactorSettings> TwoFactorSettings { get; set; }
    public DbSet<BackupCode> BackupCode { get; set; }
    public DbSet<MyAuthenticationToken> MyAuthenticationTokens { get; set; }
    public DbSet<Applications> Application { get; set; }
    public DbSet<BedAssignments> BedAssignment { get; set; }
    public DbSet<Beds> Bed { get; set; }
    public DbSet<EquipmentRecords> EquipmentRecord { get; set; }
    public DbSet<Equipments> Equipment { get; set; }
    public DbSet<Faults> Fault { get; set; }
    public DbSet<HallReservations> HallReservation { get; set; }
    public DbSet<Halls> Hall { get; set; }
    public DbSet<Invoices> Invoice { get; set; }
    public DbSet<Messages> Message { get; set; }
    public DbSet<Payments> Payment { get; set; }
    public DbSet<Roles> Role { get; set; }
    public DbSet<Rooms> Room { get; set; }
    public DbSet<Users> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Applications>()
            .HasOne(a => a.Student)
            .WithMany()
            .HasForeignKey(a => a.StudentID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Applications>()
            .HasOne(a => a.DecisionByUser)
            .WithMany()
            .HasForeignKey(a => a.DecisionByUserID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payments>()
            .HasOne(p => p.Student)
            .WithMany()
            .HasForeignKey(p => p.StudentID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payments>()
            .HasOne(p => p.Invoice)
            .WithMany()
            .HasForeignKey(p => p.InvoiceID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EquipmentRecords>()
           .HasOne(a => a.Student)
           .WithMany()
           .HasForeignKey(a => a.StudentID)
           .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EquipmentRecords>()
            .HasOne(a => a.Employee)
            .WithMany()
            .HasForeignKey(a => a.EmployeeID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Messages>()
            .HasOne(a => a.SenderUser)
            .WithMany()
            .HasForeignKey(a => a.SenderUserID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Messages>()
            .HasOne(a => a.ReceiverUser)
            .WithMany()
            .HasForeignKey(a => a.ReceiverUserID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EquipmentRecords>()
            .HasOne(r => r.Equipment)
            .WithMany()
            .HasForeignKey(r => r.EquipmentID)
            .OnDelete(DeleteBehavior.Restrict);

        /* modelBuilder.Entity<EquipmentRecords>()
             .HasOne(r => r.EquipmentItem)
             .WithMany()
             .HasForeignKey(r => r.EquipmentItemID)
             .OnDelete(DeleteBehavior.Restrict);
        */


        modelBuilder.Entity<EquipmentRecords>()
             .HasOne(r => r.Equipment)
             .WithMany(e => e.EquipmentRecords)
             .HasForeignKey(r => r.EquipmentID)
             .OnDelete(DeleteBehavior.Restrict);


        
       modelBuilder.Entity<EquipmentRecords>()
            .HasOne(r => r.Student)
            .WithMany()
            .HasForeignKey(r => r.StudentID)
            .OnDelete(DeleteBehavior.Restrict);
        /*
        modelBuilder.Entity<EquipmentRecords>()
            .HasOne(r => r.Employee)
            .WithMany()
            .HasForeignKey(r => r.EmployeeID)
            .OnDelete(DeleteBehavior.Restrict);*/

        modelBuilder.Entity<Invoices>()
    .Property(i => i.TotalAmount)
    .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Payments>()
            .Property(p => p.Amount)
            .HasColumnType("decimal(18,2)");

    }
}
    