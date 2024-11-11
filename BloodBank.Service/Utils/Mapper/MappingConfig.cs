using AutoMapper;
using BloodBank.Data.Dtos;
using BloodBank.Data.Dtos.Activity;
using BloodBank.Data.Dtos.Donor;
using BloodBank.Data.Dtos.Hospital;
using BloodBank.Data.Dtos.SessionDonor;
using BloodBank.Data.Dtos.Users;
using BloodBank.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Service.Utils.Mapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ActivityDto, Activity>()
                .ForMember(a =>a.Quantity, opt => opt.MapFrom(dto => dto.Quantity))
                .ForMember(a =>a.DateActivity, opt => opt.MapFrom(dto => dto.DateActivity))
                .ForMember(a => a.OperatingHour, opt => opt.MapFrom(dto => dto.OperatingHour))
                .ReverseMap();

                config.CreateMap<DonorDto, User>()
                .ReverseMap();
                config.CreateMap<SessionDonorDto, SessionDonor>()
               .ForMember(ss => ss.DonorId, opt => opt.MapFrom(dto => dto.DonorId))
               .ForMember(ss => ss.ActivityId, opt => opt.MapFrom(dto => dto.ActivityId))
               .ForMember(ss => ss.Status, opt => opt.MapFrom(dto => dto.Status))
               .ReverseMap();

                config.CreateMap<UpdateSessionDto, SessionDonor>()
               .ForMember(ss => ss.Status, opt => opt.MapFrom(dto => dto.Status))
               .ReverseMap();

                config.CreateMap<BloodDto, Blood>()
               .ForMember(bl => bl.BloodType, opt => opt.MapFrom(dto => dto.BloodType))
               .ForMember(bl => bl.Quantity, opt => opt.MapFrom(dto => dto.Quantity))
               .ForMember(bl => bl.HospitalId, opt => opt.MapFrom(dto => dto.HospitalId))
               .ReverseMap();

                config.CreateMap<HistoryDto, History>()
               .ForMember(ht => ht.Quantity, opt => opt.MapFrom(dto => dto.Quantity))
               .ForMember(ht => ht.DonorId, opt => opt.MapFrom(dto => dto.DonorId))
               .ForMember(ht => ht.HospitalId, opt => opt.MapFrom(dto => dto.HospitalId))
               .ForMember(ht => ht.HospitalName, opt => opt.MapFrom(dto => dto.HospitalName))
               .ReverseMap();

                config.CreateMap<RequestBloodDto, RequestBlood>()
               .ForMember(ht => ht.HospitalId, opt => opt.MapFrom(dto => dto.HospitalId))
               .ForMember(ht => ht.Quantity, opt => opt.MapFrom(dto => dto.Quantity))
               .ForMember(ht => ht.BloodType, opt => opt.MapFrom(dto => dto.BloodType))
               .ReverseMap();

                config.CreateMap<HospitalDto, User>()
                .ReverseMap();
            });
            return mapperConfig;
        }
    }
}
