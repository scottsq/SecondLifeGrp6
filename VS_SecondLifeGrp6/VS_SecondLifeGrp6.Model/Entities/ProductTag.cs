namespace VS_SLG6.Model.Entities
{
    public class ProductTag
    {
        public int Id { get; set; }
        public Tag Tag { get; set; }
        public Product Product { get; set; }
    }
}
