using GeoFence_VehicleNonAvailability.Data;
using GeoFence_VehicleNonAvailability.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<GeoFencePeriodDBContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("GeoFenceConnectionString")));

builder.Services.AddDbContext<VehicleNonAvailDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("GeoFenceConnectionString")));

builder.Services.AddScoped<VehicleNonAvailDBContext>();

builder.Services.AddScoped<VehicleNotAvailRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
