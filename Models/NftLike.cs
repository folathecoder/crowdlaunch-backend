using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MARKETPLACEAPI.Models;

public class NftLike : DefaultModel
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? nftLikeId { get; set; }
  public string? userId { get; set; }
  public string? nftId { get; set; }

}