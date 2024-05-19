using AutoMapper;
using BloodBank.Data.DataAccess;
using BloodBank.Service.Cores;
using BloodBank.Service.Utils.Authentication;
using BloodBank.Service.Utils.Mapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BloodBankContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
IMapper mapper = MappingConfig.RegisterMap().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddCustomJwtAuthentication();
builder.Services.AddTransient<JwtTokenHandler>();
builder .Services.AddTransient<IHospitalService, HospitalService>();
builder.Services.AddTransient<IActivityService, ActivityService>();
builder.Services.AddTransient<IDonorService, DonorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
