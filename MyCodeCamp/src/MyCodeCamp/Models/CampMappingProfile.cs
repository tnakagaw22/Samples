using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCodeCamp.Models
{
    public class CampMappingProfile : Profile
    {
        public CampMappingProfile()
        {
            CreateMap<Camp, CampModel>()
                .ForMember(c => c.StartDate,
                    opt => opt.MapFrom(camp => camp.EventDate))
                .ForMember(c => c.EndDate,
                    opt => opt.ResolveUsing(camp => camp.EventDate.AddDays(camp.Length)))
                .ForMember(c => c.Url,
                    opt => opt.ResolveUsing<CampUrlResolver>())
                .ReverseMap()
                .ForMember(m => m.EventDate,
                    opt => opt.MapFrom(model => model.StartDate))
                .ForMember(m => m.Length,
                    opt => opt.ResolveUsing(model => (model.EndDate - model.StartDate).Days))
                .ForMember(m => m.Location,
                    opt => opt.ResolveUsing(camp => new Location()
                    {
                        Address1 = camp.LocationAddress1,
                        Address2 = camp.LocationAddress2,
                        Address3 = camp.LocationAddress3,
                        CityTown = camp.LocationCityTown,
                        StateProvince = camp.LocationStateProvince,
                        PostalCode = camp.LocationPostalCode,
                        Country = camp.LocationCountry
                    }));

            CreateMap<Speaker, SpeakerModel>()
                .ForMember(s => s.Url, opt => opt.ResolveUsing<SpeakerUrlResolver>())
                .ReverseMap();

            CreateMap<Speaker, Speaker2Model>()
                .IncludeBase<Speaker, SpeakerModel>()
                .ForMember(s => s.BadgeName,
                    opt => opt.ResolveUsing(s => $"{s.Name} (@{s.TwitterName})"));

            CreateMap<Talk, TalkModel>()
                .ForMember(s => s.Url, opt => opt.ResolveUsing<TalkUrlResolver>())
                .ReverseMap();
        }
    }
}
