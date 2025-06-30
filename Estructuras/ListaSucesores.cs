// Definimos el namespace para organizar nuestras estructuras de datos.
namespace ProyectoPert.Estructuras
{
    /// <summary>
    /// Implementación de una lista simplemente enlazada para almacenar los IDs
    /// de las tareas sucesoras o predecesoras.
    /// </summary>
    public class ListaSucesores
    {
        private NodoSucesor? _inicio;

        public ListaSucesores()
        {
            _inicio = null;
        }

        /// <summary>
        /// Agrega un nuevo ID de tarea al final de la lista.
        /// </summary>
        /// <param name="idTarea">El ID de la tarea a agregar.</param>
        public void Agregar(string idTarea)
        {
            NodoSucesor nuevoNodo = new NodoSucesor(idTarea);

            if (_inicio == null)
            {
                _inicio = nuevoNodo;
                return;
            }

            NodoSucesor actual = _inicio;
            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }
            actual.Siguiente = nuevoNodo;
        }

        /// <summary>
        /// Elimina un nodo de la lista por su ID de tarea.
        /// </summary>
        /// <param name="idTarea">El ID a eliminar.</param>
        public void Eliminar(string idTarea)
        {
            if (_inicio == null) return;

            // Caso 1: El nodo a eliminar es el primero.
            if (_inicio.IdTarea.Equals(idTarea, StringComparison.OrdinalIgnoreCase))
            {
                _inicio = _inicio.Siguiente;
                return;
            }

            // Caso 2: El nodo a eliminar está en medio o al final.
            NodoSucesor actual = _inicio;
            while (actual.Siguiente != null && !actual.Siguiente.IdTarea.Equals(idTarea, StringComparison.OrdinalIgnoreCase))
            {
                actual = actual.Siguiente;
            }

            // Si encontramos el nodo (es decir, actual.Siguiente no es nulo)
            if (actual.Siguiente != null)
            {
                // Se salta el nodo a eliminar, enlazando el actual con el siguiente del siguiente.
                actual.Siguiente = actual.Siguiente.Siguiente;
            }
        }

        /// <summary>
        /// Devuelve el primer nodo de la lista para poder recorrerla desde fuera.
        /// </summary>
        /// <returns>El nodo inicial de la lista.</returns>
        public NodoSucesor? GetInicio()
        {
            return _inicio;
        }
    }
}