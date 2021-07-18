namespace AgreementManagement.Models.Agreements
{
    public class AgreementSearchModel
    {
        public string UserId { get; set; }
        public string SortColumn { get; set; }
        public string SortColumnDirection { get; set; }
        public string SearchValue { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }

    }
}
