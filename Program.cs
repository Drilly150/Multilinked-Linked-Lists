// Las directivas 'using' nos permitirán acceder a las clases que creamos 
// en otras carpetas sin tener que escribir el nombre completo del namespace.
using ProyectoPert.Servicios;
using System;

// El namespace ayuda a organizar el código y evitar colisiones de nombres.
namespace ProyectoPert
{
    /// <summary>
    /// Clase principal que contiene el punto de entrada de la aplicación de consola.
    /// Gestiona el menú de usuario y la interacción principal.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Método principal y punto de entrada de la ejecución del programa.
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos (no los usaremos).</param>
        static void Main(string[] args)
        {
            // 1. Instanciamos el gestor del proyecto. Este objeto manejará toda la lógica.
            var gestorProyecto = new GestorProyecto();

            // 2. Se cargan los datos de ejemplo al iniciar, como lo requiere el documento[cite: 192].
            gestorProyecto.PrecargarDatosDeEjemplo();

            Console.WriteLine("==================================================");
            Console.WriteLine(" BIENVENIDO AL SISTEMA DE CÁLCULO DE GRÁFICAS PERT");
            Console.WriteLine("==================================================");
            Console.WriteLine("Los datos del proyecto de ejemplo han sido cargados.");

            bool salir = false;
            while (!salir)
            {
                // 3. Mostramos el menú de opciones al usuario.
                Console.WriteLine("\n----------- MENÚ PRINCIPAL -----------");
                Console.WriteLine("1. Mostrar tabla de resultados PERT");
                Console.WriteLine("2. Ejecutar/Recalcular análisis PERT");
                Console.WriteLine("3. Mostrar solo la Ruta Crítica");
                Console.WriteLine("4. Agregar una nueva tarea");
                Console.WriteLine("5. Modificar una tarea existente");
                Console.WriteLine("6. Eliminar una tarea");
                Console.WriteLine("7. Salir");
                Console.Write(">> Seleccione una opción: ");

                // 4. Leemos la opción del usuario y actuamos en consecuencia.
                string? opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        // Muestra la tabla completa con los datos y resultados.
                        gestorProyecto.MostrarTareas();
                        break;

                    case "2":
                        // Ejecuta todos los cálculos y al final muestra la ruta crítica.
                        gestorProyecto.EjecutarCalculos();
                        break;

                    case "3":
                        // Muestra únicamente la ruta crítica, sin recalcular nada.
                        gestorProyecto.MostrarRutaCritica();
                        break;
                        
                    case "4":
                        // Llama al método para agregar una nueva tarea.
                        gestorProyecto.AgregarTarea();
                        break;

                    case "5":
                        // Funcionalidad futura.
                        Console.WriteLine("\nFuncionalidad de modificar aún no implementada.");
                        break;

                    case "6":
                        // Funcionalidad futura.
                        Console.WriteLine("\nFuncionalidad de eliminar aún no implementada.");
                        break;

                    case "7":
                        salir = true;
                        Console.WriteLine("\nGracias por utilizar el sistema. ¡Hasta pronto!");
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opción no válida. Por favor, seleccione una opción del menú.");
                        Console.ResetColor();
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear(); // Limpia la consola para la siguiente iteración del menú.
                }
            }
        }
    }
}