namespace MARKETPLACEAPI.Models;

public enum Status {
    Completed,
    Failed,
  }


public class DefaultModel
{
  public DateTime? createdAt { get; set; } = DateTime.UtcNow;
  public DateTime? updatedAt { get; set; } = DateTime.UtcNow;
}
