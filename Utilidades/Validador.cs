using ProyectoPert.Estructuras;
using System;
using System.Linq;

namespace ProyectoPert.Utilidades
{
    
    /// Proporciona métodos estáticos para validar la entrada del usuario y la lógica de negocio. 
    public static class Validador
    {
        
        /// Verifica que el ID de una nueva tarea no exista ya en la lista de tareas.
        
        /// name="id">El ID a verificar.
        /// name="listaTareas">La lista de tareas actual.
        /// True si el ID es único; de lo contrario, false.
        public static bool EsIdUnico(string id, ListaTareas listaTareas)
        {
            return listaTareas.BuscarPorId(id) == null;
        }

        
        /// Verifica que una cadena de texto no sea nula, vacía o solo espacios en blanco.
        
        /// name="input">La cadena a validar.
        /// True si la cadena es válida; de lo contrario, false.
        public static bool EsStringNoVacio(string? input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        /// Intenta convertir una cadena en un número decimal positivo.
        
        /// name="input">La cadena a convertir.name="valor">El valor convertido, si la operación tiene éxito. True si la conversión es exitosa y el número es positivo; de lo contrario, false.
        public static bool EsDoublePositivo(string? input, out double valor)
        {
            if (double.TryParse(input, out valor) && valor >= 0)
            {
                return true;
            }
            valor = -1;
            return false;
        }

        /// Valida la lógica de las estimaciones de tiempo (Optimista <= Probable <= Pesimista).
        
        /// name="o">Tiempo Optimista.
        /// name="m">Tiempo Más Probable.
        /// name="p">Tiempo Pesimista.
        /// True si la lógica es correcta; de lo contrario, false.
        public static bool EsLogicaDeTiemposValida(double o, double m, double p)
        {
            return o <= m && m <= p;
        }

        
        /// Valida una cadena de IDs de predecesores separados por comas.
        
        /// name="input">La cadena del usuario (ej: "A, B, C").
        /// name="listaTareas">La lista de tareas para verificar la existencia de los IDs.
        /// True si todos los IDs son válidos y existen; de lo contrario, false.
        public static bool SonPredecesoresValidos(string? input, ListaTareas listaTareas)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return true; // No tener predecesores es una opción válida.
            }

            // Dividimos la cadena por comas y limpiamos los espacios de cada ID.
            var ids = input.Split(',').Select(id => id.Trim());

            foreach (var id in ids)
            {
                if (string.IsNullOrEmpty(id) || listaTareas.BuscarPorId(id) == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: La tarea predecesora con ID '{id}' no existe.");
                    Console.ResetColor();
                    return false; // Si un ID no existe, la validación falla.
                }
            }

            return true; // Todos los IDs fueron encontrados.
        }
    }
}