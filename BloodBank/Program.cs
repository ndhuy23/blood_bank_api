using AutoMapper;
using Quartz;
using BloodBank.Data.DataAccess;
using BloodBank.Service.Cores;
using BloodBank.Service.Utils.Authentication;
using BloodBank.Service.Utils.BackGround;
using BloodBank.Service.Utils.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EntitiesAPI.Infrastructure.Filters.JsonConverters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true);
builder.Configuration.AddEnvironmentVariables();
var connectionStringsSection = builder.Configuration.GetSection("ConnectionStrings");
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BloodBankContext>(option => {
    option.UseNpgsql(connectionStringsSection["DefaultConnection"]);
});
builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("UpdateActivity");
    q.AddJob<ActivityWorkerJob>(opts => opts.WithIdentity(jobKey));
    q.UseMicrosoftDependencyInjectionJobFactory(options =>
    {
        // if we don't have the job in DI, allow fallback
        // to configure via default constructor
        options.AllowDefaultConstructor = true;
    });
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("UpdateActivity")
        //.WithDailyTimeIntervalSchedule
        //          (s =>
        //             s.WithIntervalInHours(24)
        //            .OnEveryDay()
        //            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
        //          ));

        .WithCronSchedule("0 0 0 * * ?", x => x.InTimeZone(TimeZoneInfo.Local))
        );
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
IMapper mapper = MappingConfig.RegisterMap().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddCustomJwtAuthentication();
builder.Services.AddTransient<JwtTokenHandler>();
builder.Services.AddTransient<IHospitalService, HospitalService>();
builder.Services.AddTransient<IActivityService, ActivityService>();
builder.Services.AddTransient<IDonorService, DonorService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ISessionDonorService, SessionDonorService>();
builder.Services.AddTransient<IBloodService, BloodService>();
builder.Services.AddTransient<IHistoryService, HistorySerivce>();
builder.Services.AddTransient<IRequestBloodService, RequestBloodService>();
builder.Services.AddScoped<IActivityWorker, ActivityWorker>();
builder.Services.AddSingleton<ActivityWorkerJob>();
builder.Services.AddHostedService<ScopedActivityHostedService>();
var app = builder.Build();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
//app.Urls.Add("http://0.0.0.0:7067"); //Ubuntu server
//app.Urls.Add("http://127.0.0.1:7067");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
