using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Application.Mappings;
using ParsaLibraryManagement.Application.Services;
using ParsaLibraryManagement.Application.Validators;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces.General;
using ParsaLibraryManagement.Domain.Interfaces.Repository;
using ParsaLibraryManagement.Infrastructure.Data.Contexts;
using ParsaLibraryManagement.Infrastructure.Data.Repositories;
using ParsaLibraryManagement.Infrastructure.Services.General;
using ParsaLibraryManagement.Web.ValidationServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Services

#region DataBase Context

//Add dbContext services 
builder.Services.AddDbContext<ParsaLibraryManagementDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ParsaLibraryManagementSQLServerConnection")));

#endregion

#region IOC

#region Core

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BookCategoryValidator>());



#endregion

#region Services

builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
builder.Services.AddScoped<ImageFileValidationService>();
builder.Services.AddTransient<IBookCategoryServices, BookCategoryServices>();
builder.Services.AddTransient<IImageServices, ImageServices>();

#endregion

#endregion

#endregion

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
