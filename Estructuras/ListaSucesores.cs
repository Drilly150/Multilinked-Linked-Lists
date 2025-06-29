// Definimos el namespace para organizar nuestras estructuras de datos.
namespace ProyectoPert.Estructuras
{
    /// <summary>
    /// Implementación de una lista simplemente enlazada para almacenar los IDs
    /// de las tareas sucesoras o predecesoras.
    /// Cumple con el requisito de no usar colecciones de .NET[cite: 191].
    /// </summary>
    public class ListaSucesores
    {
        // El único punto de acceso a la lista. Es el primer nodo.
        private NodoSucesor? _inicio;

        public ListaSucesores()
        {
            _inicio = null; // Una lista nueva siempre comienza vacía.
        }

        /// <summary>
        /// Agrega un nuevo ID de tarea al final de la lista.
        /// </summary>
        /// <param name="idTarea">El ID de la tarea a agregar.</param>
        public void Agregar(string idTarea)
        {
            NodoSucesor nuevoNodo = new NodoSucesor(idTarea);

            // Caso 1: La lista está vacía.
            if (_inicio == null)
            {
                _inicio = nuevoNodo;
                return; // Terminamos la ejecución del método.
            }

            // Caso 2: La lista ya tiene nodos. Hay que recorrerla hasta el final.
            NodoSucesor actual = _inicio;
            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }

            // 'actual' es ahora el último nodo. Lo enlazamos con el nuevo.
            actual.Siguiente = nuevoNodo;
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