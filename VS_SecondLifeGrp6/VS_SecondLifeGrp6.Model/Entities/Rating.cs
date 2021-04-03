namespace VS_SLG6.Model.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string UserId { get; set; }
        public string Stars { get; set; }
    }
}
