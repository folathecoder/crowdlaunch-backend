using MARKETPLACEAPI.Models;

namespace MARKETPLACEAPI.dto;


public class PortfolioCreateDto {
  public string projectId { get; set; } = null!;
  public Status status { get; set; }
  public double amountInvested { get; set; }
  public DateTime? investmentDate { get; set; } = DateTime.UtcNow;
}