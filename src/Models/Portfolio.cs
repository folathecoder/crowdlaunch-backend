using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MARKETPLACEAPI.Models;

public class Portfolio : DefaultModel
{

  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? portfolioId { get; set; }
  public string? userId { get; set; }
  public string? projectId { get; set; }
  public Status? status { get; set; }

  public DateTime? investmentDate { get; set; } = DateTime.UtcNow;
  public double? amountInvested { get; set; }

}