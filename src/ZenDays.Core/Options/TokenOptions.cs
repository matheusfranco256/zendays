using System.ComponentModel.DataAnnotations;

namespace ZenDays.Core.Options
{
    public class TokenOptions
    {
        public const string TokenSettingsSection = "TokenSettings";

        [Required]
        public string Key { get; set; } = null!;

        public string? Audience { get; set; }
        public string? Issuer { get; set; }

        public int Seconds { get; set; }
    }
}
