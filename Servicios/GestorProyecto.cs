using ProyectoPert.Entidades;
using ProyectoPert.Estructuras;
using ProyectoPert.Utilidades;
using System;
using System.Linq;
using System.Text;

namespace ProyectoPert.Servicios
{
    /// <summary>
    /// Clase que orquesta las operaciones del proyecto.
    /// Conecta la interfaz de usuario con las estructuras de datos y los servicios de cálculo.
    /// </summary>
    public class GestorProyecto
    {
        private readonly ListaTareas _listaTareas;
        private readonly CalculadoraPert _calculadora;

        public GestorProyecto()
        {
            _listaTareas = new ListaTareas();
            _calculadora = new CalculadoraPert();
        }

        #region Precarga de Datos
        /// <summary>
        /// Cumple con el requisito de precargar los datos del proyecto de ejemplo.
        /// </summary>
        public void PrecargarDatosDeEjemplo()
        {
            _listaTareas.Agregar(new Tarea("A", "Definir Concepto y Tema", 2, 3, 4));
            _listaTareas.Agregar(new Tarea("B", "Seleccionar y Reservar Lugar", 3, 4, 5));
            _listaTareas.Agregar(new Tarea("C", "Diseñar Material Promocional", 4, 6, 8));
            _listaTareas.Agregar(new Tarea("D", "Contactar y Confirmar Oradores", 2, 3, 4));
            _listaTareas.Agregar(new Tarea("E", "Desarrollar Contenido del Evento", 5, 7, 9));
            _listaTareas.Agregar(new Tarea("F", "Campaña de Marketing", 6, 8, 10));
            _listaTareas.Agregar(new Tarea("G", "Coordinar Logística", 1, 2, 3));
            _listaTareas.Agregar(new Tarea("H", "Ensayo General y Ajustes", 1, 1, 1));

            EstablecerDependencia("A", "B");
            EstablecerDependencia("A", "C");
            EstablecerDependencia("B", "D");
            EstablecerDependencia("C", "E");
            EstablecerDependencia("D", "F");
            EstablecerDependencia("E", "F");
            EstablecerDependencia("F", "G");
            EstablecerDependencia("G", "H");
        }

        /// <summary>
        /// Método de ayuda para conectar dos tareas.
        /// </summary>
        private void EstablecerDependencia(string idPredecesor, string idSucesor)
        {
            var tareaPredecesora = _listaTareas.BuscarPorId(idPredecesor);
            var tareaSucesora = _listaTareas.BuscarPorId(idSucesor);

            if (tareaPredecesora != null && tareaSucesora != null)
            {
                tareaPredecesora.Sucesores.Agregar(idSucesor);
                tareaSucesora.Predecesores.Agregar(idPredecesor);
            }
        }
        #endregion

        #region Métodos Públicos (Llamados desde el Menú)

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
                string desc = t.Descripcion.Length > 25 ? t.Descripcion.Substring(0, 22) + "..." : t.Descripcion;
                string linea = String.Format("{0,-7} | {1,-25} | {2,-5:F1} | {3,-5:F1} | {4,-5:F1} | {5,-5:F1} | {6,-5:F1} | {7,-8:F1} | {8,-10}",
                                            t.Id, desc, t.TiempoEsperado, t.ES, t.EF, t.LS, t.LF, t.Holgura, (t.EsCritica ? "Sí" : "No"));
                Console.WriteLine(linea);
                actual = actual.Siguiente;
            }
        }

        /// <summary>
        /// Orquesta la ejecución de todos los pasos del cálculo PERT.
        /// </summary>
        public void EjecutarCalculos()
        {
            Console.WriteLine("\n--- REALIZANDO CÁLCULOS PERT ---");
            _calculadora.CalcularTiemposEsperados(_listaTareas);
            _calculadora.CalcularPaseAdelante(_listaTareas);
            _calculadora.CalcularPaseAtras(_listaTareas);
            _calculadora.CalcularHolguraYRutaCritica(_listaTareas);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("¡Cálculos completados con éxito!");
            Console.ResetColor();
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
                Console.WriteLine(sb.ToString(0, sb.Length - 4));
            }
            else
            {
                Console.WriteLine("La ruta crítica no ha sido calculada. Ejecute los cálculos (opción 2).");
            }
        }

        /// <summary>
        /// Guía al usuario para agregar una nueva tarea con validaciones.
        /// </summary>
        public void AgregarTarea()
        {
            Console.WriteLine("\n--- AGREGAR NUEVA TAREA ---");

            // --- ID ---
            string id;
            do {
                Console.Write("Ingrese el ID de la nueva tarea (ej: I): ");
                id = Console.ReadLine() ?? "";
                if (!Validador.EsStringNoVacio(id)) Console.WriteLine("El ID no puede estar vacío.");
                else if (!Validador.EsIdUnico(id, _listaTareas)) Console.WriteLine($"Error: El ID '{id}' ya existe.");
            } while (!Validador.EsStringNoVacio(id) || !Validador.EsIdUnico(id, _listaTareas));

            // --- Descripción ---
            string desc;
            do {
                Console.Write("Ingrese la descripción de la tarea: ");
                desc = Console.ReadLine() ?? "";
                if (!Validador.EsStringNoVacio(desc)) Console.WriteLine("La descripción no puede estar vacía.");
            } while (!Validador.EsStringNoVacio(desc));

            // --- Tiempos ---
            double o, m, p;
            do {
                Console.Write("Ingrese el tiempo Optimista (O): ");
                while (!Validador.EsDoublePositivo(Console.ReadLine(), out o)) Console.Write("Valor inválido. Ingrese un número positivo para O: ");

                Console.Write("Ingrese el tiempo Más Probable (M): ");
                while (!Validador.EsDoublePositivo(Console.ReadLine(), out m)) Console.Write("Valor inválido. Ingrese un número positivo para M: ");

                Console.Write("Ingrese el tiempo Pesimista (P): ");
                while (!Validador.EsDoublePositivo(Console.ReadLine(), out p)) Console.Write("Valor inválido. Ingrese un número positivo para P: ");
                
                if (!Validador.EsLogicaDeTiemposValida(o, m, p)) Console.WriteLine("Error: La lógica de tiempos (O <= M <= P) no es correcta. Intente de nuevo.");
            } while (!Validador.EsLogicaDeTiemposValida(o, m, p));

            // --- Predecesores ---
            string predecesoresStr;
            do {
                Console.Write("Ingrese los IDs de las tareas predecesoras, separados por comas (deje en blanco si no tiene): ");
                predecesoresStr = Console.ReadLine() ?? "";
            } while (!Validador.SonPredecesoresValidos(predecesoresStr, _listaTareas));
            
            // --- Creación y Enlace ---
            var nuevaTarea = new Tarea(id, desc, o, m, p);
            _listaTareas.Agregar(nuevaTarea);

            if (Validador.EsStringNoVacio(predecesoresStr))
            {
                var ids = predecesoresStr.Split(',').Select(idPred => idPred.Trim());
                foreach (var idPred in ids)
                {
                    EstablecerDependencia(idPred, id);
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Tarea agregada con éxito. Recuerde recalcular el análisis PERT (opción 2).");
            Console.ResetColor();
        }

        /// <summary>
        /// Permite al usuario modificar una tarea existente.
        /// </summary>
        public void ModificarTarea()
        {
            Console.WriteLine("\n--- MODIFICAR TAREA ---");
            Console.Write("Ingrese el ID de la tarea que desea modificar: ");
            string? id = Console.ReadLine();

            var tareaAModificar = _listaTareas.BuscarPorId(id ?? "");
            if (tareaAModificar == null)
            {
                Console.WriteLine($"Error: No se encontró ninguna tarea con el ID '{id}'.");
                return;
            }

            Console.WriteLine($"Modificando Tarea: {tareaAModificar.Descripcion}");
            Console.WriteLine("1. Modificar Descripción");
            Console.WriteLine("2. Modificar Tiempos (O, M, P)");
            Console.Write(">> Seleccione una opción: ");
            
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Ingrese la nueva descripción: ");
                    string? nuevaDesc = Console.ReadLine();
                    if (Validador.EsStringNoVacio(nuevaDesc)) {
                        tareaAModificar.Descripcion = nuevaDesc!;
                        Console.WriteLine("Descripción actualizada con éxito.");
                    } else {
                        Console.WriteLine("La descripción no puede estar vacía.");
                    }
                    break;
                case "2":
                    // Lógica similar a la de AgregarTarea para leer y validar tiempos.
                    double o, m, p;
                    do {
                        Console.Write("Nuevo tiempo Optimista: ");
                        while (!Validador.EsDoublePositivo(Console.ReadLine(), out o)) Console.Write("Valor inválido. Ingrese un número positivo: ");
                        Console.Write("Nuevo tiempo Más Probable: ");
                        while (!Validador.EsDoublePositivo(Console.ReadLine(), out m)) Console.Write("Valor inválido. Ingrese un número positivo: ");
                        Console.Write("Nuevo tiempo Pesimista: ");
                        while (!Validador.EsDoublePositivo(Console.ReadLine(), out p)) Console.Write("Valor inválido. Ingrese un número positivo: ");
                        if (!Validador.EsLogicaDeTiemposValida(o, m, p)) Console.WriteLine("Error: La lógica de tiempos (O <= M <= P) no es correcta.");
                    } while (!Validador.EsLogicaDeTiemposValida(o, m, p));
                    
                    tareaAModificar.TiempoOptimista = o;
                    tareaAModificar.TiempoMasProbable = m;
                    tareaAModificar.TiempoPesimista = p;
                    Console.WriteLine("Tiempos actualizados. Debe recalcular el análisis PERT (opción 2).");
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }


        /// <summary>
        /// Elimina una tarea del proyecto, validando dependencias.
        /// </summary>
        public void EliminarTarea()
        {
            Console.WriteLine("\n--- ELIMINAR TAREA ---");
            Console.Write("Ingrese el ID de la tarea que desea eliminar: ");
            string? id = Console.ReadLine();

            var tareaAEliminar = _listaTareas.BuscarPorId(id ?? "");
            if (tareaAEliminar == null)
            {
                Console.WriteLine($"Error: No se encontró ninguna tarea con el ID '{id}'.");
                return;
            }

            if (tareaAEliminar.Sucesores.GetInicio() != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: No se puede eliminar la tarea '{id}' porque otras tareas dependen de ella.");
                Console.ResetColor();
                return;
            }

            Console.Write($"¿Está seguro de que desea eliminar la tarea '{id} - {tareaAEliminar.Descripcion}'? (S/N): ");
            if (Console.ReadLine()?.Equals("S", StringComparison.OrdinalIgnoreCase) == true)
            {
                var predecesorNode = tareaAEliminar.Predecesores.GetInicio();
                while (predecesorNode != null)
                {
                    var tareaPredecesora = _listaTareas.BuscarPorId(predecesorNode.IdTarea);
                    tareaPredecesora?.Sucesores.Eliminar(id!);
                    predecesorNode = predecesorNode.Siguiente;
                }

                _listaTareas.Eliminar(id!);
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Tarea eliminada con éxito.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
            }
        }

        #endregion
    }
}