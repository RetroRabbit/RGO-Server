namespace RGO.Repository.Entities
{
    public class User
    {

        public int id { get; set; }
        public int groupid { get; set; }
        public string firstname { get; set; } = null!;
        public string lastname { get; set; } = null!;
        public string email { get; set; } = null!;
        public int type { get; set; }
        public DateTime joindate { get; set; }
        public int status { get; set; }

    }
}
