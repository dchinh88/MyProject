using Microsoft.EntityFrameworkCore;
using ProjectNet.Models;

namespace ProjectNet.Models
{
    public class QLNoiThatDBContext : DbContext
    {
        public QLNoiThatDBContext()
        {
        }

        public QLNoiThatDBContext(DbContextOptions<QLNoiThatDBContext> options) : base(options) { }
        public DbSet<LoaiSanPham> loaiSanPhams { get; set; }
        public DbSet<SanPham> sanPhams { get; set; }
        public DbSet<NhanVien> nhanViens { get; set; }
        public DbSet<KHACHHANG> kHACHHANGs { get; set; }
        public DbSet<Phuongthucthanhtoan> phuongthucthanhtoans { get; set; }
        public DbSet<Tinhtrang> tinhtrangs { get; set; }
        public DbSet<Hoadon> hoadons { get; set; }
        public DbSet<Chitiethoadon> chitiethoadons { get; set; }
        public DbSet<Binhluan> binhluans { get; set; }
        public DbSet<Account> accounts { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoaiSanPham>().ToTable("LOAISANPHAM");
            modelBuilder.Entity<SanPham>().ToTable("SANPHAM");
            modelBuilder.Entity<NhanVien>().ToTable("NHANVIEN");
            modelBuilder.Entity<KHACHHANG>().ToTable("KHACHHANG");
            modelBuilder.Entity<Phuongthucthanhtoan>().ToTable("PHUONGTHUCTHANHTOAN");
            modelBuilder.Entity<Tinhtrang>().ToTable("TINHTRANG");
            modelBuilder.Entity<Hoadon>().ToTable("HOADON");
            modelBuilder.Entity<Chitiethoadon>().ToTable("CTHOADON");
            modelBuilder.Entity<Binhluan>().ToTable("BINHLUAN");
            modelBuilder.Entity<Account>().ToTable("ACCOUNT");
        }



       
    }
}
