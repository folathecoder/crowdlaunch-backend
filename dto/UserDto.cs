using MARKETPLACEAPI.Models;

namespace MARKETPLACEAPI.dto;
public class UserDto : DefaultModel {
    public User? user { get; set; }
    public IEnumerable<Portfolio>? portfolios { get; set; }
    public IEnumerable<Project>? listedProjects { get; set; }
    public IEnumerable<UserNft>? ownedNfts { get; set; }
    public IEnumerable<ProjectLike>? projectWatchlist { get; set; }
    public IEnumerable<NftLike>? nftWatchlist { get; set; }
}


public class SignInRegisterDto {
    public string userName { get; set; } = null!;
    public string userProfileImage { get; set; } = null!;
    public string walletAddress { get; set; } = null!;
    public Socials socials { get; set; } = null!;
}

public class SignInResponseDto {
    public string? walletAddress { get; set; }
    public bool? accountCreated { get; set; }
    public bool? accountSignedIn { get; set; }
    public bool? accountExists { get; set; }
    public bool? invalidAddress { get; set; } 
    public string? errorMessage { get; set; }
    public string? token { get; set; }
}

public class LoginDto {
    public string? walletAddress { get; set; }
}

public class UserUpdateDto {
    public string? userName { get; set; }
    public Socials? socials { get; set; }
    public string? userProfileImage { get; set; }
    public DateTime? updatedAt { get; set; } = DateTime.UtcNow;
}