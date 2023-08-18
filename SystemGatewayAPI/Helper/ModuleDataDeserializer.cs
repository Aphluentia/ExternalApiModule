using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace SystemGatewayAPI.Helper
{
    public class ModuleDataDeserializer
    {
        //var jsonObject = JsonConvert.DeserializeObject(requestString);
        //var apiRequest = ToApiRequest(jsonObject);
        private static object ToApiRequest(object requestObject)
        {
            switch (requestObject)
            {
                case JObject jObject: // objects become Dictionary<string,object>
                    return ((IEnumerable<KeyValuePair<string, JToken>>)jObject).ToDictionary(j => j.Key, j => ToApiRequest(j.Value));
                case JArray jArray: // arrays become List<object>
                    return jArray.Select(ToApiRequest).ToList();
                case JValue jValue: // values just become the value
                    return jValue.Value;
                default: // don't know what to do here
                    throw new Exception($"Unsupported type: {requestObject.GetType()}");
            }

        }
    }
}
