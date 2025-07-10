// Hacemos referencia a la clase Tarea.
using ProyectoPert.Entidades;

namespace ProyectoPert.Estructuras
{
    
    /// Implementación de una lista doblemente enlazada para almacenar todas las tareas del proyecto.
    
    public class ListaTareas
    {
        private NodoTarea? _inicio; // Referencia al primer nodo (Head).
        private NodoTarea? _fin;   // Referencia al último nodo (Tail).

        public ListaTareas()
        {
            _inicio = null;
            _fin = null;
        }

        /// Agrega una nueva tarea al final de la lista.
        /// name="tarea">La tarea a agregar.
        public void Agregar(Tarea tarea)
        {
            NodoTarea nuevoNodo = new NodoTarea(tarea);
            
            if (_inicio == null)
            {
                _inicio = nuevoNodo;
                _fin = nuevoNodo;
            }
            else
            {
                _fin!.Siguiente = nuevoNodo;
                nuevoNodo.Anterior = _fin;
                _fin = nuevoNodo;
            }
        }

        /// Busca una Tarea en la lista por su identificador único.
        /// name="id">El ID de la tarea a buscar.
        /// El objeto Tarea si se encuentra; de lo contrario, null.
        public Tarea? BuscarPorId(string id)
        {
            NodoTarea? actual = _inicio;
            while (actual != null)
            {
                if (actual.Dato.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                {
                    return actual.Dato;
                }
                actual = actual.Siguiente;
            }
            return null;
        }

        /// Elimina una tarea de la lista por su ID.
        /// name="id">El ID de la tarea a eliminar.
        public void Eliminar(string id)
        {
            NodoTarea? actual = _inicio;
            // Buscamos el nodo que contiene la tarea con el ID especificado.
            while (actual != null && !actual.Dato.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
            {
                actual = actual.Siguiente;
            }

            // Si no se encontró el nodo, no hacemos nada.
            if (actual == null) return;

            // Re-enlazamos los nodos anterior y siguiente para "saltar" el nodo actual.
            if (actual.Anterior != null)
            {
                // El nodo anterior ahora apunta al nodo siguiente del actual.
                actual.Anterior.Siguiente = actual.Siguiente;
            }
            else // Si el anterior es nulo, estábamos eliminando el inicio (_inicio).
            {
                _inicio = actual.Siguiente;
            }

            if (actual.Siguiente != null)
            {
                // El nodo siguiente ahora apunta al nodo anterior del actual.
                actual.Siguiente.Anterior = actual.Anterior;
            }
            else // Si el siguiente es nulo, estábamos eliminando el fin (_fin).
            {
                _fin = actual.Anterior;
            }
        }

        /// Devuelve el primer nodo para permitir el recorrido externo de la lista.
        /// El nodo inicial de la lista.
        public NodoTarea? GetInicio()
        {
            return _inicio;
        }
    }
}