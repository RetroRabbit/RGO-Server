namespace RGO.Repository.Entities
{
    public  class FormSubmit
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int formid { get; set; }
        public DateTime createDate { get; set; }
        public int status { get; set; }
        public string rejectionReason { get; set; } = null!;
    }
}
