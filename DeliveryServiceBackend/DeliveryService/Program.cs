using AutoMapper;
using DeliveryService.Data;
using DeliveryService.Mapping;
using DeliveryService.Services;
using DeliveryService.Services.Impl;
using DeliveryService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeliveryService", Version = "v1" });
  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    In = ParameterLocation.Header,
    Description = "Please enter token",
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    BearerFormat = "JWT",
    Scheme = "bearer"
  });
  c.AddSecurityRequirement(new OpenApiSecurityRequirement
  {
      {
          new OpenApiSecurityScheme
          {
              Reference = new OpenApiReference
              {
                  Type=ReferenceType.SecurityScheme,
                  Id="Bearer"
              }
          },
          new string[]{}
      }
  });
});

//Configure DB
builder.Services.AddDbContext<DeliveryDataContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors(options => options.AddPolicy(name: "UserOrigins",
  policy=>
  {
    policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
  }));

//Inject services
builder.Services.AddTransient<ITransistentUserService, UserService>();
builder.Services.AddTransient<ITransistentProductService, ProductService>();

//Security
builder.Services.AddAuthentication(opt =>
{
  opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
  options.TokenValidationParameters = new TokenValidationParameters 
  {
    ValidateIssuer = true, 
    ValidateAudience = false, 
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true, 
    ValidIssuer = "http://localhost:44398", //On SSL port
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"]))//navodimo privatni kljuc kojim su potpisani nasi tokeni
  };
});
builder.Services.AddCors(options =>
{
  options.AddPolicy(name: "cors", builder => {
    builder.WithOrigins("https://localhost:4200") //Angular front URL
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials();
  });
});

//Model<=>DTO mapper
var mapperCfg = new MapperConfiguration(mc =>
{
  mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperCfg.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("UserOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
