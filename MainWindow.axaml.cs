using Avalonia.Controls;
using Avalonia.Media.Imaging;
using BlackjackAvalonia.Models;

namespace BlackjackAvalonia.Views
{
    public partial class MainWindow : Window
    {
        
        private BlackjackGame game;
        private bool rondaIniciada = false;

        public MainWindow()
        {
            InitializeComponent();

            game = new BlackjackGame("Jugador");

            NuevaRondaButton.Click += (_, _) => NuevaRonda();
            PedirButton.Click += (_, _) => PedirCarta();
            PlantarseButton.Click += (_, _) => Plantarse();
            DealerTotal.IsVisible = false;
            JugadorTotal.IsVisible = false;

            ActualizarUI();
        }

        private void NuevaRonda()
        {
             DealerTotal.IsVisible = true;
            JugadorTotal.IsVisible = true;
            float apuesta;
            if (!float.TryParse(Entrada.Text, out apuesta) || apuesta <= 0)
            {
                MostrarMensaje("Ingrese una cantidad válida para apostar.");
                return;
            }

            if (!game.NuevaRonda(apuesta))
            {
                MostrarMensaje("Saldo insuficiente para apostar.");
                return;
            }

            rondaIniciada = true;
            ActualizarUI();
        }

        private void PedirCarta()
        {
            if (!rondaIniciada)
            {
                MostrarMensaje("Primero inicia una ronda.");
                return;
            }

            game.JugadorPide();

            if (game.Jugador.CalcularTotal() > 21)
            {
                MostrarMensaje("¡Te pasaste de 21! Pierdes la ronda.");
                rondaIniciada = false;
            }

            ActualizarUI();
        }

        private void Plantarse()
        {
            if (!rondaIniciada)
            {
                MostrarMensaje("Primero inicia una ronda.");
                return;
            }

            game.JugadorSePlanta();
            rondaIniciada = false;

            ActualizarUI();

            int totalJugador = game.Jugador.CalcularTotal();
            int totalDealer = game.Dealer.CalcularTotal();

            if (totalJugador > 21 || (totalDealer <= 21 && totalDealer > totalJugador))
                MostrarMensaje("¡Perdiste la ronda!");
            else if (totalJugador == totalDealer)
                MostrarMensaje("Empate.");
            else
                MostrarMensaje("¡Ganaste la ronda!");
        }

        private void MostrarMensaje(string mensaje)
        {
            var dlg = new Alerta();
            dlg.FindControl<TextBlock>("MensajeText")!.Text = mensaje;
            dlg.ShowDialog(this);
        }       

        private void ActualizarUI()
        {
            SaldoText.Text = game.Jugador.Saldo.ToString("0.00");
            JugadorNombre.Text = game.Jugador.Nombre;

            // Limpiar paneles
            JugadorPanel.Children.Clear();
            DealerPanel.Children.Clear();

            // Mostrar cartas jugador
            foreach (var carta in game.Jugador.Mano)
                JugadorPanel.Children.Add(CrearCartaImage(carta));

            // Mostrar cartas dealer
            for (int i = 0; i < game.Dealer.Mano.Count; i++)
            {
                if (i == 0 || !rondaIniciada)
                    DealerPanel.Children.Add(CrearCartaImage(game.Dealer.Mano[i]));
                else
                    DealerPanel.Children.Add(CrearCartaBack());
            }

            JugadorTotal.Text = $"Total: {game.Jugador.CalcularTotal()}";
            DealerTotal.Text = rondaIniciada ? "Total: ?" : $"Total: {game.Dealer.CalcularTotal()}";
        }

        private Control CrearCartaImage(Card carta)
        {
            string ruta = ObtenerNombreArchivo(carta);

            return new Image
            {
                Source = new Bitmap(ruta),
                Width = 100,
                Height = 150
            };
        }

        private Control CrearCartaBack()
        {
            return new Image
            {
                Source = new Bitmap("Assets/cartas/red_joker.png"),
                Width = 100,
                Height = 150
            };
        }

        private string ObtenerNombreArchivo(Card carta)
        {
            string valor = carta.Valor switch
            {
                "A" => "ace",
                "J" => "jack",
                "Q" => "queen",
                "K" => "king",
                _ => carta.Valor
            };

            string palo = carta.Palo switch
            {
                "Corazones" => "hearts",
                "Diamantes" => "diamonds",
                "Tréboles" => "clubs",
                "Picas" => "spades",
                _ => carta.Palo
            };

            return $"Assets/cartas/{valor}_of_{palo}.png";
        }
    }
}
