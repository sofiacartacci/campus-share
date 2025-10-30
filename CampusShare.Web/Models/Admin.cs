<<<<<<< HEAD
using System.Collections.Generic;

public class Admin : User
{
   

    public Admin(string id, string nombre, string apellido, string dni, string email)
        : base(id, nombre, apellido, dni, email)
   
=======
namespace CampusShare.Web.Models
{
    public class Admin : User
    {
        public Admin(string id, string nombre, string apellido, string dni, string email)
            : base(id, nombre, apellido, dni, email)
        {
        }
    }
>>>>>>> e043d0e15ebed0e6cfeedfdb596138f97edc53dc
}
