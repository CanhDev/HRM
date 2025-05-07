using ERP.APIs.Contracts.Model.Contract;
using ERP.APIs.Leaves.entity;
using ERP.Base_sys;
using ERP.Base_sys.Helpers;
using ERP.DTO.Lists;
using ERP.Entities._0_Systems;
using ERP.Entities._1_Configs;
using ERP.Entities.Lists.Contract;
using ERP.Entities.Lists.Employee;
using ERP.Entities.Vouchers.Employee;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ERP.Entities
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options) {
            _configuration = configuration;
        }

        //system
        public DbSet<ListOptions> ListOptions { get; set; }
        public virtual DbSet<RefreshTokenEntity> RefreshTokenEntities { get; set; }
        public DbSet<sys_dmtt> sys_Dmtts { get; set; }

        //lists
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<EmergencyContact> emergencyContacts { get; set; }
        public DbSet<WorkExperience> workExperiences { get; set; }

        //vouchers
        public DbSet<EmploymentContract> EmploymentContracts { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
        public DbSet<Termination> Terminations { get; set; }
        public DbSet <ContractAddendum> ContractAddendums { get; set; }
        public DbSet <ContractHistory> ContractHistories { get; set; }
        public DbSet <EmployeeLeaveBalance> employeeLeaveBalances { get; set; }
        public DbSet<LeaveBalanceHistory> leaveBalanceHistories { get; set; }
        //
        public DbSet<LeaveRequest_details> leaveRequest_Details { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<BaseEntity>();
            modelBuilder.Ignore<BaseVoucherEntity>();
            modelBuilder.Ignore<Employee_dataset>();
            modelBuilder.Ignore<FileHelper>();
            modelBuilder.Ignore<ImageHelper>();
            modelBuilder.Ignore<ApiRespone_basic>();


            var entitiesWithId = new Type[]
                 {
                    typeof(Employee),
                    typeof(ContractType),
                    typeof(Department),
                    typeof(Position),
                    typeof(LeaveType),
                    typeof(EmploymentContract),
                    typeof(LeaveRequest),
                 };

            foreach (var entity in entitiesWithId)
            {
                modelBuilder.Entity(entity)
                    .Property("id")
                    .ValueGeneratedOnAdd();

                modelBuilder.Entity(entity)
                    .HasIndex("id")
                    .IsUnique();
            }
            // Cấu hình cho ApplicationUser
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("AspNetUsers");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
