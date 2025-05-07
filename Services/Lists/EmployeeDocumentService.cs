using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys;
using ERP.Entities;
using System.Linq.Expressions;
using ERP.Entities.Vouchers.Employee;
using ERP.Services.Lists.interfaces;
using ERP.DTO.Lists;
using ERP.Base_sys.Helpers;
using AutoMapper;

namespace ERP.Services.Lists
{

    public class EmployeeDocumentService : BaseService<EmployeeDocument>, IEmployeeDocumentService
    {
        private readonly ILogger<EmployeeDocumentService> _logger;
        private readonly FileHelper _fileHelper;
        private readonly IMapper _mapper;
        public EmployeeDocumentService(IBaseRepository<EmployeeDocument> repository,
            ApplicationDbContext context,
            FileHelper fileHelper,
            IMapper mapper,
            ILogger<EmployeeDocumentService> logger) : base(repository, mapper, context)
        {
            _logger = logger;
            _fileHelper = fileHelper;
            _mapper = mapper;
        }


        public override async Task<ApiResponse<List<EmployeeDocument>>> GetAllAsync(Expression<Func<EmployeeDocument, bool>>? predicate = null)
        {
            return await base.GetAllAsync(predicate);
        }

        public override async Task<ApiResponse<EmployeeDocument?>> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        public override async Task<ApiResponse<PagedResult<EmployeeDocument>>> GetPagedListAsync(
            SearchRequest<EmployeeDocument> searchRequest, params Expression<Func<EmployeeDocument, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public override async Task<ApiResponse<EmployeeDocument>> AddAsync(EmployeeDocument entity)
        {
            var res = await base.AddAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<EmployeeDocument>> UpdateAsync(EmployeeDocument entity)
        {
            var res = await base.UpdateAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<EmployeeDocument>> DeleteAsync(int id)
        {
            var res = await base.DeleteAsync(id);
            return res;
        }

        public override async Task<bool> DeleteRangeAsync(IEnumerable<int> ids)
        {
            return await base.DeleteRangeAsync(ids);
        }
        public override async Task<bool> DeleteAllAsync()
        {
            return await base.DeleteAllAsync();
        }

        public async Task<ApiRespone_basic> AddAsync_full(List<EmployeeDocumentDTO> req)
        {
            try
            {

                var EmployeeDocuments = new List<EmployeeDocument>();
                foreach (var document in req)
                {
                    var employeeDocumentE = _mapper.Map<EmployeeDocument>(document);
                    if (document.docfile != null)
                    {
                        var uploadResult = await _fileHelper.SaveFileAsync(
                            document.docfile,
                            "",
                            subFolder: "EmployeeDocument",
                            preserveOriginalName: true);

                        if (!uploadResult.Success)
                        {
                            return new ApiRespone_basic
                            {
                                Success = false,
                                Message = $"Upload file failed for one of the items: {uploadResult.ErrorMessage}"
                            };
                        }

                        employeeDocumentE.filePath = uploadResult.FileUrl;
                    }
                    EmployeeDocuments.Add(employeeDocumentE);
                }
                var res = await _repository.AddRangeAsync(EmployeeDocuments);
                return new ApiRespone_basic
                {
                    Data = res,
                    Success = true
                };
                
            }
            catch(Exception ex)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiRespone_basic> EditAsync_full(List<EmployeeDocumentDTO> req)
        {
            try
            {
                foreach (var document in req)
                {
                    if (document.docfile != null && document.actionType?.ToUpper() != "D")
                    {
                        var uploadResult = await _fileHelper.SaveFileAsync(
                            document.docfile,
                            "",
                            subFolder: "EmployeeDocument",
                            preserveOriginalName: true);

                        if (!uploadResult.Success)
                        {
                            return new ApiRespone_basic
                            {
                                Success = false,
                                Message = $"Upload file failed for item with ID = {document.id}: {uploadResult.ErrorMessage}"
                            };
                        }

                        document.filePath = uploadResult.FileUrl;
                    }
                }

                await ProcessActionTypeListAsync<EmployeeDocumentDTO>(req);

                return new ApiRespone_basic
                {
                    Success = true,
                    Message = "Operations completed successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }


    }
}
