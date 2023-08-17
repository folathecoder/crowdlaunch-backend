using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.Helpers;

namespace MARKETPLACEAPI.MiddleWare;


public class TokenMiddleware 
{
  private readonly RequestDelegate _next;
  private readonly IConfiguration _config;


  public TokenMiddleware(RequestDelegate next, IConfiguration config)
  {
    _next = next;
    _config = config;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    
    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

    if (token != null)
    {
      TokenHelper _tokenHelper = new TokenHelper(_config);

      var userId = _tokenHelper.DecodeToken(token);
     

      if (userId != null)
      {
        context.Request.Headers.Add("userId", userId);
      }
    }

    await _next(context);
  }
}


public static class TokenMiddlewareExtensions
{
  public static IApplicationBuilder UseTokenMiddleware(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<TokenMiddleware>();
  }
}
