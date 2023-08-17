using MARKETPLACEAPI.dto;
using MARKETPLACEAPI.Models;
namespace MARKETPLACEAPI.Interfaces;


public interface IAuthService {
  Task<SignInResponseDto> Authenticate(LoginDto loginDto);
}