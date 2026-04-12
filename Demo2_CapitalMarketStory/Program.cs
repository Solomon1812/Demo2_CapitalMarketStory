using Demo2_CapitalMarketStory.Data;
using Demo2_CapitalMarketStory.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<Demo2_CapitalMarketStoryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Demo2_CapitalMarketStoryContext") ?? throw new InvalidOperationException("Connection string 'Demo2_CapitalMarketStoryContext' not found.")));


builder.Services.AddScoped<ICalculatorService, CalculatorService>();
builder.Services.AddScoped<IFinancialAnalysisService, FinancialAnalysisService>();





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
