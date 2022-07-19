using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NTTCinemas.Data;


var builder = WebApplication.CreateBuilder(args);
var NTTCinemasIdentityConnect = builder.Configuration.GetConnectionString("NTTCinemasIdentity") ?? throw new InvalidOperationException("Connection string 'NTTCinemasContextConnection' not found.");

builder.Services.AddDbContext<NTTCinemasContext>(options =>
    options.UseSqlServer(NTTCinemasIdentityConnect));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<NTTCinemasContext>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.ConfigureApplicationCookie(options => {
    // options.Cookie.HttpOnly = true;  
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = $"/login/";
    options.LogoutPath = $"/login/";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

