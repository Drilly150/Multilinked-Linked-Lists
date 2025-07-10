// Hacemos referencia a la clase Tarea que se encuentra en la carpeta de Entidades.
using ProyectoPert.Entidades;

namespace ProyectoPert.Estructuras
{
    
    /// Representa un nodo en la lista doblemente enlazada principal.
    /// Este nodo contiene la Tarea completa y enlaces al nodo siguiente y anterior.
    
    public class NodoTarea
    {
        // El dato principal que almacena el nodo.
        public Tarea Dato { get; set; }
        
        // Enlace al siguiente nodo de la lista.
        public NodoTarea? Siguiente { get; set; }
        
        // Enlace al nodo previo de la lista.
        public NodoTarea? Anterior { get; set; }

        /// Constructor para crear un nuevo nodo de tarea.
        
        /// tarea">El objeto Tarea que se almacenará en este nodo.
        public NodoTarea(Tarea tarea)
        {
            Dato = tarea;
            Siguiente = null;
            Anterior = null;
        }
    }
}