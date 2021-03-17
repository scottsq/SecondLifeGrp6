namespace VS_SLG6.Model.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AvatarURL { get; set; }

        
        public override string ToString()
        {
            return string.Format("Id: [{0}], Name: [{1}]", Id, Name);
        }
    }
}
