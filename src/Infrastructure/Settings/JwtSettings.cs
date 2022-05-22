namespace gambling.Infrastructure.Settings;

public class JwtAppSettings
{
    public string Secret { get; set; }
    public string ValidAudience { get; set; }
    public string ValidIssuer { get; set; }
    public int SessionMinutes { get; set; } = 20;
}

