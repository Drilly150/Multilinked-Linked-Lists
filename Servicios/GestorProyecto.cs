// Hacemos referencia a las clases que vamos a utilizar de otras carpetas.
using ProyectoPert.Entidades;
using ProyectoPert.Estructuras;
using System;
using System.Text; // Usaremos StringBuilder para la ruta crítica

namespace ProyectoPert.Servicios
{
    /// <summary>
    /// Clase que orquesta las operaciones del proyecto.
    /// Conecta la interfaz de usuario con las estructuras de datos y los servicios de cálculo.
    /// </summary>
    public class GestorProyecto
    {
        private readonly ListaTareas _listaTareas;
        // Añadimos una instancia de nuestra calculadora.
        private readonly CalculadoraPert _calculadora;

        public GestorProyecto()
        {
            _listaTareas = new ListaTareas();
            _calculadora = new CalculadoraPert(); // La instanciamos en el constructor.
        }

        // El método PrecargarDatosDeEjemplo() permanece exactamente igual.
        public void PrecargarDatosDeEjemplo()
        {
            // ... (código de precarga de la respuesta anterior)
            _listaTareas.Agregar(new Tarea("A", "Definir Concepto y Tema", 2, 3, 4));
            _listaTareas.Agregar(new Tarea("B", "Seleccionar y Reservar Lugar", 3, 4, 5));
            _listaTareas.Agregar(new Tarea("C", "Diseñar Material Promocional", 4, 6, 8));
            _listaTareas.Agregar(new Tarea("D", "Contactar y Confirmar Oradores", 2, 3, 4));
            _listaTareas.Agregar(new Tarea("E", "Desarrollar Contenido del Evento", 5, 7, 9));
            _listaTareas.Agregar(new Tarea("F", "Campaña de Marketing y Venta de Entradas", 6, 8, 10));
            _listaTareas.Agregar(new Tarea("G", "Coordinar Logística (Catering, Sonido, etc.)", 1, 2, 3));
            _listaTareas.Agregar(new Tarea("H", "Ensayo General y Ajustes Finales", 1, 1, 1));

            EstablecerDependencia("A", "B");
            EstablecerDependencia("A", "C");
            EstablecerDependencia("B", "D");
            EstablecerDependencia("C", "E");
            EstablecerDependencia("D", "F");
            EstablecerDependencia("E", "F");
            EstablecerDependencia("F", "G");
            EstablecerDependencia("G", "H");
        }
        private void EstablecerDependencia(string idPredecesor, string idSucesor)
        {
            // ... (código de la respuesta anterior)
            var tareaPredecesora = _listaTareas.BuscarPorId(idPredecesor);
            var tareaSucesora = _listaTareas.BuscarPorId(idSucesor);

            if (tareaPredecesora != null && tareaSucesora != null)
            {
                tareaPredecesora.Sucesores.Agregar(idSucesor);
                tareaSucesora.Predecesores.Agregar(idPredecesor);
            }
        }


        // --- Métodos llamados desde Program.cs (Ahora implementados) ---

        /// <summary>
        /// Muestra una tabla con los resultados del análisis PERT para cada tarea.
        /// </summary>
        public void MostrarTareas()
        {
            Console.WriteLine("\n--- LISTA DE TAREAS Y RESULTADOS PERT ---");
            string header = String.Format("{0,-7} | {1,-25} | {2,-5} | {3,-5} | {4,-5} | {5,-5} | {6,-5} | {7,-8} | {8,-10}",
                                            "TAREA", "DESCRIPCIÓN", "Te", "ES", "EF", "LS", "LF", "HOLGURA", "¿CRÍTICA?");
            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            NodoTarea? actual = _listaTareas.GetInicio();
            if (actual == null)
            {
                Console.WriteLine("No hay tareas cargadas en el proyecto.");
                return;
            }

            while (actual != null)
            {
                var t = actual.Dato;
                string linea = String.Format("{0,-7} | {1,-25} | {2,-5:F1} | {3,-5:F1} | {4,-5:F1} | {5,-5:F1} | {6,-5:F1} | {7,-8:F1} | {8,-10}",
                                                t.Id, t.Descripcion.Length > 25 ? t.Descripcion.Substring(0, 22) + "..." : t.Descripcion,
                                                t.TiempoEsperado, t.ES, t.EF, t.LS, t.LF, t.Holgura, (t.EsCritica ? "Sí" : "No"));
                Console.WriteLine(linea);
                actual = actual.Siguiente;
            }
        }
        
        /// <summary>
        /// Orquesta la ejecución de todos los pasos del cálculo PERT en el orden correcto.
        /// </summary>
        public void EjecutarCalculos()
        {
            Console.WriteLine("\n--- REALIZANDO CÁLCULOS PERT ---");

            // Se ejecutan los métodos de la calculadora en la secuencia correcta.
            _calculadora.CalcularTiemposEsperados(_listaTareas); // [cite: 17]
            _calculadora.CalcularPaseAdelante(_listaTareas); // [cite: 23]
            _calculadora.CalcularPaseAtras(_listaTareas); // [cite: 33]
            _calculadora.CalcularHolguraYRutaCritica(_listaTareas); // [cite: 43]

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("¡Cálculos completados con éxito!");
            Console.ResetColor();
            Console.WriteLine("Seleccione la opción '1' para ver la tabla de resultados detallados.");
            
            // Mostramos la ruta crítica inmediatamente después de calcular.
            MostrarRutaCritica();
        }

        /// <summary>
        /// Muestra la secuencia de tareas que componen la ruta crítica.
        /// </summary>
        public void MostrarRutaCritica()
        {
            Console.WriteLine("\n--- RUTA CRÍTICA DEL PROYECTO ---");
            var sb = new StringBuilder();
            NodoTarea? actual = _listaTareas.GetInicio();

            while (actual != null)
            {
                if (actual.Dato.EsCritica)
                {
                    sb.Append(actual.Dato.Id + " -> ");
                }
                actual = actual.Siguiente;
            }

            if (sb.Length > 0)
            {
                // Eliminar el último " -> "
                string ruta = sb.ToString(0, sb.Length - 4);
                Console.WriteLine(ruta);
            }
            else
            {
                Console.WriteLine("La ruta crítica no ha sido calculada. Por favor, ejecute los cálculos (opción 2).");
            }
        }


        public void AgregarTarea()
        {
            // TODO: Implementar la lógica para pedir datos al usuario y crear una nueva tarea.
            Console.WriteLine("\n--- AGREGAR NUEVA TAREA ---");
            Console.WriteLine("Funcionalidad de agregar tarea aún no implementada.");
        }
    }
}