namespace VS_SLG6.Model.Entities
{
    public class UserRating
    {
        public int Id { get; set; }
        public User Origin{ get; set; }
        public User Target { get; set; }
        public int Stars { get; set; }
    }
}
