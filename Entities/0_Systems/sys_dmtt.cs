namespace ERP.Entities._0_Systems
{
    public class sys_dmtt : BaseEntity
    {
        public string ma_ct { get; set; }
        public int status_code { get; set; }
        public string? status_name { get; set; }    
        public bool? isApproved { get; set; }
        public bool? isReject { get; set; }
        public short status { get; set; } = -1;
    }
}
