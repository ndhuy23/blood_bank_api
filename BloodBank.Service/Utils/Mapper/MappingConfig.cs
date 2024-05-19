using AutoMapper;
using BloodBank.Data.Dtos.Activity;
using BloodBank.Data.Dtos.Donor;
using BloodBank.Data.Dtos.Hospital;
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

                config.CreateMap<HospitalDto, Hospital>()
                .ForMember(h => h.Username,opt => opt.MapFrom(dto => dto.Username))
                .ForMember(h => h.Avatar, opt => opt.MapFrom(dto => dto.Avatar))
                .ForMember(h => h.Password, opt => opt.MapFrom(dto => dto.Password))
                .ForMember(h => h.Address, opt => opt.MapFrom(dto => dto.Address))
                .ForMember(h => h.FullName, opt => opt.MapFrom(dto => dto.FullName))
                .ReverseMap();

                config.CreateMap<DonorDto, Donor>()
                .ForMember(h => h.Phone, opt => opt.MapFrom(dto => dto.Phone))
                .ForMember(h => h.Avarta, opt => opt.MapFrom(dto => dto.Avarta))
                .ForMember(h => h.Username, opt => opt.MapFrom(dto => dto.Username))
                .ForMember(h => h.FullName, opt => opt.MapFrom(dto => dto.FullName))
                .ForMember(h => h.Password, opt => opt.MapFrom(dto => dto.Password))
                .ForMember(h => h.BloodType, opt => opt.MapFrom(dto => dto.BloodType))
                .ReverseMap();
            });
            return mapperConfig;
        }
    }
}
