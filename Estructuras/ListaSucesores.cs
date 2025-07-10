namespace ProyectoPert.Estructuras
{
    
    /// Implementación de una lista simplemente enlazada para almacenar los IDs
    /// de las tareas sucesoras o predecesoras.
    
    public class ListaSucesores
    {
        private NodoSucesor? _inicio;

        public ListaSucesores()
        {
            _inicio = null;
        }

        /// Agrega un nuevo ID de tarea al final de la lista.
        
        /// name="idTarea">El ID de la tarea a agregar.
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

        /// Elimina un nodo de la lista por su ID de tarea.
        /// name="idTarea">El ID a eliminar.
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

        /// Devuelve el primer nodo de la lista para poder recorrerla desde fuera.
        /// El nodo inicial de la lista.
        public NodoSucesor? GetInicio()
        {
            return _inicio;
        }
    }
}