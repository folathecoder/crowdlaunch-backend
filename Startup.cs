using MARKETPLACEAPI.MiddleWare;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MARKETPLACEAPI.Models;
using MARKETPLACEAPI.Services;
using Microsoft.OpenApi.Models;
using MARKETPLACEAPI.Interfaces;
using System.Text.Json.Serialization;
using MARKETPLACEAPI.Helpers;
using Microsoft.Extensions.Options;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add your services configuration here if needed.
        services.Configure<MarketPlaceDBSettings>(
            _configuration.GetSection("MarketPlaceDatabase"));

        services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        // var emailConfig = _configuration
        //     .GetSection("EmailConfiguration")
        //     .Get<EmailConfiguration>();
        // services.AddSingleton(emailConfig);
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<INftService, NftService>();
        services.AddScoped<IUserNftService, UserNftService>();
        services.AddScoped<IProjectLikeService, ProjectLikeService>();
        services.AddScoped<INftLikeService, NftLikeService>();
        services.AddScoped<IPortfolioService, PortfolioService>();
        services.AddScoped<IProjectDetailService, ProjectDetailService>();
        services.AddScoped<IProjectUpdateService, ProjectUpdateService>();
        services.AddScoped<IDefaultService<Category>, CategoryService>();
        services.AddScoped<IAuthService, AuthService>();
          services.AddControllers();
          // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
          services.AddEndpointsApiExplorer();
          services.AddSwaggerGen(
              options =>
              {
                  options.SwaggerDoc("v1", new() { Title = "CROWDLAUNCH", Version = "v1" });
                  options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                      In = ParameterLocation.Header,
                      Description = "Enter a vailid token",
                      Name = "Authorization",
                      Type = SecuritySchemeType.Http,
                      BearerFormat = "JWT",
                      Scheme = "Bearer"
                  });
                  options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                      {
                          new OpenApiSecurityScheme {
                              Reference = new OpenApiReference {
                                  Id = "Bearer",
                                  Type = ReferenceType.SecurityScheme
                              }
                          },
                          new string[] {}
                      }
                  });
              }
          );
          services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
          services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,

                      ValidIssuer = _configuration["Jwt:Issuer"],
                      ValidAudience = _configuration["Jwt:Issuer"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!))
                  };
              }
          );

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials()
            );
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Other middleware and configurations...
        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHsts();


    


        app.UseHttpsRedirection();

        // using Microsoft.AspNetCore.HttpOverrides;

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });


        

        
        app.UseTokenMiddleware();
        

        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        // Add the ExceptionLoggingMiddleware to the pipeline.
        string logFilePath = Path.Combine(env.ContentRootPath, "exceptions.log");
        app.UseMiddleware<ExceptionLoggingMiddleware>(logFilePath);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        // The following middleware will only execute if an exception is not thrown.
        // You can continue adding other middleware to the pipeline here.
    }
}
