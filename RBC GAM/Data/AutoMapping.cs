using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RBC_GAM.Model;
using RBC_GAM.ModelDTO;

namespace RBC_GAM.Data
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.FinInstrumentId, opt => opt.Ignore())
            .ReverseMap();

            CreateMap<Threshold, ThresholdDTO>()
            .ReverseMap()
                .ForMember(dest => dest.FinancialInstrument, opt => opt.Ignore());
            
            CreateMap<Trigger, TriggerDTO>()
                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.Action == Model.Action.Buy ? "Buy" : "Sell"))
                .ForMember(dest => dest.Direction, opt => opt.MapFrom(src => src.Direction == Direction.Above ? "Above" : "Below"))                        
            .ReverseMap()
                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => (Model.Action) Enum.Parse(typeof(Model.Action), src.Action)))
                .ForMember(dest => dest.Direction, opt => opt.MapFrom(src => (Model.Direction) Enum.Parse(typeof(Model.Direction), src.Direction)))
            ;
        }

    }
}
