// Haremos referencia al namespace de nuestras estructuras de datos personalizadas.
using ProyectoPert.Estructuras;

// Definimos un namespace para organizar nuestras clases de entidad.
namespace ProyectoPert.Entidades
{
    /// <summary>
    /// Representa una tarea individual dentro del proyecto PERT.
    [cite_start]/// Almacena todos los datos de entrada y los resultados de los cálculos. 
    /// </summary>
    public class Tarea
    {
        // --- Propiedades de Entrada ---
        public string Id { get; set; }
        public string Descripcion { get; set; }
        public double TiempoOptimista { get; set; } // 'O' en la fórmula [cite: 15]
        public double TiempoMasProbable { get; set; } // 'M' en la fórmula [cite: 15]
        public double TiempoPesimista { get; set; } // 'P' en la fórmula [cite: 16]

        // --- Propiedades Calculadas ---
        public double TiempoEsperado { get; set; } // Te [cite: 17]
        public double Varianza { get; set; } // V [cite: 20]
        public double ES { get; set; } // Early Start - Inicio Temprano [cite: 23]
        public double EF { get; set; } // Early Finish - Finalización Temprana [cite: 23]
        public double LS { get; set; } // Late Start - Inicio Tardío [cite: 33]
        public double LF { get; set; } // Late Finish - Finalización Tardía [cite: 33]
        public double Holgura { get; set; } // Slack [cite: 44]
        public bool EsCritica { get; set; }

        // --- Relaciones con otras Tareas ---
        [cite_start]// El documento especifica una lista de sucesores. [cite: 182, 186]
        // Añadimos también una lista de predecesores para facilitar los cálculos del pase hacia adelante.
        public ListaSucesores Sucesores { get; set; }
        public ListaSucesores Predecesores { get; set; }

        /// <summary>
        /// Constructor para crear una nueva tarea con sus valores iniciales.
        /// Los demás campos se inicializan a cero o valores por defecto.
        /// </summary>
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

            [cite_start]// Cada tarea tendrá sus propias listas de dependencias. [cite: 182]
            // Estas serán instancias de nuestras listas enlazadas personalizadas.
            Sucesores = new ListaSucesores();
            Predecesores = new ListaSucesores();
        }
    }
}