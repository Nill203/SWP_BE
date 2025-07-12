using Microsoft.EntityFrameworkCore;
using BloodDonationBE.Features.Users;
using BloodDonationBE.Features.BloodDonationCampaigns;
using BloodDonationBE.Features.CampaignRegistrations;
using BloodDonationBE.Features.Hospitals;
using BloodDonationBE.Features.BloodUnits;
using BloodDonationBE.Common.Enums;
using BloodDonationBE.Features.BloodRequests; // <-- Thêm using cho module mới

namespace BloodDonationBE.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<BloodDonationCampaign> BloodDonationCampaigns { get; set; }
    public DbSet<CampaignRegistration> CampaignRegistrations { get; set; }
    public DbSet<Hospital> Hospitals { get; set; }
    public DbSet<BloodUnit> BloodUnits { get; set; }
    public DbSet<BloodRequest> BloodRequests { get; set; } // <-- Thêm DbSet cho bảng mới


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Cấu hình để lưu trữ tất cả các enums dưới dạng chuỗi (string) trong database ---
        
        // User Enums
        modelBuilder.Entity<User>().Property(u => u.BloodType).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<User>().Property(u => u.Gender).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<User>().Property(u => u.AvailabilityStatus).HasConversion<string>().HasMaxLength(20);

        // CampaignRegistration Enums
        modelBuilder.Entity<CampaignRegistration>().Property(cr => cr.Status).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<CampaignRegistration>().Property(cr => cr.ProductType).HasConversion<string>().HasMaxLength(20);

        // BloodUnit Enums
        modelBuilder.Entity<BloodUnit>().Property(bu => bu.Status).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<BloodUnit>().Property(bu => bu.ProductType).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<BloodUnit>().Property(bu => bu.BloodType).HasConversion<string>().HasMaxLength(20);

        // BloodRequest Enums (MỚI)
        modelBuilder.Entity<BloodRequest>().Property(br => br.Status).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<BloodRequest>().Property(br => br.BloodType).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<BloodRequest>().Property(br => br.ProductType).HasConversion<string>().HasMaxLength(20);

        // --- Cấu hình mối quan hệ phức tạp ---
        // Cấu hình các mối quan hệ từ BloodRequest đến User để tránh lỗi
        modelBuilder.Entity<BloodRequest>()
            .HasOne(br => br.RequestingUser)
            .WithMany()
            .HasForeignKey(br => br.RequestingUserId)
            .OnDelete(DeleteBehavior.Restrict); // Ngăn chặn xóa User nếu họ có yêu cầu

        modelBuilder.Entity<BloodRequest>()
            .HasOne(br => br.VerifyingStaff)
            .WithMany()
            .HasForeignKey(br => br.VerifyingStaffId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BloodRequest>()
            .HasOne(br => br.ApprovingAdmin)
            .WithMany()
            .HasForeignKey(br => br.ApprovingAdminId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- Kết thúc phần cấu hình ---

        // Cấu hình index
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
    }
}
