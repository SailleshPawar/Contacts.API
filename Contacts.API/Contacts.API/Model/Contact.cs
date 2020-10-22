using Newtonsoft.Json;

namespace Contacts_.API.Model
{
    public class Contact
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }


    }
}
