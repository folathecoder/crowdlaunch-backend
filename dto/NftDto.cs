using MARKETPLACEAPI.Models;

namespace MARKETPLACEAPI.dto;


public class NftCreateDto {
  public string nftName { get; set; } = null!;
  public string nftDescription { get; set; } = null!;
  public double price { get; set; } 
  public string categoryId { get; set; } = null!;

}

public class NftUpdateDto {
  public string? nftName { get; set; }
  public string? nftDescription { get; set; }
  public double? price { get; set; }
  public DateTime? updatedAt { get; set; } = DateTime.UtcNow;
}


public class NftDto {
  public Nft nft { get; set; } = null!;
  public Category category { get; set; } = null!;
}

public class UpdateNftLikeDto {
  public double? noOfLikes { get; set; }
}

public class BuyNftDto {
  public string ownerId { get; set; } = null!;
}