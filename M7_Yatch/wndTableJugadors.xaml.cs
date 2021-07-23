using System;
using System.Diagnostics;
using System.Globalization;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace M7_Yatch
{
    /// <summary>
    /// Lógica de interacción para wndTableJugadors.xaml
    /// </summary>
    public partial class wndTableJugadors : Window
    {
        string nomGanador = string.Empty;
        Jugador[] jugadors = null;
        bool fiJuego;
        public wndTableJugadors()
        {
            InitializeComponent();

        }
        public wndTableJugadors(Jugador[] jugadors, bool fiJuego) : this()
        {
            this.jugadors = jugadors;
            this.fiJuego = fiJuego;
        }
        public wndTableJugadors(Jugador[] jugadors,string nomGanador,bool fiJuego) : this()
        {
            this.jugadors = jugadors;
            this.nomGanador = nomGanador;
            this.fiJuego = fiJuego;
        }
        
        private static void GanadorSpeacher(string nomGanador)
        {
            using (var synthesizer = new SpeechSynthesizer())
            {
                synthesizer.SetOutputToDefaultAudioDevice();
                var builder = new PromptBuilder();
                builder.StartVoice(new CultureInfo("en-US"));
                builder.AppendText("Congratulations the winner is ");
                builder.EndVoice();
                builder.StartVoice(new CultureInfo("es-MX"));
                builder.AppendText(nomGanador);
                builder.EndVoice();
                synthesizer.Speak(builder);
            }
        }
        private void MostrarDades()
        {
            //Numero de columnas
            int nColumna = jugadors.Length;
            int nRows = 19;//Enum.GetNames(typeof(Utils.CategoriaJuego)).Length; 
            this.gridResultat.RowDefinitions.Add(new RowDefinition());
            for (int x = 0; x < nColumna; x++)
            {
                this.gridResultat.ColumnDefinitions.Add(new ColumnDefinition());
                TextBlock tb = new TextBlock();
                //tb.Width = 100;
                tb.Padding = new Thickness(10, 3, 10, 3);
                tb.TextAlignment = TextAlignment.Center;
                tb.FontSize = 15;
                tb.Foreground = Brushes.White;
                Grid.SetColumn(tb, x + 1);
                Grid.SetRow(tb, 0);
                tb.Text = jugadors[x].nomJugador;
                this.gridResultat.Children.Add(tb); 
              if (nomGanador != jugadors[x].nomJugador)
                for (int i = 0; i <= nRows; i++)
                {
                    TextBlock tb2 = new TextBlock(); 
                    tb2.TextAlignment = TextAlignment.Center;
                    tb2.FontSize = 15;
                    tb2.Foreground = Brushes.White;
                    int puntuacion;
                    if (jugadors[x].Puntuacio.TryGetValue((Utils.CategoriaJuego)i, out puntuacion)) { tb2.Text = "" + puntuacion; }
                    //else { tb2.Text = "None"; }

                    Grid.SetColumn(tb2, x + 1);
                    Grid.SetRow(tb2, i+1);
                    this.gridResultat.Children.Add(tb2); 
                }
               else
                    for (int i = 0; i < nRows; i++)
                    {
                        
                        TextBlock tb2 = new TextBlock();
                        tb2.Background = Brushes.Goldenrod;
                        tb2.Padding = new Thickness(20, 0, 20, 0);
                        tb2.TextAlignment = TextAlignment.Center;
                        tb2.FontSize = 15;
                        tb2.Foreground = Brushes.White;
                        int puntuacion;
                        if (jugadors[x].Puntuacio.TryGetValue((Utils.CategoriaJuego)i, out puntuacion)) 
                        { tb2.Text = "" + puntuacion; }
                        //else { tb2.Text = "None"; }

                        Grid.SetColumn(tb2, x + 1);
                        Grid.SetRow(tb2, i + 1);
                        this.gridResultat.Children.Add(tb2); 
                    }
            }
          
        }

        #region  Ver info en wikpedia desde HyperLink

        private void Hyperlink_RequestNavigate_1(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            using (Process p = new Process())
            {
                p.StartInfo = new ProcessStartInfo(e.Uri.AbsoluteUri);
                p.Start();
                e.Handled = true;
            }
        }
        #endregion



        private void gridResultat_Loaded(object sender, RoutedEventArgs e)
        {
            MostrarDades();
            if (nomGanador == string.Empty) nomGanador = "Nobody";
            if (fiJuego)
            {  
                Thread t = new Thread(() =>
                GanadorSpeacher(nomGanador));
                t.Start();
            }

        }
    }
}
