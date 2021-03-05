namespace VS_SLG6.Model.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
        public int Stars { get; set; }
    }
}
