using blog_rest_api.Installers;
using blog_rest_api.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var installers = typeof(Program).Assembly.ExportedTypes.Where(x =>
    typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

installers.ForEach(installer => installer.InstallServices(builder));

var app = builder.Build();

var swaggerOptions = new SwaggerOptions();
app.Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

app.UseSwagger(option =>
{
    option.RouteTemplate = swaggerOptions.JsonRoute;
});

app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
