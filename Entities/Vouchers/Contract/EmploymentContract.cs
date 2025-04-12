using ERP.Entities._0_Systems;
using ERP.Entities.Lists.Contract;
using ERP.Entities.Lists.Employee;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Entities.Vouchers.Contract
{
    public class EmploymentContract : BaseVoucherEntity
    {

        public int EmployeeID { get; set; }
        

        public int ContractTypeID { get; set; }
        

        public decimal Salary { get; set; }
        [StringLength(250)]
        public string Notes { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime SignedDate { get; set; }
        public string WorkingTime { get; set; }
        public string JobDescription { get; set; }
        public string Benefits { get; set; }
        public string FilePath { get; set; }

        public string Terms { get; set; }
    }
}
