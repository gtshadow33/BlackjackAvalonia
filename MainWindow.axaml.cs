using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using BlackjackAvalonia.Models;
using System;

namespace BlackjackAvalonia.Views
{
    public partial class MainWindow : Window
    {
        private BlackjackGame game;
        private bool rondaIniciada = false;
        public float apuesta;

        public MainWindow()
        {
            InitializeComponent();

            game = new BlackjackGame("Jugador");

            NuevaRondaButton.Click += (_, _) => MostrarMensaje("Ingrese una apuesta en la ventana emergente.");
            PedirButton.Click += (_, _) => PedirCarta();
            PlantarseButton.Click += (_, _) => Plantarse();
            DealerTotal.IsVisible = false;
            JugadorTotal.IsVisible = false;

            PedirButton.IsVisible = false;
            PlantarseButton.IsVisible = false;

            ActualizarUI();
        }

        public void NuevaRonda(float apuesta = 0)
        {
            DealerTotal.IsVisible = false;
            JugadorTotal.IsVisible = true;
            PedirButton.IsVisible = true;
            PlantarseButton.IsVisible = true;

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
                DealerTotal.IsVisible = true;
                DealerTotal.Text = $"Total: {game.Dealer.CalcularTotal()}";
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
            DealerTotal.IsVisible = true;
            DealerTotal.Text = $"Total: {game.Dealer.CalcularTotal()}";

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

        public void MostrarMensaje(string mensaje)
        {
            var dlg = new Alerta(this);
            dlg.FindControl<TextBlock>("MensajeText")!.Text = mensaje;
            dlg.ShowDialog(this);
        }

        private void ActualizarUI()
        {
            SaldoText.Text = game.Jugador.Saldo.ToString("0.00");
            JugadorNombre.Text = game.Jugador.Nombre;

            JugadorPanel.Children.Clear();
            DealerPanel.Children.Clear();

            if (game.Jugador.CalcularTotal() == 0)
            {
                return;
            }

            foreach (var carta in game.Jugador.Mano)
                JugadorPanel.Children.Add(CrearCartaImage(carta));

            for (int i = 0; i < game.Dealer.Mano.Count; i++)
            {
                if (i == 0 || !rondaIniciada)
                    DealerPanel.Children.Add(CrearCartaImage(game.Dealer.Mano[i]));
                else
                    DealerPanel.Children.Add(CrearCartaBack());
            }

            JugadorTotal.Text = $"Total: {game.Jugador.CalcularTotal()}";
        }

        private Control CrearCartaImage(Card carta)
        {
            string ruta = ObtenerNombreArchivo(carta);

            // AvaloniaResource
            var uri = new Uri($"avares://BlackjackAvalonia/{ruta}");
            return new Image
            {
                Source = new Bitmap(AssetLoader.Open(uri)),
                Width = 120,
                Height = 180
            };
        }

        private Control CrearCartaBack()
        {
            var uri = new Uri("avares://BlackjackAvalonia/Assets/cartas/red_joker.png");
            return new Image
            {
                Source = new Bitmap(AssetLoader.Open(uri)),
                Width = 120,
                Height = 180
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
