// Definimos el namespace para organizar nuestras estructuras de datos.
namespace ProyectoPert.Estructuras
{
    
    /// Representa un nodo en la lista simplemente enlazada de sucesores.
    /// Contiene el ID de una tarea y una referencia al siguiente nodo.
    
    public class NodoSucesor
    {
        // El dato que almacena: el identificador de la tarea.
        public string IdTarea { get; set; }
        // El enlace o puntero al siguiente nodo en la lista.
        // Si es el último nodo, este valor será 'null'.
        public NodoSucesor? Siguiente { get; set; }
        /// Constructor para crear un nuevo nodo.
        /// name="idTarea">El ID de la tarea sucesora que se almacenará.
        public NodoSucesor(string idTarea)
        {
            IdTarea = idTarea;
            Siguiente = null; // Al crearse, un nodo no apunta a nadie.
        }
    }
}