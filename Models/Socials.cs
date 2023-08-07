namespace MARKETPLACEAPI.Models;

public class Socials
{
  public string? websiteUrl { get; set; }
  public string? twitterUrl { get; set; }
  public string? telegramUrl { get; set; }
  public string? discordUrl { get; set; }

  public Socials(
    string? websiteUrl,
    string? twitterUrl,
    string? telegramUrl,
    string? discordUrl
  )
  {
    this.websiteUrl = websiteUrl;
    this.twitterUrl = twitterUrl;
    this.telegramUrl = telegramUrl;
    this.discordUrl = discordUrl;
  }

}
