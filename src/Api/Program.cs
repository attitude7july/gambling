using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using gambling.Api.Filters;
using gambling.Api.Services;
using gambling.Application;
using gambling.Application.Common.Interfaces;
using gambling.Infrastructure;
using gambling.Infrastructure.Context;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddAntiforgery(options =>
{
    options.FormFieldName = "AntiforgeryFieldname";
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "XSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = true;
});
builder.Services.AddTransient<AntiforgeryCookieResultFilterAttribute>();
builder.Services.AddMemoryCache();
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
builder
.WithOrigins(configuration.GetValue<string>("Client_Url").Split(","))
.AllowAnyHeader()
.AllowAnyMethod()
));
builder.Services.AddControllers(options =>
{

    options.Filters.AddService<AntiforgeryCookieResultFilterAttribute>();
})
.AddJsonOptions(ops =>
{
    ops.JsonSerializerOptions.WriteIndented = true;
    ops.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    ops.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    ops.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(configure =>
{
    configure.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    configure.SwaggerDoc("v1", new OpenApiInfo { Title = "Gambling Api", Version = "v1" });
    configure.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    configure.AddSecurityRequirement(new OpenApiSecurityRequirement
                         {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                Array.Empty<string>()
                            }
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddControllersWithViews(options =>
    options.Filters.Add<ApiExceptionFilterAttribute>())
        .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

builder.Services.AddRazorPages();

// Customise default API behaviour
builder.Services.Configure<ApiBehaviorOptions>(options =>
    options.SuppressModelStateInvalidFilter = true);



var app = builder.Build();
Console.WriteLine(app.Environment.EnvironmentName);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseHsts();
}
app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");

var antiforgery = app.Services.GetRequiredService<IAntiforgery>();
app.Use((context, next) =>
{
    string path = context?.Request?.Path.Value;
    if (path != null && path.ToLower().Contains("/api"))
    {
        var tokenSet = antiforgery.GetAndStoreTokens(context);
        context?.Response.Cookies.Append("XSRF-TOKEN", tokenSet.RequestToken!,
            new CookieOptions { HttpOnly = false });
    }

    return next(context);
});

app.MapControllers();


app.Run();
