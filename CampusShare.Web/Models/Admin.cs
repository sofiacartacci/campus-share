namespace CampusShare.Web.Models
{
    public class Admin : User
    {
        public Admin() { }

        public Admin(string id, string nombre, string apellido, string dni, string email)
            : base(id, nombre, apellido, dni, email)
        {
        }
    }
}
