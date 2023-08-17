using MARKETPLACEAPI.Models;
namespace MARKETPLACEAPI.Interfaces;


public interface IProjectService : IDefaultService<Project> {
  Task<Project?> GetProjectByWalletAddress(string walletAddress);

  Task<IList<Project>> GetProjectsByUserId(string userId);
  Task<IList<Project>> SearchByProjectName(string projectName, bool? ascending = true);

  Task<IList<Project>> GetProjectWithFilters(string? search, bool? newest, bool? trending, Status? active, 
  bool? mostLiked, List<string?> categoryIds, double? minInvestmentMin, 
   double? minInvestmentMax, bool? minInvestmentAsc,
  double? amountRaisedMin, double? amountRaisedMax, bool? amountRaisedAsc, double? targetAmountMin,
  double? targetAmountMax, bool? targetAmountAsc, 
  int? noOfDaysLeftMin, int? noOfDaysLeftMax, bool? noOfDaysLeftAsc, bool? ascending = true);
}
