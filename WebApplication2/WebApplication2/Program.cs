using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2;

public class Program
{
    private static String SQL_SERVER_CONNECTION_STRING=
                        $"Server=localhost; database=arfosit; user id=sa; password="
                        +$"{Environment.GetEnvironmentVariable("SQL_SERVER_PASSWORD")};"
                        +$" TrustServerCertificate=True;";
    private static String MYSQL_CONNECTION_STRING=
                        $"Server=localhost; database=arfosit; user id=root; password="
                        +$"{Environment.GetEnvironmentVariable("MYSQL_PASSWORD")};";
    private static String POSTGRESQL_CONNECTION_STRING=
                        $"Server=localhost; database=postgres; user id=root; password="
                        +$"{Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD")};";
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        
       //builder.Services.Add ile başlayan metodlar DI için kullanılır
        //Mesela alt satırdaki AddDbContext<>() metodu ile aşağıdaki AddScoped<>() 
        //aynı işi farklı yöntemlerle yaparlar
        //Aynı şekilde builder.Services.AddSingleton da öyle.
        //Çakışma olmaması için yorum satırına aldım
        //builder.Services.AddScoped<Context>();
        //Ama normalde AddScoped<>() Dependency Injection için kullanılıyor
        builder.Services.AddScoped<ConverterToDTO>();
        builder.Services.AddDbContext<Context>(options => 
        options.UseMySql(MYSQL_CONNECTION_STRING
                                ,ServerVersion.AutoDetect(MYSQL_CONNECTION_STRING)));
        //Not: eğer mysql kullanılacaksa UseMysql fonksiyonuna ikinci parametre yani 
        //,ServerVersion.AutoDetect(MYSQL_CONNECTION_STRING kodu gerekli

        //Not: builder.Configuration.GetConnectionString() metodu appsettings.json içinden connection string çekmek için kullanılır.

        builder.Services.AddDistributedMemoryCache();

        //aşağıdaki kod session için ayarlar zorunlu değil ama gerekli özellikle de IsEssential
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(20);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        // MVC veya ControllersWithViews ekle
        builder.Services.AddControllersWithViews();

        

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        //aşağıdaki kod ef'nin database'i kendisinin oluşturması
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<Context>();
            context.Database.EnsureCreated();
        }

        app.UseSession();
        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.UseStaticFiles(); // MapStaticAssets yerine

        app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"); 

        app.Run();
    }
}