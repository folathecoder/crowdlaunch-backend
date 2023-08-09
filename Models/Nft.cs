using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MARKETPLACEAPI.Models;

public class Nft : DefaultModel
{

  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? nftId { get; set; }
  public string? creatorId { get; set; }
  public string? nftName { get; set; }
  public string? nftDescription { get; set; }
  public double? price { get; set; }
  public int? noOfLikes { get; set; } = 0;
  public string? ownerId { get; set; }
  public string? categoryId { get; set; }
}