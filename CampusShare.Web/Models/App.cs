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

        public void AgregarArticulo(Articulo articulo)
        {
            Articulos.Add(articulo);
        }

        public void GenerarDashboard()
        {
            Dashboard.MostrarResumen();
        }
    }
}
