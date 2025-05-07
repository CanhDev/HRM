using AutoMapper;
using ERP.APIs.Contracts.Model;
using ERP.APIs.Contracts.Model.Contract;
using ERP.APIs.Leaves.DTOs;
using ERP.APIs.Leaves.DTOs_Res;
using ERP.APIs.Leaves.entity;
using ERP.Base_sys.User.models;
using ERP.Data_res.Lists;
using ERP.DTO.Lists;
using ERP.Entities._1_Configs;
using ERP.Entities.Lists.Employee;
using ERP.Entities.Vouchers.Employee;

namespace ERP.Base_sys.Helpers
{
    public class Mapper : Profile
    {
        
        public Mapper() {
            //employeemap
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<EmergencyContact, EmergencyContactDTO>().ReverseMap();
            CreateMap<Education, EducationDTO>().ReverseMap();
            CreateMap<WorkExperienceDTO, WorkExperience>()
                        .ForMember(dest => dest.startDate,
                            opt => opt.MapFrom(src => src.startDate ?? DateTime.MinValue))
                        .ForMember(dest => dest.endDate,
                            opt => opt.MapFrom(src => src.endDate ?? DateTime.MinValue))
                        .ReverseMap();

            CreateMap<EmployeeDocument, EmployeeDocumentDTO>().ReverseMap();

            CreateMap<Employee, Employee_res>().ReverseMap();
            CreateMap<EmergencyContact, EmergencyContact_res>().ReverseMap();
            CreateMap<Education, Education_res>().ReverseMap();
            CreateMap<WorkExperience, WorkExperience_res>().ReverseMap();
            CreateMap<EmployeeDocument, EmployeeDocument_res>().ReverseMap();
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserRes>().ReverseMap();
            // contract
            CreateMap<EmploymentContract, ContractDTO>().ReverseMap();
            CreateMap<EmploymentContract, ContractRes>().ReverseMap();
            CreateMap<ContractAddendum, ContractAddendumDTO>().ReverseMap();
            CreateMap<ContractAddendum, ContractAddendumRes>().ReverseMap();
            CreateMap<ContractHistory, ContractHistoryDTO>().ReverseMap();
            CreateMap<ContractHistory, ContractHistoryRes>().ReverseMap();
            //leave
            CreateMap<LeaveRequest, LeaveRequestDto>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestRes>().ReverseMap();
            CreateMap<EmployeeLeaveBalance, LeaveBalanceRes>().ReverseMap();
            CreateMap<EmployeeLeaveBalance, LeaveBalanceResSub>().ReverseMap();
            CreateMap<LeaveRequest_details, LeaveRequestDetaislDto>().ReverseMap();
            CreateMap<LeaveRequest_details, LeaveRequestDetailsRes>().ReverseMap();
        }
    }
}
