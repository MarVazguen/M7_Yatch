using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M7_Yatch
{
   public class Utils
    {
        #region ReglasJuego
        public enum TipoJugada { Tirar,Jugar}
        public enum CategoriaJuego
        { 
            Uns,
            Dosos,
            Tresos,
            Quatres,
            Cincs,
            Sisos,

            ExtraSuperior,
            SubtotalSuperior,
            TotalSuperior,
            
            quatreIguals,
            Full,
            EscalaCurta,
            EscalaLlarga,
            Yatch,
            Atzar,
            
            ExtraInferior,
            SubtotalInferior, 
            TotalInferior,
            TotalGeneral,
            None
        }
        #endregion
        #region Agregar y actualizar datos de puntuacion de usuario
        /// <summary>
        /// Agrega y acualiza los datos de dictionario de puntuacion de usuario
        /// </summary>
        /// <param name="categoriaJugadaFeta"></param>
        /// <param name="puntuacio"></param>
        /// <param name="jugador"></param>
        public static void AgregarPuntuacionUsuario(CategoriaJuego categoriaJugadaFeta, int puntuacio, Jugador jugador)
        {
            int valorExtra = 35;//subtotal >63 , suma 35
            int valorExtraInferior = 50;// cada Yatch nou) 50
            int posicio = (int)categoriaJugadaFeta;
            int subtotalSuperior = 0;
            int subtotalInferior = 0;

            int totalSuperior = 0;
            int totalInferior = 0;


            //Agregem puntuacion de la categoria de joc
            if (jugador.Puntuacio.ContainsKey(categoriaJugadaFeta) == false) 
                jugador.Puntuacio.Add(categoriaJugadaFeta, puntuacio);
            else 
                jugador.Puntuacio[categoriaJugadaFeta] = puntuacio;


            if (posicio <= 8)//seccio superior
            {

                //subtotalSuperior = jugador.Puntuacio.TryGetValue(Utils.CategoriaJuego.SubtotalSuperior, out subtotalSuperior) ? subtotalSuperior : puntuacio ;

                //Calcul Subtotal superior
                if (!jugador.Puntuacio.ContainsKey(Utils.CategoriaJuego.SubtotalSuperior))
                {
                    jugador.Puntuacio.Add(Utils.CategoriaJuego.SubtotalSuperior, puntuacio);
                    subtotalSuperior = puntuacio;
                }
                else
                {
                    
                    jugador.Puntuacio.TryGetValue(Utils.CategoriaJuego.SubtotalSuperior, out subtotalSuperior);
                    subtotalSuperior += puntuacio;
                    jugador.Puntuacio[Utils.CategoriaJuego.SubtotalSuperior] = subtotalSuperior;
                }

                //Calcul Extra Superior 
                if (!jugador.Puntuacio.ContainsKey(Utils.CategoriaJuego.ExtraSuperior) && subtotalSuperior >= 63) 
                    totalSuperior = valorExtra + subtotalSuperior; 

                else  
                    totalSuperior = subtotalSuperior;

                //Calcul Total superior
                if (!jugador.Puntuacio.ContainsKey(Utils.CategoriaJuego.TotalSuperior))
                    jugador.Puntuacio.Add(Utils.CategoriaJuego.TotalSuperior, totalSuperior);
                else
                { 
                    jugador.Puntuacio[Utils.CategoriaJuego.TotalSuperior] = totalSuperior;
                }

            }
            else//Seccio inferior
            { 
                //Calcul Subtotal superior
                if (!jugador.Puntuacio.ContainsKey(Utils.CategoriaJuego.SubtotalInferior))
                    jugador.Puntuacio.Add(Utils.CategoriaJuego.SubtotalInferior, puntuacio);
                else
                {
                    jugador.Puntuacio.TryGetValue(Utils.CategoriaJuego.SubtotalInferior, out subtotalInferior);
                    jugador.Puntuacio[Utils.CategoriaJuego.SubtotalInferior] = subtotalInferior + puntuacio;
                }

                //Calcul Extra Superior : cada Yatch nou) 50
                if ( categoriaJugadaFeta == CategoriaJuego.Yatch)
                    totalInferior = valorExtraInferior + subtotalInferior;

                else
                    totalInferior = subtotalInferior;

                //Calcul Total superior
                if (!jugador.Puntuacio.ContainsKey(Utils.CategoriaJuego.TotalInferior))
                    jugador.Puntuacio.Add(Utils.CategoriaJuego.TotalInferior, totalInferior); 
                else
                {
                    jugador.Puntuacio[Utils.CategoriaJuego.TotalInferior] = totalInferior;
                }
                 
            }

            //Caclul total General
            if (jugador.Puntuacio.ContainsKey(Utils.CategoriaJuego.TotalGeneral))
                jugador.Puntuacio.Remove(Utils.CategoriaJuego.TotalGeneral);
            jugador.Puntuacio.Add(Utils.CategoriaJuego.TotalGeneral, totalInferior +totalSuperior);


        }
        #endregion
        #region puntuacion 

        public static int Puntuacion(CategoriaJuego categoriaJuego ,int[]puntDados)
        {
            int puntuacio = 0;
            int auxiliar = 0;
            // int distintos = puntDados.Distinct().Count(); 
            // var contador = Enumerable.Range(0, 5).ToDictionary(x => x, x => puntDados.Count(v => v == x));
            // var puntMinim = contador.Values.Where(x => x != 0).Min();

            switch (categoriaJuego)
            {
                case CategoriaJuego.Uns:
                    auxiliar   = Array.FindAll(puntDados, x => x == 1).Count();
                    puntuacio = auxiliar * 1;
                    break; 
                case CategoriaJuego.Dosos:
                    auxiliar = Array.FindAll(puntDados, x => x == 2).Count();
                    puntuacio = auxiliar * 2;
                    break;
                case CategoriaJuego.Tresos:
                    auxiliar = Array.FindAll(puntDados, x => x == 3).Count();
                    puntuacio = auxiliar * 3;
                    break;
                case CategoriaJuego.Quatres:
                    auxiliar = Array.FindAll(puntDados, x => x == 4).Count();
                    puntuacio = auxiliar * 4;
                    break;
                case CategoriaJuego.Cincs:
                    auxiliar = Array.FindAll(puntDados, x => x == 5).Count();
                    puntuacio = auxiliar * 5;
                    break;
                case CategoriaJuego.Sisos:
                    auxiliar = Array.FindAll(puntDados, x => x == 6).Count();
                    puntuacio = auxiliar * 6;
                    break; 
                case CategoriaJuego.quatreIguals:
                    if (NumDistinto(puntDados)==1)//4 iguales i   1 diferent
                    {
                        puntuacio = puntDados.Sum();
                    }
                    break;
                case CategoriaJuego.Full:
                    if (isFull(puntDados))
                        puntuacio = 25; //puntDados.Sum();
                    break;
                case CategoriaJuego.EscalaCurta:
                    if (isEscalaMenor(puntDados))
                        puntuacio = 30;
                    break;
                case CategoriaJuego.EscalaLlarga:
                    if(isEscalaMajor(puntDados))
                        puntuacio = 40;
                    break; 
                case CategoriaJuego.Yatch:
                    if(NumDistinto(puntDados) == 0)//tots son iguals
                    {
                        puntuacio = 50;
                    }
                    break; 
                case CategoriaJuego.Atzar:
                    puntuacio = puntDados.Sum();//suma total de valors
                    break;
            }
            return puntuacio;
        }

        /// <summary>
        /// Val 30 punts.
        /// </summary>
        /// <param name="dados"></param>
        /// <returns>retorna true si compleix el patro</returns>
        public static bool isEscalaMajor(int[] dados)
        {
            return dados[0] > dados[1] && dados[1] > dados[2] && dados[3] > dados[4]&& dados[4]>dados[5];//2-3-4-5-6 o 1-2-3-4-5. Val 30 punts.
        }
        /// <summary>
        ///  Val 15 punts
        /// </summary>
        /// <param name="dados"></param>
        /// <returns>retorna true si compleix el patro 1-2-3-4; 2-3-4-5 o 3-4-5-6 </returns>
        public static bool isEscalaMenor(int[] dados)
        {
            return dados[0] > dados[1] && dados[1] > dados[2] && dados[3] > dados[4];//1-2-3-4; 2-3-4-5 o 3-4-5-6. Val 15 punts
        }
        /// <summary>
        /// Tres iguals i una parella, . 3 sisos i 2 cincs val 28 
        /// </summary>
        /// <param name="dados"></param>
        /// <returns></returns>
        public static bool isFull(int[] dados)
        {
            var valorDistintos = dados.Distinct().GetEnumerator();
            int numDistinto = dados.Distinct().Count();
            bool isDistinto = false;
            if (numDistinto == 2)
            {
                while (valorDistintos.MoveNext())
                {
                    if (valorDistintos.Current % 2 == 0) { isDistinto = true; break; }
                }
            }
            return isDistinto;
        }
        private static int NumDistinto(int[] array)
        {
            int[] aux= new int[array.Length];
            aux[0] = array[0];
            int nDistinto = 0; 
            foreach (var c in array)
            {
                if (!aux.Contains(c)) { aux[nDistinto] = c; nDistinto++; }
            }
            return nDistinto;
        }
        #endregion

        /// <summary>
        /// Segun secion de jugada convertir la posicion en Enum a integer
        /// </summary>
        /// <param name="categoriaJuego"></param>
        /// <returns></returns>
        public static int PosicioControlOcupat(CategoriaJuego categoriaJuego)
        {
            int poscion = 0;
            switch (categoriaJuego)
            {
                case Utils.CategoriaJuego.Uns:
                    poscion = 0;
                    break;
                case Utils.CategoriaJuego.Dosos:
                    poscion = 1;
                    break;
                case Utils.CategoriaJuego.Tresos:
                    poscion = 2;
                    break;
                case Utils.CategoriaJuego.Quatres:
                    poscion = 3;
                    break;
                case Utils.CategoriaJuego.Cincs:
                    poscion = 4;
                    break;
                case Utils.CategoriaJuego.Sisos:
                    poscion = 5;
                    break;
                case Utils.CategoriaJuego.quatreIguals:
                    poscion = 6;
                    break;
                case Utils.CategoriaJuego.Full:
                    poscion = 7;
                    break;
                case Utils.CategoriaJuego.EscalaCurta:
                    poscion = 8;
                    break;
                case Utils.CategoriaJuego.EscalaLlarga:
                    poscion = 9;
                    break;
                case Utils.CategoriaJuego.Yatch:
                    poscion = 10;
                    break;
                case Utils.CategoriaJuego.Atzar:
                    poscion = 11;
                    break;
            }
            return poscion;
        }

        public Dictionary<CategoriaJuego, int> OrdenarPuntuacionGeneral(Dictionary<CategoriaJuego,int> dict)
        {
            // var sortedDict = from entry in myDict orderby entry.Value ascending select entry;
            // var ordenat = dict.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            var ordenat = dict.OrderBy(x => x.Key).ToDictionary(x => CategoriaJuego.TotalGeneral, x => x.Value);
            return ordenat;
        }
        


    }
}
