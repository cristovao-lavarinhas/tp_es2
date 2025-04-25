using esii.Context;
using esii.stratagies;
using esii.stratagies.Email;
using esii.stratagies.Password;
using esii.stratagies.Pin;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços MVC
builder.Services.AddControllersWithViews();

// Configura Entity Framework com PostgreSQL
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona suporte a sessões
builder.Services.AddSession();


builder.Services.AddScoped<PasswordLoginStrategy>();
builder.Services.AddScoped<PinLoginStrategy>();
builder.Services.AddScoped<OTPLoginStrategy>();
builder.Services.AddScoped<LoginContext>(provider => new LoginContext(new Dictionary<string, ILoginStrategy>
{
    { "password", provider.GetRequiredService<PasswordLoginStrategy>() },
    { "pin", provider.GetRequiredService<PinLoginStrategy>() },
    { "otp", provider.GetRequiredService<OTPLoginStrategy>() }
}));
builder.Services.AddScoped<IEmailSender, EmailSender>();


// Adiciona autenticação com cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Utilizador/Login";
        options.LogoutPath = "/Utilizador/Logout";
        options.AccessDeniedPath = "/Home/AccessDenied";
    });

var app = builder.Build();

// Configuração do pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Autenticação e sessões
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Rotas MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();