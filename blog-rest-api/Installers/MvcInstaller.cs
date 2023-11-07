using blog_rest_api.Filters;
using blog_rest_api.MappingProfiles;
using Data.Options;
using FluentValidation;
using FluentValidation.AspNetCore;
using Logic;
using Logic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Models.Interfaces;
using System.Text;

namespace blog_rest_api.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            var jwtSettings = new JwtSettings();
            builder.Configuration.Bind(nameof(JwtSettings), jwtSettings);
            builder.Services.AddSingleton(jwtSettings);
            builder.Services.AddAutoMapper(typeof(DomainToResponseProfile), typeof(RequestToDomainProfile));
            builder.Services.AddScoped<IIdentityService, IdentityService>();

            builder.Services.AddScoped<IBlogRepository, BlogRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<IBlogService, BlogService>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<MvcInstaller>();
            builder.Services.AddMvc(opt =>
            {
                opt.Filters.Add<ValidationFilter>();
            });



            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            builder.Services.AddSingleton(tokenValidationParameters);
            builder.Services.AddAuthentication(confOptions =>
            {
                confOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                confOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                confOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.SaveToken = true;
                    x.TokenValidationParameters = tokenValidationParameters;
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("TagViewer", builder => builder.RequireClaim("tags.view", "true"));
            });

            builder.Services.AddSingleton<IUriService>(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");
                return new UriService(absoluteUri);
            });
        }
    }
}
