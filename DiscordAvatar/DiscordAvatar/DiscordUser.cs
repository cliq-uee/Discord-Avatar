using Newtonsoft.Json;

namespace DiscordAvatar
{
    internal class DiscordUser
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }

        [JsonProperty("avatar")]
        public string AvatarHash { get; set; }
    }
}
