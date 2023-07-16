using LibrarianWorkplaceAPI;
using LibrarianWorkplaceAPI.Interfaces;
using LibrarianWorkplaceAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using LibrarianWorkplaceAPI.Models.PatchModels;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new PatchRequestContractResolver();
});

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));

#region Repositories
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IBooksRepository, BooksRepository>();
builder.Services.AddTransient<IReadersRepository, ReadersRepository>();
builder.Services.AddTransient<ILibraryDbUnit, LibraryDbUnit>();
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlfile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
