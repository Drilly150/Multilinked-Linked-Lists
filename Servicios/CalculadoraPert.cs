using ProyectoPert.Entidades;
using ProyectoPert.Estructuras;
using System;
using System.Collections.Generic; // Usaremos una lista temporal para el Pase Adelante/Atrás

namespace ProyectoPert.Servicios
{
    /// <summary>
    /// Contiene toda la lógica para realizar los cálculos PERT.
    /// Funciona como un servicio que recibe los datos, los procesa y los actualiza.
    /// </summary>
    public class CalculadoraPert
    {
        /// <summary>
        /// Paso 1: Calcula el Tiempo Esperado (Te) y la Varianza (V) para cada tarea.
        /// </summary>
        public void CalcularTiemposEsperados(ListaTareas listaTareas)
        {
            NodoTarea? actual = listaTareas.GetInicio();
            while (actual != null)
            {
                var tarea = actual.Dato;
                // Fórmula del Tiempo Esperado (Te) 
                tarea.TiempoEsperado = (tarea.TiempoOptimista + 4 * tarea.TiempoMasProbable + tarea.TiempoPesimista) / 6;
                
                // Fórmula de la Varianza (V) 
                tarea.Varianza = Math.Pow((tarea.TiempoPesimista - tarea.TiempoOptimista) / 6, 2);
                
                actual = actual.Siguiente;
            }
        }

        /// <summary>
        /// Paso 2: Realiza el Pase Adelante para calcular ES (Inicio Temprano) y EF (Finalización Temprana). [cite: 23]
        /// </summary>
        public void CalcularPaseAdelante(ListaTareas listaTareas)
        {
            NodoTarea? actual = listaTareas.GetInicio();
            while (actual != null)
            {
                var tarea = actual.Dato;
                
                // Las tareas sin predecesoras comienzan en tiempo 0. [cite: 26]
                if (tarea.Predecesores.GetInicio() == null)
                {
                    tarea.ES = 0;
                }
                else
                {
                    // El ES es el máximo de los EF de todas sus predecesoras. 
                    double maxEF_Predecesoras = 0;
                    NodoSucesor? predecesor = tarea.Predecesores.GetInicio();
                    while (predecesor != null)
                    {
                        var tareaPredecesora = listaTareas.BuscarPorId(predecesor.IdTarea);
                        if (tareaPredecesora != null && tareaPredecesora.EF > maxEF_Predecesoras)
                        {
                            maxEF_Predecesoras = tareaPredecesora.EF;
                        }
                        predecesor = predecesor.Siguiente;
                    }
                    tarea.ES = maxEF_Predecesoras;
                }

                // La Finalización Temprana (EF) se calcula como ES + Te. 
                tarea.EF = tarea.ES + tarea.TiempoEsperado;
                
                actual = actual.Siguiente;
            }
        }

        /// <summary>
        /// Paso 3: Realiza el Pase Hacia Atrás para calcular LF (Finalización Tardía) y LS (Inicio Tardío). [cite: 33]
        /// </summary>
        public void CalcularPaseAtras(ListaTareas listaTareas)
        {
            // Primero, encontramos la duración total del proyecto, que es el EF más alto de todas las tareas. 
            double duracionProyecto = 0;
            NodoTarea? nodoActual = listaTareas.GetInicio();
            while (nodoActual != null)
            {
                if (nodoActual.Dato.EF > duracionProyecto)
                {
                    duracionProyecto = nodoActual.Dato.EF;
                }
                nodoActual = nodoActual.Siguiente;
            }

            // Para recorrer la lista hacia atrás, necesitamos empezar desde el final.
            // Para simplificar, primero guardamos los nodos en una lista temporal.
            List<NodoTarea> nodosEnOrdenInverso = new List<NodoTarea>();
            nodoActual = listaTareas.GetInicio();
            while (nodoActual != null)
            {
                nodosEnOrdenInverso.Insert(0, nodoActual); // Inserta al principio para invertir el orden.
                nodoActual = nodoActual.Siguiente;
            }

            foreach (var nodo in nodosEnOrdenInverso)
            {
                var tarea = nodo.Dato;
                
                // Si la tarea no tiene sucesoras (es una tarea final), su LF es la duración del proyecto. [cite: 37]
                if (tarea.Sucesores.GetInicio() == null)
                {
                    tarea.LF = duracionProyecto;
                }
                else
                {
                    // El LF es el mínimo de los LS de todas sus sucesoras. 
                    double minLS_Sucesores = double.MaxValue;
                    NodoSucesor? sucesor = tarea.Sucesores.GetInicio();
                    while (sucesor != null)
                    {
                        var tareaSucesora = listaTareas.BuscarPorId(sucesor.IdTarea);
                        if (tareaSucesora != null && tareaSucesora.LS < minLS_Sucesores)
                        {
                            minLS_Sucesores = tareaSucesora.LS;
                        }
                        sucesor = sucesor.Siguiente;
                    }
                    tarea.LF = minLS_Sucesores;
                }

                // El Inicio Tardío (LS) se calcula como LF - Te. 
                tarea.LS = tarea.LF - tarea.TiempoEsperado;
            }
        }
        
        /// <summary>
        /// Paso 4: Calcula la Holgura (Slack) y determina la Ruta Crítica.
        /// </summary>
        public void CalcularHolguraYRutaCritica(ListaTareas listaTareas)
        {
            NodoTarea? actual = listaTareas.GetInicio();
            while (actual != null)
            {
                var tarea = actual.Dato;

                // Fórmula de la Holgura: S = LF - EF o S = LS - ES. 
                // Usamos Math.Round para evitar pequeñas imprecisiones con los números de punto flotante.
                tarea.Holgura = Math.Round(tarea.LF - tarea.EF, 5);

                // Las tareas con holgura cero forman la ruta crítica. 
                tarea.EsCritica = (tarea.Holgura == 0);
                
                actual = actual.Siguiente;
            }
        }
    }
}