// Hacemos referencia a la clase Tarea.
using ProyectoPert.Entidades;

namespace ProyectoPert.Estructuras
{
    /// <summary>
    /// Implementación de una lista doblemente enlazada para almacenar todas las tareas del proyecto.
    /// Cumple con el requisito principal de la estructura de datos del sistema.
    /// </summary>
    public class ListaTareas
    {
        private NodoTarea? _inicio; // Referencia al primer nodo (Head).
        private NodoTarea? _fin;   // Referencia al último nodo (Tail).

        public ListaTareas()
        {
            _inicio = null;
            _fin = null;
        }

        /// <summary>
        /// Agrega una nueva tarea al final de la lista.
        /// </summary>
        /// <param name="tarea">La tarea a agregar.</param>
        public void Agregar(Tarea tarea)
        {
            NodoTarea nuevoNodo = new NodoTarea(tarea);
            
            // Si la lista está vacía, el nuevo nodo es tanto el inicio como el fin.
            if (_inicio == null)
            {
                _inicio = nuevoNodo;
                _fin = nuevoNodo;
            }
            else // Si no está vacía, se enlaza al final.
            {
                _fin!.Siguiente = nuevoNodo; // El '!' indica al compilador que confiamos que _fin no es nulo aquí.
                nuevoNodo.Anterior = _fin;
                _fin = nuevoNodo; // El nuevo nodo es ahora el último.
            }
        }

        /// <summary>
        /// Busca una Tarea en la lista por su identificador único.
        /// Es crucial para establecer las dependencias entre tareas.
        /// </summary>
        /// <param name="id">El ID de la tarea a buscar.</param>
        /// <returns>El objeto Tarea si se encuentra; de lo contrario, null.</returns>
        public Tarea? BuscarPorId(string id)
        {
            NodoTarea? actual = _inicio;
            while (actual != null)
            {
                // Comparamos los IDs ignorando mayúsculas/minúsculas para más flexibilidad.
                if (actual.Dato.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                {
                    return actual.Dato;
                }
                actual = actual.Siguiente;
            }
            // Si el bucle termina, no se encontró la tarea.
            return null;
        }

        /// <summary>
        /// Devuelve el primer nodo para permitir el recorrido externo de la lista.
        /// </summary>
        /// <returns>El nodo inicial de la lista.</returns>
        public NodoTarea? GetInicio()
        {
            return _inicio;
        }
    }
}