using AutoMapper;
using Tolarian.Copyshop.Business.Models;
using Tolarian.Copyshop.Controller.ResponseObjects;
using System.Linq;
using Tolarian.Copyshop.Business.Models.Enums;
using System.Collections;
using System.Collections.Generic;
using Tolarian.Copyshop.Controller.Interfaces;

namespace Tolarian.Copyshop.ScreenPresenter.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SfCard, CardNameResponse>();

            CreateMap<SfCard, IFullCard>()
                .ForMember(dest => dest.PngImage, opt => opt.MapFrom(s => s.ImageUris.ContainsKey(CardImageTypes.Large) ? s.ImageUris[CardImageTypes.Large] : null))
                .ForMember(dest => dest.SmallImage, opt => opt.MapFrom(s => s.ImageUris.ContainsKey(CardImageTypes.Small) ? s.ImageUris[CardImageTypes.Small] : null))
                .ForMember(dest => dest.Legalities1, opt => opt.MapFrom(s => GetFirstHalfOfLegalities(s)))
                .ForMember(dest => dest.Legalities2, opt => opt.MapFrom(s => GetSecondHalfOfLegalities(s)))
                .ForMember(dest => dest.CardCount, opt => opt.MapFrom(_ => 1))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(s => GetTextOfCard(s)));

            CreateMap<SfCard, List<IFullCard>>().ConvertUsing(source => source.CardFaces.Select(c => new FullCardResponse
                {
                    Id =  source.Id,
                    Text = c.Text,
                    Name = c.Name,
                    CardType = c.CardType,
                    CardCount = 1,
                    PngImage = c.ImageUris.ContainsKey(CardImageTypes.Large) ? c.ImageUris[CardImageTypes.Large] : null,
                    SmallImage = c.ImageUris.ContainsKey(CardImageTypes.Small) ? c.ImageUris[CardImageTypes.Small] : null,
                    Legalities1 = GetFirstHalfOfLegalities(source),
                    Legalities2 = GetSecondHalfOfLegalities(source)
                }
            ).AsEnumerable().Cast<IFullCard>().ToList());
        }

        private Dictionary<string, string> GetFirstHalfOfLegalities(SfCard source)
        {
            return source.Legalities.Take(GetHalfIndexOfDictionary(source.Legalities)).ToDictionary(k => k.Key.ToString(), e => e.Value.Replace('_', ' '));
        }

        private Dictionary<string, string> GetSecondHalfOfLegalities(SfCard source)
        {
            return source.Legalities.Skip(GetHalfIndexOfDictionary(source.Legalities)).Take(source.Legalities.Count - GetHalfIndexOfDictionary(source.Legalities)).ToDictionary(k => k.Key.ToString(), e => e.Value.Replace('_', ' '));
        }

        private string GetTextOfCard(SfCard source)
        {
            if(string.IsNullOrEmpty(source.Text) && source.CardFaces != null)
            {
                return string.Join(" // ", source.CardFaces.Select(c => c.Text).ToArray());
            }
            else
            {
                return source.Text ?? "";
            }
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