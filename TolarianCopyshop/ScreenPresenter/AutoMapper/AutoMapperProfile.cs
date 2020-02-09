using AutoMapper;
using Tolarian.Copyshop.Business.Models;
using Tolarian.Copyshop.Controller.ResponseObjects;
using System.Linq;
using Tolarian.Copyshop.Business.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Tolarian.Copyshop.ScreenPresenter.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SfCard, FullCardResponse>().ForMember(dest => dest.PngImage, opt => opt.MapFrom(s => s.ImageUris.ContainsKey(CardImageTypes.Png) ? s.ImageUris[Business.Models.Enums.CardImageTypes.Png] : null))
                .ForMember(dest => dest.SmallImage, opt => opt.MapFrom(s => s.ImageUris.ContainsKey(CardImageTypes.Small) ? s.ImageUris[CardImageTypes.Small] : null))
                .ForMember(dest => dest.Legalities1, opt => opt.MapFrom(s => s.Legalities.Take(GetHalfIndexOfDictionary(s.Legalities)).ToDictionary(k => k.Key, e => e.Value)))
                .ForMember(dest => dest.Legalities2, opt => opt.MapFrom(s => s.Legalities.Skip(GetHalfIndexOfDictionary(s.Legalities)).Take(s.Legalities.Count - GetHalfIndexOfDictionary(s.Legalities)).ToDictionary(k => k.Key, e => e.Value)));//.Take(s.Legalities.Count - GetHalfIndexOfDictionary(s.Legalities)).ToDictionary(k => k.Key)));
        }

        private int GetHalfIndexOfDictionary(ICollection collection)
        {
            int count = collection.Count;
            if (count % 2 == 0)
                return count / 2;
            else
                return count / 2 + 1;
        }
    }
}