// Haremos referencia al namespace de nuestras estructuras de datos personalizadas.
using ProyectoPert.Estructuras;

namespace ProyectoPert.Entidades
{
    
    /// Representa una tarea individual dentro del proyecto PERT.
    /// Almacena todos los datos de entrada y los resultados de los cálculos. 
    
    public class Tarea
    {
        // --- Propiedades de Entrada ---
        public string Id { get; set; }
        public string Descripcion { get; set; }
        public double TiempoOptimista { get; set; } 
        public double TiempoMasProbable { get; set; } 
        public double TiempoPesimista { get; set; } 

        // --- Propiedades Calculadas ---
        public double TiempoEsperado { get; set; } 
        public double Varianza { get; set; }
        public double ES { get; set; }
        public double EF { get; set; }
        public double LS { get; set; } 
        public double LF { get; set; } 
        public double Holgura { get; set; } 
        public bool EsCritica { get; set; }

        // --- Relaciones con otras Tareas ---
        // El documento especifica una lista de sucesores. [cite: 182, 186]
        // Añadimos también una lista de predecesores para facilitar los cálculos del pase hacia adelante.
        public ListaSucesores Sucesores { get; set; }
        public ListaSucesores Predecesores { get; set; }

        
        /// Constructor para crear una nueva tarea con sus valores iniciales.
        /// Los demás campos se inicializan a cero o valores por defecto.
        
        public Tarea(string id, string descripcion, double optimista, double masProbable, double pesimista)
        {
            Id = id;
            Descripcion = descripcion;
            TiempoOptimista = optimista;
            TiempoMasProbable = masProbable;
            TiempoPesimista = pesimista;

            // Inicializamos los valores calculados por defecto.
            TiempoEsperado = 0;
            Varianza = 0;
            ES = 0;
            EF = 0;
            LS = 0;
            LF = 0;
            Holgura = 0;
            EsCritica = false;

            // Cada tarea tendrá sus propias listas de dependencias.
            // Estas serán instancias de nuestras listas enlazadas personalizadas.
            Sucesores = new ListaSucesores();
            Predecesores = new ListaSucesores();
        }
    }
}