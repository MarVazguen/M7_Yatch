using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace M7_Yatch
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int nJugador = 0;
        string[] nomsJugador;
        public MainWindow()
        {  
            this.stackPanelJugadores = new StackPanel();
            InitializeComponent();
        }

        //private void integerUpDownNjugadores_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e) //{integerUpDownNjugadores //}

        private void btnCrearUsuarios_Click(object sender, RoutedEventArgs e)
        {
            if (integerUpDownNjugadores.Value != null && integerUpDownNjugadores.Value.Value > 0)
                for (int i = 0; i < integerUpDownNjugadores.Value.Value; i++)
                {

                    TextBox printTextBlock = new TextBox();
                    printTextBlock.Text = "Jugador" + nJugador;
                    printTextBlock.Tag = "Jugador" + nJugador;
                    printTextBlock.FontSize = 35;
                    printTextBlock.TextWrapping = TextWrapping.Wrap;
                    printTextBlock.TextAlignment = TextAlignment.Center;
                    printTextBlock.MinHeight = 50;
                    printTextBlock.Background = Brushes.Blue;
                    printTextBlock.Foreground = Brushes.White;
                    stackPanelJugadores.Children.Add(printTextBlock); nJugador++;
                }
            else MessageBox.Show("Error !!");
        }

        private void btnQuitar_Click(object sender, RoutedEventArgs e)
        { 
            if (nJugador >= integerUpDownNjugadores.Value.Value) 
            {
                for(int i = 0; i < integerUpDownNjugadores.Value.Value; i++)
                {
                    UIElement element = stackPanelJugadores.Children[nJugador - 1];
                    this.stackPanelJugadores.Children.Remove(element);
                    nJugador--;
                } 
            }  
        }

        private void btnJugar_Click(object sender, RoutedEventArgs e)
        {
            if (stackPanelJugadores.Children.Count >= 2)
            {
                //wndJuego newWindow = new wndJuego(stackPanelJugadores);
                //newWindow.DataContext = this;
                //newWindow.ShowDialog();
                //
                nomsJugador = new string[nJugador];
                int i = 0;
                foreach (var control in stackPanelJugadores.Children.OfType<TextBox>())
                {
                    if (control.GetType() == typeof(TextBox))
                    {
                        TextBox element = control as TextBox;
                        nomsJugador[i] = element.Text;
                        i++;
                    }
                }  
                wndJuego ownedWindow = new wndJuego(nomsJugador );
                //ownedWindow.Owner = this;
                this.Visibility = Visibility.Hidden;
                ownedWindow.ShowDialog();
                this.Close();
            }
            else MessageBox.Show("No hay suficientes jugadores");
        }
    }
}
