using LibrarianWorkplaceAPI;
using LibrarianWorkplaceAPI.Interfaces;
using LibrarianWorkplaceAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Получаем строку подключения
string connection = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddControllers();

//var options = new DbContextOptionsBuilder<ApplicationContext>().UseNpgsql(connection);
//builder.Services.AddDbContext<LibrarianWorkplaceService>(options => options.UseNpgsql(connection));
//builder.Services.AddScoped<ILibrarianWorkplaceService, LibrarianWorkplaceService>();

builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));

#region Repositories
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IBooksRepository, BooksRepository>();
builder.Services.AddTransient<IReadersRepository, ReadersRepository>();
builder.Services.AddTransient<ILibraryDbUnit, LibraryDbUnit>();
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
