using ERP.Entities._0_Systems;
using ERP.Entities.Lists.Contract;
using ERP.Entities.Lists.Employee;
using ERP.Entities.Lists.Leave;
using ERP.Entities.Vouchers.Contract;
using ERP.Entities.Vouchers.Employee;
using ERP.Entities.Vouchers.Leave;
using Microsoft.EntityFrameworkCore;

namespace ERP.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //system
        public DbSet<ListOptions> ListOptions { get; set; }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<BaseEntity>();
            modelBuilder.Ignore<BaseVoucherEntity>();

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
                    .Property("Id")
                    .ValueGeneratedOnAdd();

                modelBuilder.Entity(entity)
                    .HasIndex("Id")
                    .IsUnique();
            }

            
            


            base.OnModelCreating(modelBuilder);
        }
    }
}
