using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Application.Configuration;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Application.Interfaces.ImageServices;
using ParsaLibraryManagement.Application.Mappings;
using ParsaLibraryManagement.Application.Services;
using ParsaLibraryManagement.Application.Validators;
using ParsaLibraryManagement.Domain.Interfaces;
using ParsaLibraryManagement.Domain.Interfaces.ImageServices;
using ParsaLibraryManagement.Domain.Interfaces.Repository;
using ParsaLibraryManagement.Infrastructure.Data.Contexts;
using ParsaLibraryManagement.Infrastructure.Data.Repositories;
using ParsaLibraryManagement.Infrastructure.Services.ImageServices;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Logger

//Serilog Logger
var logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

#endregion

#region Services

// Add services to the container.
builder.Services.AddControllersWithViews();

#region DataBase Context

//Add dbContext services 
builder.Services.AddDbContext<ParsaLibraryManagementDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ParsaLibraryManagementSQLServerConnection")));

#endregion

#region IOC

#region Core

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BookCategoryValidator>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<GenderValidator>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PublisherValidator>());//todo add others


#endregion

#region Services

//todo categorized and separate to new files

var fileUploadOptions = builder.Configuration.GetSection("FileUploadOptions").Get<FileUploadOptions>(); //todo refactor
builder.Services.AddSingleton(fileUploadOptions);

// Application services
builder.Services.AddScoped<ImageFileValidationServices>();


// Domain interfaces
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
builder.Services.AddTransient<IBooksCategoryRepository, BooksCategoryRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddTransient<IImageServices, ImageServices>();



// Application interfaces
builder.Services.AddTransient<IBookCategoryServices, BookCategoryServices>();
builder.Services.AddScoped<IPublisherServices, PublisherServices>();
builder.Services.AddTransient<IImageFileValidationService, ImageFileValidationServices>();


builder.Services.AddScoped<IGenderService, GenderServices>();


#endregion

#endregion

#endregion

var app = builder.Build();

#region Development environment

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //Unexpected error
    app.UseExceptionHandler("/Error");

    //Handle status codes like 404
    app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");

    //Security mechanism that protect websites against protocol downgrade attacks and cookie hijacking.
    app.UseHsts();
}

#endregion

#region Production environment

if (!app.Environment.IsDevelopment())
{
    //Unexpected error
    app.UseExceptionHandler("/Error");

    //Handle status codes like 404
    app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");

    //Security mechanism that protect websites against protocol downgrade attacks and cookie hijacking.
    app.UseHsts();
}

#endregion

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
