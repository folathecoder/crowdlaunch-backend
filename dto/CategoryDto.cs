namespace MARKETPLACEAPI.dto;

public class CategoryCreateDto {
    public string categoryName { get; set; } = null!;
    public string categoryDescription { get; set; } = null!;
}

public class CategoryUpdateDto {
    public string? categoryName { get; set; }
    public string? categoryDescription { get; set; }
    public DateTime? updatedAt { get; set; } = DateTime.UtcNow;
}