
using Consumer_API_BFF.IServices;
using Consumer_API_BFF.Models;
using Consumer_API_BFF.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Consumer_API_BFF
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpClient();
            builder.Services.AddSwaggerGen(options => {
                options.EnableAnnotations();
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "CF Gateway API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."

                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                         new string[] {}
                    }
                });
            });
            var authConfig = builder.Configuration.GetSection("Security").Get<CFAuthenticationConfig>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.MapInboundClaims = false;
                    o.Authority = authConfig.Authority;
                    o.RequireHttpsMetadata = authConfig.RequireHttpsMetadata;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidTypes = new[] { authConfig.ValidTokenTypes },
                        ValidateAudience = false
                    };
                });
            builder.Services.AddAuthorization();
            builder.Services.AddScoped<IGetAuthTokenService, GetAuthTokenService>();
            builder.Services.AddScoped<IGetJobIdForGetFieldsService, GetJobIdForGetFieldsService>();
            builder.Services.AddScoped<IPollFieldsService, PollFieldsService>();
            builder.Services.AddScoped<IGetJobIdForContentTypeGroupsService, GetJobIdForContentTypeGroupsService>();
            builder.Services.AddScoped<IPollContentTypeGroupsService, PollContentTypeGroupsService>();
            builder.Services.AddScoped<IConnectionsService, ConnectionsService>();
            builder.Services.AddScoped<IGetJobIdForContentTypeGroupIdService, GetJobIdForContentTypeGroupIdService>();
            builder.Services.AddScoped<IPollContentTypeGroupsIdService, PollContentTypeGroupsIdService>();
            builder.Services.AddScoped<IGetJobIdForContentTypeService, GetJobIdForContentTypeService>();
            builder.Services.AddScoped<IPollContentTypesService, PollContentTypesService>();
            builder.Services.AddScoped<IGetJobIdForDataObjectQueriesService, GetJobIdForDataObjectQueriesService>();
            builder.Services.AddScoped<IPollDataObjectQueriesService, PollDataObjectQueriesService>();
            builder.Services.AddScoped<IGetJobIdForDataObjectQueriesByIdService, GetJobIdForDataObjectQueriesByIdService>();
            builder.Services.AddScoped<IPollDataObjectQueriesByIdService, PollDataObjectQueriesByIdService>();
            builder.Services.AddScoped<IGetJobIdForExecuteDataObjectQueriesService, GetJobIdForExecuteDataObjectQueriesService>();
            builder.Services.AddScoped<IPollExecuteObjectDataQueriesService, PollExecuteObjectDataQueriesService>();
            builder.Services.AddScoped<IGetJobIdForStandardSearchService, GetJobIdForStandardSearchService>();
            builder.Services.AddScoped<IPollStandardSearchService, PollStandardSearchService>();
            builder.Services.AddScoped<IIntegrationService, IntegrationService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular",
                    policy =>
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CF Gateway API");
            });
            app.MapControllers();
            
            app.Run();
        }
    }
}
