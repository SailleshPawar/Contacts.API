using Newtonsoft.Json;
using System.Collections.Generic;

namespace Contacts_.API.Model
{
    public class UserContact
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }


        [JsonProperty(PropertyName = "userid")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "contacts")]
        public List<Contact> Contacts { get; set; }
    }
}
