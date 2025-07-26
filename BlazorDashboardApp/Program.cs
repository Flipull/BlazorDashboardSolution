using BlazorDashboardApp.Components;
using BlazorDashboardApp.Components.Account;
using BlazorDashboardApp.Globals;
using BlazorDashboardApp.Data;
using BlazorDashboardApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlazorDashboardApp.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>//variant would be builder.Services.AddDbContextFactory<>, if ever needed
    options.UseSqlServer(connectionString));

builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<IdentityUser>, IdentityNoOpEmailSender>();


/**/
builder.Services.AddScoped<UserService>();

builder.Services.AddTransient<DatumMapper>();
builder.Services.AddTransient<DatumViewModelMapper>();
builder.Services.AddScoped<DatumService>();

builder.Services.AddTransient<TranscriptMapper>();
builder.Services.AddTransient<TranscriptViewModelMapper>();
builder.Services.AddScoped<TranscriptService>();

builder.Services.AddTransient<TagMapper>();
builder.Services.AddTransient<TagViewModelMapper>();
builder.Services.AddScoped<TagService>();

builder.Services.AddTransient<SubjectMapper>();
builder.Services.AddTransient<SubjectViewModelMapper>();
builder.Services.AddScoped<SubjectService>();
/**/


var app = builder.Build();


/**/Constants.CreatePrerequisiteFileStructure();
/**/await DbInitializer.SeedAdminUserAsync(app.Services);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
