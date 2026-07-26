namespace Tolarian.Copyshop.Business.Models.Enums
{
    /// <summary>
    /// Specifies the available card image formats.
    /// https://scryfall.com/docs/api/images
    /// </summary>
    public enum CardImageTypes
    {
        /// <summary>
        /// Transparent, rounded full card PNG (744×1040).
        /// Best image for videos or other high-quality content.
        /// </summary>
        Png,

        /// <summary>
        /// Small full card image (146×204 JPG).
        /// Designed for use as a thumbnail or list icon.
        /// </summary>
        Small,

        /// <summary>
        /// Medium-sized full card image (488×680 JPG).
        /// </summary>
        Normal,

        /// <summary>
        /// Large full card image (672×936 JPG).
        /// </summary>
        Large,

        /// <summary>
        /// Full card image with rounded corners and most of the border cropped off (480×680 JPG).
        /// Designed for contexts where rounded images cannot be used.
        /// </summary>
        Border_Crop,

        /// <summary>
        /// Rectangular crop of the card artwork only (JPG).
        /// Not guaranteed to be perfect for cards with unusual designs or frame layouts.
        /// </summary>
        Art_Crop,

        /// <summary>
        /// Small thumbnail of the card image (146×204 WEBP).
        /// Replaces <see cref="Small"/>.
        /// </summary>
        Thumb,

        /// <summary>
        /// Medium-sized full card image (488×680 WEBP).
        /// Replaces <see cref="Normal"/>.
        /// </summary>
        Grid,

        /// <summary>
        /// Large full card image (672×936 WEBP).
        /// Replaces <see cref="Large"/>.
        /// </summary>
        Display,

        /// <summary>
        /// Full card image with rounded corners and most of the border cropped off (480×680 WEBP).
        /// Replaces <see cref="Border_Crop"/>.
        /// </summary>
        Crop,

        /// <summary>
        /// Rectangular crop of the card artwork only (626×457 WEBP).
        /// Replaces <see cref="Art_Crop"/>.
        /// </summary>
        Art
    }
}