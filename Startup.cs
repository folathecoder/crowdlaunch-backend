using MARKETPLACEAPI.MiddleWare;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MARKETPLACEAPI.Models;
using MARKETPLACEAPI.Services;
using Microsoft.OpenApi.Models;


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
          services.AddSingleton<UserService>();
          services.AddSingleton<ProjectService>();
          services.AddSingleton<NftService>();
          services.AddSingleton<UserNftService>();
          services.AddSingleton<ProjectLikeService>();
          services.AddSingleton<NftLikeService>();
          services.AddSingleton<PortfolioService>();
          services.AddSingleton<ProjectDetailService>();
          services.AddSingleton<ProjectUpdateService>();
          services.AddSingleton<CategoryService>();
          services.AddSingleton<AuthService>();
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
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
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
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseCors("CorsPolicy");
        app.UseTokenMiddleware();
        // Add the ExceptionLoggingMiddleware to the pipeline.
        string logFilePath = Path.Combine(env.ContentRootPath, "exceptions.log");
        app.UseMiddleware<ExceptionLoggingMiddleware>(logFilePath);


        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        // The following middleware will only execute if an exception is not thrown.
        // You can continue adding other middleware to the pipeline here.
    }
}
