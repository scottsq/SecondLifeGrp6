namespace VS_SLG6.Model.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public enum Roles { ADMIN, USER };
}
