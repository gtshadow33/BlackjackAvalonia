using System.Collections.Generic;
using System.Linq;

namespace BlackjackAvalonia.Models
{
    public class Player
    {
        public string Nombre { get; }
        public List<Card> Mano { get; } = new();
        public float Saldo { get; set; } = 100;

        public Player(string nombre) => Nombre = nombre;

        public void PedirCarta(Card carta) => Mano.Add(carta);

        public int CalcularTotal()
        {
            int total = Mano.Sum(c => c.GetPuntos());
            int ases = Mano.Count(c => c.Valor == "A");

            while (total > 21 && ases-- > 0)
                total -= 10;

            return total;
        }


    }
}
