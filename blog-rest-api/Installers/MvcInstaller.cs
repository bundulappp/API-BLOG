using blog_rest_api.Filters;
using blog_rest_api.MappingProfiles;
using blog_rest_api.Options;
using blog_rest_api.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
            builder.Services.AddAutoMapper(typeof(DomainToResponseProfile));
            builder.Services.AddScoped<IIdentityService, IdentityService>();
            builder.Services.AddScoped<IBlogService, BlogService>();
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
        }
    }
}
