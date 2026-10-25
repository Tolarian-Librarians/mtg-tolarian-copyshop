using System;

using Newtonsoft.Json;

namespace Tolarian.Copyshop.Business.Models.SfCardInfo
{
    /// <summary>
    /// The card image formats the app uses. See https://scryfall.com/docs/api/images.
    /// This is a class instead of a dictionary keyed by an enum, so image formats added by Scryfall are ignored instead of failing deserialization.
    /// </summary>
    public class SfImageUris
    {
        /// <summary>
        /// Small thumbnail of the card image (146×204 WEBP).
        /// </summary>
        [JsonProperty(PropertyName = "thumb")]
        public Uri Thumb { get; set; }

        /// <summary>
        /// Medium-sized full card image (488×680 WEBP).
        /// </summary>
        [JsonProperty(PropertyName = "grid")]
        public Uri Grid { get; set; }

        /// <summary>
        /// Large full card image (672×936 WEBP).
        /// </summary>
        [JsonProperty(PropertyName = "display")]
        public Uri Display { get; set; }

        /// <summary>
        /// Full card image with rounded corners and most of the border cropped off (480×680 WEBP).
        /// </summary>
        [JsonProperty(PropertyName = "crop")]
        public Uri Crop { get; set; }
    }
}
