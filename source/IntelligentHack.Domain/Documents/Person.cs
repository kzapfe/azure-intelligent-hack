﻿using Newtonsoft.Json;

namespace IntelligentHack.Domain
{
    public class Person
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("reportedby")]
        public string ReportedBy { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("locationoflost")]
        public string LocationOfLost { get; set; }

        [JsonProperty("dateoflost")]
        public string DateOfLost { get; set; }

        [JsonProperty("reportid")]
        public string ReportId { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }

        [JsonProperty("complexion")]
        public string Complexion { get; set; }

        [JsonProperty("skin")]
        public string Skin { get; set; }

        [JsonProperty("front")]
        public string Front { get; set; }

        [JsonProperty("mouth")]
        public string Mouth { get; set; }

        [JsonProperty("eyebrows")]
        public string Eyebrows { get; set; }

        [JsonProperty("age")]
        public string Age { get; set; }

        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("face")]
        public string Face { get; set; }

        [JsonProperty("nose")]
        public string Nose { get; set; }

        [JsonProperty("lips")]
        public string Lips { get; set; }

        [JsonProperty("chin")]
        public string Chin { get; set; }

        [JsonProperty("typecoloreyes")]
        public string TypeColorEyes { get; set; }

        [JsonProperty("typecolorhair")]
        public string TypeColorHair { get; set; }

        [JsonProperty("particularsigns")]
        public string ParticularSigns { get; set; }

        [JsonProperty("clothes")]
        public string Clothes { get; set; }

        [JsonProperty("isactive")]
        public int IsActive { get; set; }

        [JsonProperty("isfound")]
        public int IsFound { get; set; }

        [JsonProperty("faceapifaceid")]
        public string FaceAPIFaceId { get; set; }

        [JsonProperty("faceapipersonid")]
        public string FaceAPIPersonId { get; set; }
    }
}