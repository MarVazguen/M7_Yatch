using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M7_Yatch
{
    public class Jugador
    {
        public string nomJugador = string.Empty;
        private int nTiradasGastadasPartit = 0;
        Dictionary<Utils.CategoriaJuego, int> puntuacio = new Dictionary<Utils.CategoriaJuego, int>();
        bool[] categoriaOcupat = new bool[12];
        //puntuacio total
        private int totalSuperior = 0, subTotalSessioSuperior = 0, ExtraSeccioSuperior = 0;
        private int totalInferior = 0;
        private int totalGeneral = 0; 
        public Jugador()
        {
            this.nomJugador = "Default";
        }
        public Jugador(string nom)
        {
            this.nomJugador = nom;
        } 
        public int NTiradasGastadas { get => nTiradasGastadasPartit; set => nTiradasGastadasPartit = value; }
        /// <summary>
        /// 12 categorias mas uno extra
        /// </summary>
        public Dictionary<Utils.CategoriaJuego, int> Puntuacio { get => puntuacio; set => puntuacio = value; }
        /// <summary>
        /// 12 categorias mas uno extra
        /// </summary>
        public bool[] CategoriaOcupat { get => categoriaOcupat; set => categoriaOcupat = value; }
        public int TotalSuperior { get => totalSuperior; set => totalSuperior = value; }
        public int SubTotalSessioSuperior { get => subTotalSessioSuperior; set => subTotalSessioSuperior = value; }
        public int ExtraSeccioSuperior1 { get => ExtraSeccioSuperior; set => ExtraSeccioSuperior = value; }
        public int TotalInferior { get => totalInferior; set => totalInferior = value; }
        public int TotalGeneral { get => totalGeneral; set => totalGeneral = value; }
        public string NomJugador { set { nomJugador = value; }get { return nomJugador; } }


       
    }
}
