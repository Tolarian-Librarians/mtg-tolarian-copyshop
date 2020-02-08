using AutoMapper;
using Tolarian.Copyshop.Business.Models;
using Tolarian.Copyshop.Controller.ResponseObjects;
using System.Linq;
using Tolarian.Copyshop.Business.Models.Enums;

namespace Tolarian.Copyshop.ScreenPresenter.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SfCard, FullCardResponse>().ForMember(dest => dest.PngImage, opt => opt.MapFrom(d => d.ImageUris.ContainsKey(CardImageTypes.Png) ? d.ImageUris[Business.Models.Enums.CardImageTypes.Png] : null)).
                ForMember(dest => dest.SmallImage, opt => opt.MapFrom(d => d.ImageUris.ContainsKey(CardImageTypes.Small) ? d.ImageUris[CardImageTypes.Small] : null));  //.ConvertUsing(convert);
        }
    }
}