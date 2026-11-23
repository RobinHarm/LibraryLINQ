using Library.ApplicationServices.Services;
using Library.Core.ServiceInterface;
using Library.Data;
using Microsoft.EntityFrameworkCore;


namespace LibraryLINQ
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IBookServices, BookServices>();
            builder.Services.AddScoped<ILoanServices, LoanServices>();
            builder.Services.AddScoped<IReaderServices, ReaderServices>();

            // Andmebaasiühenduse seadistamine.
            // AddDbContext registreerib LibraryContexti rakenduse teenuste hulka.
            // UseSqlServer määrab, et kasutatakse Microsoft SQL Serveri andmebaasi.
            // GetConnectionString loeb ühenduse seaded appsettings.json failist,
            // kasutades ühenduse nime "LibraryContext".

            builder.Services.AddDbContext<LibraryContext>(options =>
                           options.UseSqlServer(
                               builder.Configuration.GetConnectionString("LibraryContext")));
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
