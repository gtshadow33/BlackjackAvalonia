using Avalonia.Controls;
using Avalonia.Media.Imaging;  // Si también necesitas imágenes, por ejemplo
using BlackjackAvalonia.Models;  // Si tienes otras dependencias

namespace BlackjackAvalonia.Views
{
    public partial class Alerta : Window
    {
        private MainWindow mainWindow;

        public Alerta(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            NuevaRondaButton.Click += (_, _) => Ronda();
        }

        private void Ronda()
        {
            mainWindow.NuevaRonda();
            Close();
        }
    }
}
