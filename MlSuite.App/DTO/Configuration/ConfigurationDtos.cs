namespace MlSuite.App.DTO.Configuration;

public class EmailSettings
{
    public EmailSettings(string emailFrom, string smtpHost, int smtpPort, string smtpUser, string smtpPass)
    {
        EmailFrom = emailFrom;
        SmtpHost = smtpHost;
        SmtpPort = smtpPort;
        SmtpUser = smtpUser;
        SmtpPass = smtpPass;
    }

    public string EmailFrom { get; set; }
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUser { get; set; }
    public string SmtpPass { get; set; }
}

public class AppSettings
{
    public string PostgresSqlPassword { get; set; }
    public string JwtSecret { get; set; }
    public int RefreshTokenTtl { get; set; }
}