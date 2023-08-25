using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectNet.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

//Thiet lap ket noi
var ConnectionString = builder.Configuration.GetConnectionString("NoiThatDB");
builder.Services.AddDbContext<QLNoiThatDBContext>(options => options.UseSqlServer(ConnectionString));
builder.Services.AddControllersWithViews();
//end

//khai bao su dung session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromSeconds(120);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
    option.Cookie.Name = "DucChinh";
});
builder.Services.ConfigureApplicationCookie(option =>
{
    option.Cookie.HttpOnly = true;
    option.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    option.LoginPath = "/User/Login";
});

//end



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//khai bao su dung session
app.UseSession();
//end

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SanPhams}/{action=TrangChu}/{id?}");

app.Run();
