using MARKETPLACEAPI.Models;

namespace MARKETPLACEAPI.dto;


public class ProjectCreateDto {
  public string categoryId { get; set; } = null!;
  public string projectName { get; set; } = null!;
  public string bannerImageUrl { get; set; } = null!;
  public double targetAmount { get; set; } 
  public double minInvestment { get; set; }
  public int noOfDaysLeft { get; set; }
  public string projectWalletAddress { get; set; } = null!;
  public CustomColour customColour { get; set; } = null!;
  public Status projectStatus { get; set; }
  public double amountRaised { get; set; }
}

public class UpdateProjectDto {
  public string? categoryId { get; set; }
  public string? projectName { get; set; }
  public string? bannerImageUrl { get; set; }
  public double? targetAmount { get; set; }
  public double? minInvestment { get; set; }
  public int? noOfDaysLeft { get; set; }
  public string? projectWalletAddress { get; set; }
  public CustomColour? customColour { get; set; }
  public Status? projectStatus { get; set; }
  public double? amountRaised { get; set; }
  public DateTime? updatedAt { get; set; } = DateTime.UtcNow;
}


public class ProjectDto 
{
  public Project? project { get; set; }
  public Category? category { get; set; }
  public IEnumerable<ProjectUpdate>? projectUpdates { get; set; }
  public ProjectDetail? projectDetails { get; set; }
}


public class ChangeProjectLikesDto
{
  public int noOfLikes { get; set; }
}