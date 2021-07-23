using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace M7_Yatch
{
    /// <summary>
    /// Lógica de interacción para wndJuego.xaml
    /// </summary>
    public partial class wndJuego : Window
    {
        string nomGanador = string.Empty;
        Random rmd = new Random();
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private string[] nomJugadors = { };
        private bool[] dadosATirar = new bool[] {true,true,true,true,true};
        private int[] dadosPunt = new int[5];
        private int nJugadorActual = 0;// jugadorActual% jugadors.Lenght
        private const int nTotalPartidaPermitido = 12;
        private const int nTiradasPermidodos = 3;
        private int nPartidaJugada=0;
        private int nJugadors = 0;
        private int contJuegosHechos = 0;
        private Utils.CategoriaJuego categoriaJugadaFeta =   Utils.CategoriaJuego.None;
        Jugador[] jugadors;
        Jugador jugador;
         
        public wndJuego(string[] nomJugadors)
        {
            InitializeComponent();
            AsignarValoresIniciales(nomJugadors); 
        }

        private void AsignarValoresIniciales(string[] nomJugador)
        {

            stackPanelPuntuacionJugador.IsEnabled = true;
            this.nomJugadors = nomJugador;
            nJugadors = nomJugadors.Length;
            jugador = new Jugador(nomJugador[0]);
            this.tbNomJugadorActual.Text = jugador.NomJugador;

            jugadors = new Jugador[nJugadors];
            for (int i = 0; i < nJugadors; i++)
            {
                jugadors[i] = new Jugador(nomJugador[i]);
            }
          

          
            int nColumna = jugadors.Length;
            int nRows = gridColumnaCategoria.Children.Count;
            for (int x = 0; x < nColumna; x++)
            {
               
               jugadors[x].Puntuacio.Add((Utils.CategoriaJuego.SubtotalInferior), 0);
               jugadors[x].Puntuacio.Add((Utils.CategoriaJuego.SubtotalSuperior), 0);

               jugadors[x].Puntuacio.Add((Utils.CategoriaJuego.TotalSuperior), 0);
               jugadors[x].Puntuacio.Add((Utils.CategoriaJuego.TotalInferior), 0);

               jugadors[x].Puntuacio.Add((Utils.CategoriaJuego.TotalGeneral), 0);
                  
            }
            ActualizarTableUsuaris(jugadors);
            GenerarColumnDynamicTablePuntuacion();
            TirarDados();
        }
        #region Key presed
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
           
            if(e.Key == Key.F1)
            {
                this.Title = "Mostrar tabla de jugadors";
                wndTableJugadors wndTableJugadors = new wndTableJugadors(this.jugadors,false);  
                wndTableJugadors.Show();
              
            }
        }
        #endregion

        #region  Ver info en wikpedia desde HyperLink
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            using(Process p = new Process())
            {
                p.StartInfo = new ProcessStartInfo(e.Uri.AbsoluteUri);
                p.Start();
                e.Handled = true;
            }
           
        }
        #endregion


        #region Tabla Puntuacion de jugadores
        private void ActualizarTableUsuaris(Jugador[] jugadors)
        {
            //Numero de columnas
            int nColumna = jugadors.Length;
            int nRows = gridColumnaCategoria.Children.Count;
            for (int x = 0; x < nColumna; x++)
            {
                TextBlock tb = new TextBlock();
                //tb.Width = 100;
                tb.Padding = new Thickness(10, 0, 10, 0);
                tb.TextAlignment = TextAlignment.Center;
                tb.FontSize = 15;
                tb.Foreground = Brushes.White;
                Grid.SetColumn(tb, x);
                Grid.SetRow(tb, 0);
                tb.Text = jugadors[x].nomJugador;
                this.gridResultat.Children.Add(tb);
                Console.WriteLine($"Nombre jugador: {jugadors[x].nomJugador}");
                 
                for (int i = 0; i < nRows; i++)
                {
                    TextBlock tb2;
                    tb2 = new TextBlock();
                    tb2.TextAlignment = TextAlignment.Center;
                    tb2.FontSize = 15;
                    tb2.Foreground = Brushes.White;
                    tb2.Tag = $"{jugadors[x].nomJugador}{i}";

                    int puntuacion;
                    if (jugadors[x].Puntuacio.TryGetValue((Utils.CategoriaJuego)i, out puntuacion)) { tb2.Text = Convert.ToString(puntuacion); }
                    //else { tb2.Text = "None"; }
                    Console.WriteLine($" CAEGORIA  {(Enum.GetName(typeof( Utils.CategoriaJuego),i))}   row: {i}  puntuacion: {puntuacion.ToString()}");
                    Grid.SetColumn(tb2, x);
                    Grid.SetRow(tb2, i + 1);// <= 6 ? i + 1 : i - 1);//i==19?i-1:(i>5?i:

                    this.gridResultat.Children.Add(tb2);

                }
                Console.WriteLine("|||||||||   FIN     ||||||||");

            }
        }

        private void GenerarColumnDynamicTablePuntuacion()
        {
            for (int i = 0; i < jugadors.Length; i++)
            {
                this.gridResultat.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < gridColumnaCategoria.Children.Count;i++)// Enum.GetNames(typeof(Utils.CategoriaJuego)).Length; i++)
            {
                this.gridResultat.RowDefinitions.Add(new RowDefinition());
            }
        }

        private void ClearTableResultat(Jugador[] jugadors)
        {

            //Eliminar segun su Tag
            var textBlocks = gridResultat.Children.OfType<TextBlock>().ToList();
            for (int x = 0; x < jugadors.Length; x++)
                for (int i = 0; i <= gridResultat.RowDefinitions.Count; i++)
                {
                    foreach (var c in textBlocks)
                    {
                        TextBlock aux = (TextBlock)c;
                        if ((aux.Tag != null && aux.Tag.ToString() == $"{jugadors[x].nomJugador}{i}"))
                        {
                            gridResultat.Children.Remove(aux);
                        }
                    }
                }
            //gridResultat.Children.Clear();
            //gridResultat.ColumnDefinitions.Clear();
            //GenerarColumnDynamicTablePuntuacion();
        }
        #endregion

        #region metodos funcionalidad de juego

        private void btnBarejar_Click(object sender, RoutedEventArgs e)
        {
            DefaulBackgroundDados();
            stackPanelPuntuacionJugador.IsEnabled = true;
            if (dadosATirar.Contains(true) && jugadors[nJugadorActual].NTiradasGastadas<nTiradasPermidodos)
            {
                jugadors[nJugadorActual].NTiradasGastadas++;
                tbNumJugada.Text = Convert.ToString(jugadors[nJugadorActual].NTiradasGastadas);
                //SonidoDados();
                TirarDados();

            }
            else
            {
                if (categoriaJugadaFeta == Utils.CategoriaJuego.None)
                {
                    tbInfoUsuario.Text = "Falta escoger la categoria!!!";
                }
                else if (jugadors[nJugadorActual].NTiradasGastadas < 3 && !dadosATirar.Contains(true)) { tbInfoUsuario.Text = "Ningun dado selecionado para tirar !!!"; }
               
                else
                {
                    
                    //var sortedByCategoryName = jugadors.Cast<IDictionary<Utils.CategoriaJuego, int>>() .OrderBy(d => d.Values);

                    ActualizarDatosJugador(categoriaJugadaFeta, nJugadorActual);

                    jugadors[nJugadorActual].NTiradasGastadas = 0;
                    tbNumJugada.Text = "0";

                    contJuegosHechos++;
                    nPartidaJugada = contJuegosHechos / nJugadors;
                    tbNumPartida.Text = Convert.ToString(nPartidaJugada);

                    ///Jugador acutal
                    nJugadorActual = (nJugadorActual + 1) % nJugadors;

                    tbNomJugadorActual.Text = jugadors[nJugadorActual].nomJugador;
                    this.tbNomJugadorActual.Foreground = (nJugadorActual % 2) == 1 ? Brushes.White : Brushes.Black;

                    ClearTableResultat(jugadors);
                    ActualizarTableUsuaris(jugadors);

                    //desactivamos los campos ocupados de juego
                    ActualizarTablaLateralJuego(jugadors[nJugadorActual].CategoriaOcupat);
                    dadosATirar = Enumerable.Repeat(true, 5).ToArray();
                    //SonidoDados();
                    TirarDados();
                    categoriaJugadaFeta = Utils.CategoriaJuego.None;
                }
                if (nPartidaJugada == nTotalPartidaPermitido)
                {
                    ObtenirNombGuanyador();

                    //tbInfoUsuario.Text = "Fin del juego";
                    wndTableJugadors tableJugadors = new wndTableJugadors(jugadors,nomGanador,true);
                    //tableJugadors.Owner = this;
                    tableJugadors.Show();
                    this.Close();
                }
            }
           
        }

        private void DefaulBackgroundDados()
        {
            btnDadoUno.Background = Brushes.White;
            btnDadoDos.Background = Brushes.White;
            btnDadoTres.Background = Brushes.White;
            btnDadoCuatro.Background = Brushes.White;
            btnDadoCinco.Background = Brushes.White;
        }

        private void ObtenirNombGuanyador()
        {
            int aux = 0;
            int maxPuntuacion = 0;
            jugadors[0].Puntuacio.TryGetValue(Utils.CategoriaJuego.TotalGeneral, out aux);
            nomGanador = jugadors[0].nomJugador;
            maxPuntuacion = aux;
            for(int i = 1; i < jugadors.Length; i++)
            {
                jugadors[i].Puntuacio.TryGetValue( Utils.CategoriaJuego.TotalGeneral, out aux);
                if (aux > maxPuntuacion) { maxPuntuacion = aux; nomGanador = jugadors[i].nomJugador;  }//en caso de empate ganador sera el primero 

            } 

            if(maxPuntuacion == 0) nomGanador = "Nobody";
           
        }
        private void TirarDados()
        { 
            for (int i = 0; i < 5; i++)
            {
                if (dadosATirar[i] == true)
                {
                    dadosPunt[i] = rmd.Next(1, 7);
                    dadosATirar[i] = false;
                    switch (i)
                    {
                        case 0:
                            imgDadoUno.Source = new BitmapImage(new Uri(urlDados(dadosPunt[i]), UriKind.Relative));
                            break;
                        case 1:
                            imgDadosDos.Source = new BitmapImage(new Uri(urlDados(dadosPunt[i]), UriKind.Relative));
                            break;
                        case 2:
                            imgDadosTres.Source = new BitmapImage(new Uri(urlDados(dadosPunt[i]), UriKind.Relative));
                            break;
                        case 3:
                            imgDadosCuatro.Source = new BitmapImage(new Uri(urlDados(dadosPunt[i]), UriKind.Relative));
                            break;
                        case 4:
                            imgDadosCinco.Source = new BitmapImage(new Uri(urlDados(dadosPunt[i]), UriKind.Relative));
                            break;
                    }
                }
            }
            dadosATirar = new bool[5];
          
        }
        private void ActualizarDatosJugador( Utils.CategoriaJuego categoriaFeta, int nJugadorAct)
        {
            jugadors[nJugadorAct].NTiradasGastadas = 3;
            jugadors[nJugadorAct].CategoriaOcupat[Utils.PosicioControlOcupat(categoriaFeta)] = true;//Utils.PosicioControlOcupat(categoriaJuego)] = estado;//(int)categiriaJuego
            int puntuacio = Utils.Puntuacion(categoriaFeta, dadosPunt); 
            Utils.AgregarPuntuacionUsuario(categoriaFeta, puntuacio, jugadors[nJugadorAct]); 
             

        }


        /*private void ActualizarDatosTablaJugadors(Jugador jugador)
        {
            //dgJugadors.Items.Clear();
            //for(int i = 0; i < nJugadors; i++)
            //{ 
            //    int totalGeneral, totalInferior, totalSuperior;
            //    jugadors[i].Puntuacio.TryGetValue(Utils.CategoriaJuego.TotalSuperior,out totalSuperior);
            //    jugadors[i].Puntuacio.TryGetValue(Utils.CategoriaJuego.TotalInferior, out totalInferior);
            //    jugadors[i].Puntuacio.TryGetValue(Utils.CategoriaJuego.TotalGeneral, out totalGeneral);
            //    dgJugadors.Items.Add(new JugadorS( jugadors[i].nomJugador, totalInferior, totalSuperior, totalGeneral));
              
            //}

        }*/
        #endregion

        #region funcionalidad dados
        private void btnDadoUno_Click(object sender, RoutedEventArgs e)
        {
            JugadaDados(0);
            if (dadosATirar[0])
                btnDadoUno.Background = Brushes.Black; 
            else btnDadoUno.Background = Brushes.White;
           
        }
        private void btnDadoDos_Click(object sender, RoutedEventArgs e)
        {
            JugadaDados(1);
            if (dadosATirar[1])
                btnDadoDos.Background = Brushes.Black;
            else btnDadoDos.Background = Brushes.White;
            
        }
        private void btnDadoTres_Click(object sender, RoutedEventArgs e)
        {
            JugadaDados(2);
            if (dadosATirar[2])
                btnDadoTres.Background = Brushes.Black;// btnDadoCuatro.Background = Brushes.Black;// btnDadoCinco.Background = Brushes.Black;
            else btnDadoTres.Background = Brushes.White;
           
        }

        private void btnDadoCuatro_Click(object sender, RoutedEventArgs e)
        {
            JugadaDados(3);
            if (dadosATirar[3])
                btnDadoCuatro.Background = Brushes.Black;// btnDadoCinco.Background = Brushes.Black;
            else btnDadoCuatro.Background = Brushes.White;
            
        }

        private void btnDadoCinco_Click(object sender, RoutedEventArgs e)
        {
            JugadaDados(4);
            if (dadosATirar[4])
                btnDadoCinco.Background = Brushes.Black;
            else btnDadoCinco.Background = Brushes.White;
          
        }
         
        private void JugadaDados(int posicio)
        {
            if (jugadors[nJugadorActual].NTiradasGastadas < 3)
            {
                btnBarejar.Content = Utils.TipoJugada.Tirar.ToString();
                dadosATirar[posicio] = !dadosATirar[posicio];
            }
            else
            {  
                tbInfoUsuario.Text = "Error !! maximo tiradas dados";
              
            }
           
        }
        private string urlDados(int num)
        {
            string path = "";
            switch (num-1)
            {
                case 0:
                    path = @"Imagenes\uno.png";
                    break;
                case 1:
                    path = @"Imagenes\dos.png";
                    break;
                case 2:
                    path = @"Imagenes\tres.png";
                    break;
                case 3:
                    path = @"Imagenes\cuatro.png";
                    break;
                case 4:
                    path = @"Imagenes\cinco.png";
                    break;
                case 5:
                    path = @"Imagenes\seis.png";
                    break;
            }
            return path;
        }
        public void SonidoDados()
        {
             
            string pathSonido = @"../../Sonido/agitarDado.mp3";
            if ( File.Exists(pathSonido))
            { 
                Uri curtURI = new Uri(pathSonido, UriKind.Relative);
                mediaPlayer.Open(curtURI);
                mediaPlayer.Volume = 50;

                mediaPlayer.Play();
            }
        }
        #endregion

        #region Funcionalidad Barra Menu Jugador

        private void btnUns_Click(object sender, RoutedEventArgs e)
        {
           
            EscogerOpcionCategoria(Utils.CategoriaJuego.Uns);
        }
       
        private void btmDos_Click(object sender, RoutedEventArgs e)
        {
            EscogerOpcionCategoria(Utils.CategoriaJuego.Dosos);
        } 
       
        private void btnTres_Click(object sender, RoutedEventArgs e)
        {
            EscogerOpcionCategoria(Utils.CategoriaJuego.Tresos);
        }

        private void btnCuatro_Click(object sender, RoutedEventArgs e)
        {
            EscogerOpcionCategoria(Utils.CategoriaJuego.Quatres);
        }

        private void btnCinco_Click(object sender, RoutedEventArgs e)
        {
            EscogerOpcionCategoria(Utils.CategoriaJuego.Cincs);
        }

        private void btnSeis_Click(object sender, RoutedEventArgs e)
        {
            EscogerOpcionCategoria(Utils.CategoriaJuego.Sisos);
        }
         
        private void btnQuatreIguals_Click(object sender, RoutedEventArgs e)
        {
            EscogerOpcionCategoria(Utils.CategoriaJuego.quatreIguals);
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            EscogerOpcionCategoria(Utils.CategoriaJuego.Full);
        }

        private void btnEcalaCurta_Click(object sender, RoutedEventArgs e)
        {
            EscogerOpcionCategoria(Utils.CategoriaJuego.EscalaCurta);
        }

        private void btnEscalaLlarga_Click(object sender, RoutedEventArgs e)
        {
            EscogerOpcionCategoria(Utils.CategoriaJuego.EscalaLlarga);
        }

        private void btnYatch_Click(object sender, RoutedEventArgs e)
        {
            EscogerOpcionCategoria(Utils.CategoriaJuego.Yatch);
        }

        private void btnAtzar_Click(object sender, RoutedEventArgs e)
        {
            EscogerOpcionCategoria(Utils.CategoriaJuego.Atzar);
        }  
        private void ActualizarTablaLateralJuego(bool[] categoriaOcupada)
        {
            Console.WriteLine(jugadors[(nJugadorActual+1)%nJugadors].nomJugador+"  opcion: "+categoriaJugadaFeta);
            Console.WriteLine("Array de "+jugadors[nJugadorActual].nomJugador);
            Array.ForEach(jugadors[nJugadorActual].CategoriaOcupat, x => Console.Write(x + " ,"));
            Console.WriteLine();

            ActivarBarraJuego();

            for (int i=0;i<categoriaOcupada.Length;i++)
            {
               
                if (categoriaOcupada[i] == true)
                {
                    DesactivarControlCategoriaJugador(i);
                }
            }
           
        }
        /// <summary>
        /// Asignamos CategoiraOcupada de jugador, ntiradas gastadas a 3
        /// </summary>
        /// <param name="posicion"></param>
        /// <param name="estado"></param>
        private void EscogerOpcionCategoria(Utils.CategoriaJuego categoriaJuego)
        {
            jugadors[nJugadorActual].CategoriaOcupat[Utils.PosicioControlOcupat(categoriaJuego)] = true;
            jugadors[nJugadorActual].NTiradasGastadas = 3;
            btnBarejar.Content = Utils.TipoJugada.Jugar.ToString();

            this.categoriaJugadaFeta = categoriaJuego;
            stackPanelPuntuacionJugador.IsEnabled = false;


           
        }

        

        private void ActivarBarraJuego()
        {
            //botones
            btnUns.IsEnabled = true;
            btmDos.IsEnabled = true;
            btnTres.IsEnabled = true;
            btnCuatro.IsEnabled = true;
            btnCinco.IsEnabled = true;
            btnSeis.IsEnabled = true;

            btnQuatreIguals.IsEnabled = true;
            btnFull.IsEnabled = true;
            btnEcalaCurta.IsEnabled = true;
            btnEscalaLlarga.IsEnabled = true;
            btnYatch.IsEnabled = true;
            btnAtzar.IsEnabled = true;


            tbUnsResultat.Foreground = Brushes.Black;
            tbDosResultat.Foreground = Brushes.Black;
            tbTresResultat.Foreground = Brushes.Black;
            tbQuatreResultat.Foreground = Brushes.Black;
            tbCincResultat.Foreground = Brushes.Black;
            tbSisosResultat.Foreground = Brushes.Black;

            tbUnsResultat.TextDecorations = null;
            tbDosResultat.TextDecorations = null;
            tbTresResultat.TextDecorations = null;
            tbQuatreResultat.TextDecorations = null;
            tbCincResultat.TextDecorations = null;
            tbSisosResultat.TextDecorations = null;



           
        }
        private void DesactivarControlCategoriaJugador(int i)
        {
            
            switch (i)
            {
                case 0: 
                    btnUns.IsEnabled = false;
                    tbUnsResultat.TextDecorations = TextDecorations.Strikethrough;
                    tbUnsResultat.Foreground = Brushes.Blue;
                    break;

                case 1:
                    btmDos.IsEnabled = false;
                    tbDosResultat.TextDecorations = TextDecorations.Strikethrough;
                    tbDosResultat.Foreground = Brushes.Blue;
                    break;

                case 2: 
                    btnTres.IsEnabled = false;
                    tbTresResultat.TextDecorations = TextDecorations.Strikethrough;
                    tbTresResultat.Foreground = Brushes.Blue;
                    break;

                case 3: 
                    btnCuatro.IsEnabled = false;
                    tbQuatreResultat.TextDecorations = TextDecorations.Strikethrough;
                    tbQuatreResultat.Foreground = Brushes.Blue;
                    break;

                case 4: 
                    btnCinco.IsEnabled = false;
                    tbCincResultat.TextDecorations = TextDecorations.Strikethrough;
                    tbCincResultat.Foreground = Brushes.Blue;
                    break;

                case 5: 
                    btnSeis.IsEnabled = false;
                    tbSisosResultat.TextDecorations = TextDecorations.Strikethrough;
                    tbSisosResultat.Foreground = Brushes.Blue;
                    break;

                case 6: 
                    btnQuatreIguals.IsEnabled = false;
                    break;

                case 7: 
                    btnFull.IsEnabled = false;
                    break;

                case 8: 
                    btnEcalaCurta.IsEnabled = false;
                    break;

                case 9: 
                    btnEscalaLlarga.IsEnabled = false;
                    break;

                case 10: 
                    btnYatch.IsEnabled = false;
                    break;

                case 11: 
                    btnAtzar.IsEnabled = false;
                    break;
            }
        }
        #endregion
    }
}
