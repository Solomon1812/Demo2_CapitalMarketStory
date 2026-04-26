using Demo2_CapitalMarketStory.Data;
using Demo2_CapitalMarketStory.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

//CONTEXTUL DE DATE (Demo2_CapitalMarketStoryContext) --pt clase
builder.Services.AddDbContext<Demo2_CapitalMarketStoryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Demo2_CapitalMarketStoryContext")
    ?? throw new InvalidOperationException("Connection string 'Demo2_CapitalMarketStoryContext' not found.")));

//CONTEXTUL DE LOGIN (LicentaIdentityContext) --pt useri
builder.Services.AddDbContext<LicentaIdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Demo2_CapitalMarketStoryContext")
    ?? throw new InvalidOperationException("Connection string 'Demo2_CapitalMarketStoryContext' not found.")));

//ACTIVAREA IDENTITY USERS
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<LicentaIdentityContext>();

//SERVICII CUSTOM 
builder.Services.AddScoped<ICalculatorService, CalculatorService>();
builder.Services.AddScoped<IFinancialAnalysisService, FinancialAnalysisService>();

//SECURITATE PAGINI
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    policy.RequireRole("Admin"));
});


builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Companies");
    options.Conventions.AuthorizeFolder("/Imports");
    options.Conventions.AuthorizePage("/Dashboard");


    options.Conventions.AllowAnonymousToPage("/Index");

});


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
