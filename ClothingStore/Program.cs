using ClothingStore.Factory;
using ClothingStore.Models;
using ClothingStore.Observer;
using ClothingStore.Repositories;
using ClothingStore.Repository;
using ClothingStore.Service;
using ClothingStore.Services;
using ClothingStore.Strategies;
using ClothingStore.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ClothingStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderSubject>();

builder.Services.AddScoped<IVoucherFactory, DefaultVoucherFactory>();
builder.Services.AddScoped<VoucherService>();


builder.Services.AddHttpClient();

// Register repositories and unit of work
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Thêm logging vào
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddScoped<AdminAuthStrategy>();
builder.Services.AddScoped<UserAuthStrategy>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session hết hạn sau 30 phút
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // 🔹 Đảm bảo cookie session luôn được gửi
    options.Cookie.SameSite = SameSiteMode.Strict;
});


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseStaticFiles();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Cus}/{controller=Product}/{action=Index}/{id?}");




app.Run();
