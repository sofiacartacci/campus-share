namespace CampusShare.Web.Models
{
    public class App
    {
        public string Nombre { get; set; }
        public Dashboard Dashboard { get; set; }
        public List<Articulo> Articulos { get; set; }

        public App(string nombre)
        {
            Nombre = nombre;
            Dashboard = new Dashboard();
            Articulos = new List<Articulo>();
        }

        public void AgregarArticulo(String nombre, String descripcion, TipoArticulo tipoArticulo)
        {
            Articulos.Add(new Articulo(nombre, descripcion, tipoArticulo));
        }

        public void GenerarDashboard()
        {
            Dashboard.MostrarResumen();
        }
    }
}
