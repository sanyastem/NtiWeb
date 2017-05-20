using System;
using System.Linq;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Extentions;
using ASUVP.Core.Web.Dto;
using ASUVP.Core.Web.OData;
using AutoMapper;

namespace ASUVP.Middleware.WebApi
{
    public class DomainToODataModelProfile : Profile
    {
        public DomainToODataModelProfile()
        {
            CreateMap<Role, RoleOData>();

            CreateMap<Permission, PermissionOData>();

            CreateMap<Agreement, AgreementOData>()
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Document.DocNumber))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Document.Name))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Document.DocDate))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.DateBeg ?? default(DateTime)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.DateEnd ?? default(DateTime)))
                .ForMember(dest => dest.CustomerCompany, opt => opt.MapFrom(src => src.Document.CustomerCompany.ShortName))
                .ForMember(dest => dest.TargetCompany, opt => opt.MapFrom(src => src.Document.TargetCompany.ShortName))
                .ForMember(dest => dest.CustomerCompanyId, opt => opt.MapFrom(src => src.Document.CustomerCompanyId))
                .ForMember(dest => dest.TargetCompanyId, opt => opt.MapFrom(src => src.Document.TargetCompanyId));

            CreateMap<Contact, ContactOData>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName ?? string.Empty))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName ?? string.Empty))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone ?? string.Empty))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.ShortName ?? string.Empty));

            CreateMap<Company, CompanyOData>();

            CreateMap<User, UserOData>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Contact.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.Contact.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Contact.LastName));

            CreateMap<Employee, EmployeeOData>()
                .ForMember(dest => dest.ShortName, opt => opt.MapFrom(src => src.Company.ShortName))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Company.Note));
        }
    }

    public class DomainToDtoProfile : Profile
    {
        public DomainToDtoProfile()
        {
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Permissions,
                    opt => opt.MapFrom(src => src.RolePermissions.Where(x => !x.IsDeleted).Select(e => e.PermissionId)));

            CreateMap<Agreement, AgreementDto>()
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Document.DocNumber))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Document.Name))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Document.DocDate.ToShort()))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.DateBeg.ToShort()))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.DateEnd.ToShort()))
                .ForMember(dest => dest.CustomerCompany, opt => opt.MapFrom(src => src.Document.CustomerCompany.ShortName))
                .ForMember(dest => dest.TargetCompany, opt => opt.MapFrom(src => src.Document.TargetCompany.ShortName));

            CreateMap<Contact, ContactDto>();

            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.Roles,
                    opt => opt.MapFrom(src => src.EmployeeRoles.Where(x => !x.IsDeleted).Select(e => e.RoleId)));

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Companies, opt => opt.MapFrom(src => src.Employees.Select(e => e.CompanyId)))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
                .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.PasswordHash));

            CreateMap<Company, CompanyDto>();
        }
    }

    public class DtoToDomainProfile : Profile
    {
        public DtoToDomainProfile()
        {
            CreateMap<RoleDto, Role>();
            CreateMap<AgreementDto, Agreement>();
            CreateMap<ContactDto, Contact>();
            CreateMap<EmployeeDto, Employee>();
            CreateMap<UserDto, User>();
        }
    }
}