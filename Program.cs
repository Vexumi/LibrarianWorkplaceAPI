using LibrarianWorkplaceAPI;
using LibrarianWorkplaceAPI.Core.Repositories.Interfaces;
using LibrarianWorkplaceAPI.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using LibrarianWorkplaceAPI.Models.PatchModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using LibrarianWorkplaceAPI.Core.Auth;
using System.Xml.Linq;
using LibrarianWorkplaceAPI.Core.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new PatchRequestContractResolver();
});

builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtOptions =>
{
    var key = builder.Configuration.GetValue<string>("JwtConfig:Key");
    var keyBytes = Encoding.ASCII.GetBytes(key);
    jwtOptions.SaveToken = true;
    jwtOptions.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));

#region Repositories
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IBooksRepository, BooksRepository>();
builder.Services.AddTransient<IReadersRepository, ReadersRepository>();
builder.Services.AddTransient<ILibraryDbUnit, LibraryDbUnit>();
builder.Services.AddTransient<IUsersDbUnit, UsersDbUnit>();
#endregion

builder.Services.AddTransient<ITokenManager, JwtTokenManager>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    var xmlfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlfile);
    c.IncludeXmlComments(xmlPath);

    c.OperationFilter<BearerTokenHeaderParameter>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
