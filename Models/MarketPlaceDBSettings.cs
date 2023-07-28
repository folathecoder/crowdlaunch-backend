namespace MARKETPLACEAPI.Models;

public class MarketPlaceDBSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string UserCollectionName { get; set; } = null!;
    public string NftCollectionName { get; set; } = null!;
    public string NftLikeCollectionName { get; set; } = null!;
    public string UserNftCollectionName { get; set; } = null!;
    public string CategoryCollectionName { get; set; } = null!;
    public string ProjectCollectionName { get; set; } = null!;
    public string ProjectLikeCollectionName { get; set; } = null!;
    public string ProjectDetailCollectionName { get; set; } = null!;
    public string ProjectUpdateCollectionName { get; set; } = null!;
    public string PortfolioCollectionName { get; set; } = null!;

}