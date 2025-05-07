
using ERP.APIs.Contracts.Interfaces;
using ERP.APIs.Contracts.Services;
using ERP.APIs.Leaves.Services;
using ERP.APIs.Leaves.Services.interfaces;
using ERP.Base_sys.Helpers;
using ERP.Base_sys.jwtService;
using ERP.Base_sys.Login.Service;
using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys.User.Service;
using ERP.Entities;
using ERP.Entities._1_Configs;
using ERP.Services.Lists;
using ERP.Services.Lists.interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

namespace ERP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //sqlsever
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("erpDB")));
            #region authen & author
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {

                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                };
            });
            #endregion


            #region lifetime 
            //helpers
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<JwtHelper>();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped<ImageHelper>();
            builder.Services.AddScoped<FileHelper>();
            builder.Services.AddScoped<IEntityAuditHelper, EntityAuditHelper>();


            //system
            builder.Services.AddScoped<IAccountRepo, AccountRepo>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IUser, UserService>();

            //lists 
            builder.Services.AddScoped<PositionService>();
            builder.Services.AddScoped<DepartmentService>();
            builder.Services.AddScoped<EducationService>();
            builder.Services.AddScoped<EmergencyContactService>();
            builder.Services.AddScoped<WorkExperienceService>();
            builder.Services.AddScoped<IEmployeeService,EmployeeService>();
            builder.Services.AddScoped<IEmployeeDocumentService, EmployeeDocumentService>();
            builder.Services.AddScoped<ContractTypeService>();
            builder.Services.AddScoped<LeaveTypeService>();
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            //voucher
            builder.Services.AddScoped<EmployeeLeaveBalanceService>();
            builder.Services.AddScoped<ContractAddendumService>();
            builder.Services.AddScoped<ContractHistoryService>();
            builder.Services.AddScoped<IContract, ContractService>();
            builder.Services.AddScoped<LeaveRequest_detailsService>();
            builder.Services.AddScoped<ILeave, LeaveService>();
            //cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            #endregion


            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(app.Environment.WebRootPath, "resource")),
                RequestPath = "/resource"
            });
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
