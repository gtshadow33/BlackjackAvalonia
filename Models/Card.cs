namespace BlackjackAvalonia.Models
{
    public class Card
    {
        public string Valor { get; }
        public string Palo { get; }

        public Card(string valor, string palo)
        {
            Valor = valor;
            Palo = palo;
        }

        public int GetPuntos() =>
            Valor switch
            {
                "A" => 11,
                "J" or "Q" or "K" => 10,
                _ => int.Parse(Valor)
            };


    }
}
