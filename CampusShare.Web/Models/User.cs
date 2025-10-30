<<<<<<< HEAD
public class User
{
    protected string id { get; set; }
    protected string nombre { get; set; }
    protected string apellido { get; set; }
    protected string dni { get; set; }
    protected string email { get; set; }

    public User(string id, string nombre, string apellido, string dni, string email)
    {
        this.id = id;
        this.nombre = nombre;
        this.apellido = apellido;
        this.dni = dni;
        this.email = email;
=======
namespace CampusShare.Web.Models
{
    public class User
    {
        protected string id { get; set; }
        protected string nombre { get; set; }
        protected string apellido { get; set; }
        protected string dni { get; set; }
        protected string email { get; set; }

        public User(string id, string nombre, string apellido, string dni, string email)
        {
            this.id = id;
            this.nombre = nombre;
            this.apellido = apellido;
            this.dni = dni;
            this.email = email;
        }
>>>>>>> e043d0e15ebed0e6cfeedfdb596138f97edc53dc
    }
}
