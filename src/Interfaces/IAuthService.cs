using MARKETPLACEAPI.dto;
using MARKETPLACEAPI.Models;
namespace MARKETPLACEAPI.Interfaces;


public interface IAuthService {
  string GenerateToken(User user);
  Task<SignInResponseDto> Authenticate(LoginDto loginDto);
  string DecodeToken(string token);
}