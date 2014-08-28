using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace push_wp8_sample.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    [DataContract]
    class PushRequest
    {
        [DataMember, JsonProperty(PropertyName = "message", NullValueHandling = NullValueHandling.Ignore)]
        public PushRequestMessage Message { get; set; }

        [DataMember, JsonProperty(PropertyName = "target", NullValueHandling = NullValueHandling.Ignore)]
        public PushRequestTarget Target { get; set; }

        private PushRequest()
        {
        }

        public static PushRequest MakePushRequest(string body, string[] devices, string templateType, string templateName, Dictionary<string, string> templateFields)
        {
            return new PushRequest
            {
                Message = new PushRequestMessage
                {
                    Body = body,
                    Custom = new PushRequestMessageCustom
                    {
                        Windows8 = new PushRequestMessageCustomWindows8
                        {
                            Type = templateType,
                            TemplateName = templateName,
                            TemplateFields = templateFields
                        }
                    }
                },

                Target = new PushRequestTarget
                {
                    Platform = "windows8",
                    Devices = devices
                }
            };
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    [DataContract]
    internal class PushRequestMessage
    {
        [DataMember, JsonProperty(PropertyName = "body", NullValueHandling = NullValueHandling.Ignore)]
        public string Body { get; set; }

        [DataMember, JsonProperty(PropertyName = "custom", NullValueHandling = NullValueHandling.Ignore)]        
        public PushRequestMessageCustom Custom { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    [DataContract]
    internal class PushRequestMessageCustom
    {
        [DataMember, JsonProperty(PropertyName = "windows8", NullValueHandling = NullValueHandling.Ignore)]
        public PushRequestMessageCustomWindows8 Windows8 { get; set; }
    }

    internal class PushRequestMessageCustomWindows8
    {
        [DataMember, JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [DataMember, JsonProperty(PropertyName = "template_name", NullValueHandling = NullValueHandling.Ignore)]
        public string TemplateName { get; set; }

        [DataMember, JsonProperty(PropertyName = "template_fields", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<String, String> TemplateFields { get; set; } 
    }

    [JsonObject(MemberSerialization.OptIn)]
    [DataContract]
    internal class PushRequestTarget
    {
        [DataMember, JsonProperty(PropertyName = "platform", NullValueHandling = NullValueHandling.Ignore)]
        public string Platform { get; set; }
     
        [DataMember, JsonProperty(PropertyName = "devices", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Devices { get; set; }
    }
}
