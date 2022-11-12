using HealthEquity.Assessment.Application;
using HealthEquity.Assessment.Infrastructure;
using MediatR;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(typeof(Program).Assembly);

builder.Services.AddRazorPages().AddNToastNotifyToastr(new ToastrOptions()
{
    ProgressBar = true,
    CloseButton = true,
    TimeOut = 5000,
    PositionClass = ToastPositions.BottomRight
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

app.Services.SeedInfrastructureData();

app.UseExceptionHandler("/Error");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseNToastNotify();

app.MapRazorPages();

app.Run();
