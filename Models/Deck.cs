using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackAvalonia.Models
{
    public class Deck
    {
        private List<Card> cartas;
        private Random rnd = new();

        public Deck()
        {
            string[] palos = { "Corazones", "Diamantes", "Tréboles", "Picas" };
            string[] valores = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            cartas = new List<Card>();

            foreach (var palo in palos)
                foreach (var valor in valores)
                    cartas.Add(new Card(valor, palo));
        }

        public void Barajar() => cartas = cartas.OrderBy(c => rnd.Next()).ToList();

        public Card SacarCarta()
        {
            if (cartas.Count == 0)
                throw new Exception("No hay cartas en el mazo");

            Card carta = cartas[0];
            cartas.RemoveAt(0);
            return carta;
        }
    }
}
