namespace BlackjackAvalonia.Models;

public class BlackjackGame
{
    public Deck deck { get; private set; }
    public Player Jugador { get; private set; }
    public Dealer Dealer { get; private set; }

    public int ApuestaActual { get; private set; }

    public BlackjackGame(string nombreJugador)
    {
        Jugador = new Player(nombreJugador);
        Dealer = new Dealer();
        deck = new Deck();
        deck.Barajar();
    }

    // Inicia una nueva ronda y descuenta la apuesta del saldo
    public bool NuevaRonda(int apuesta)
    {
        if (apuesta <= 0 || apuesta > Jugador.Saldo)
            return false;

        ApuestaActual = apuesta;
        Jugador.Saldo -= apuesta; 

        deck = new Deck();
        deck.Barajar();

        Jugador.Mano.Clear();
        Dealer.Mano.Clear();

        Jugador.PedirCarta(deck.SacarCarta());
        Jugador.PedirCarta(deck.SacarCarta());

        Dealer.PedirCarta(deck.SacarCarta());
        Dealer.PedirCarta(deck.SacarCarta());

        return true;
    }

    public void JugadorPide()
    {
        Jugador.PedirCarta(deck.SacarCarta());

        if (Jugador.CalcularTotal() > 21)
        {
            TerminarRonda(); 
        }
    }

    public void JugadorSePlanta()
    {
        // Dealer juega según reglas
        while (Dealer.CalcularTotal() < 17)
            Dealer.PedirCarta(deck.SacarCarta());

        TerminarRonda();
    }


    private void TerminarRonda()
    {
        int totalJugador = Jugador.CalcularTotal();
        int totalDealer = Dealer.CalcularTotal();

        if (totalJugador > 21 || (totalDealer <= 21 && totalDealer > totalJugador))
        {
            // El jugador ya perdió la apuesta al iniciar la ronda
        }
        else if (totalJugador == totalDealer)
        {
            // Empate, se devuelve la apuesta
            Jugador.Saldo += ApuestaActual;
        }
        else
        {
            // Gana el jugador, recibe doble de la apuesta
            Jugador.Saldo += ApuestaActual * 2;
        }

        // Reiniciar apuesta
        ApuestaActual = 0;
    }
}
