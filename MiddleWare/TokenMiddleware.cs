using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.Models;
using MARKETPLACEAPI.Services;

namespace MARKETPLACEAPI.MiddleWare;


public class TokenMiddleware 
{
  private readonly RequestDelegate _next;
  private readonly IAuthService _authService;


  public TokenMiddleware(RequestDelegate next, AuthService authService)
  {
    _next = next;
    _authService = authService;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    
    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

    if (token != null)
    {
      var userId = _authService.DecodeToken(token);
     

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
