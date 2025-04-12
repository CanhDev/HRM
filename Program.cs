
using ERP.Base_sys.Helpers;
using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Entities;
using ERP.Services.Lists;
using Microsoft.EntityFrameworkCore;

namespace ERP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //sqlsever
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("erpDB")));

            //helpers
            //builder.Services.AddScoped<IEntityAuditHelper, EntityAuditHelper>();

            // Add services to the container.

            #region lifetime 
            //helpers
            builder.Services.AddScoped<ImageHelper>();
            builder.Services.AddScoped<FileHelper>();
            //lists 
            builder.Services.AddScoped<PositionService>();
            builder.Services.AddScoped<DepartmentService>();
            builder.Services.AddScoped<EducationService>();
            builder.Services.AddScoped<EmergencyContactService>();
            builder.Services.AddScoped<WorkExperienceService>();
            builder.Services.AddScoped<EmployeeService>();
            builder.Services.AddScoped<ContractTypeService>();
            builder.Services.AddScoped<LeaveTypeService>();

            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));


            #endregion


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
