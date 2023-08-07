using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MARKETPLACEAPI.Models;

public class Project : DefaultModel
{

  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? projectId { get; set; }
  public string? userId { get; set; }
  public string? categoryId { get; set; }
  public string? projectName { get; set; }
  public string? bannerImageUrl { get; set; }
  public double? targetAmount { get; set; }
  public double? amountRaised { get; set; }
  public double? minInvestment { get; set; }
  public int? noOfInvestors { get; set; } = 0;
  public int? noOfDaysLeft { get; set; }
  public int? noOfLikes { get; set; } = 0;
  public string? projectWalletAddress { get; set; }
  public CustomColour? customColour { get; set; }
  public Status? projectStatus { get; set; }
}