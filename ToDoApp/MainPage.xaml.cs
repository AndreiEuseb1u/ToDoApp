namespace ToDoApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnAddTaskTapped(object Sender, TappedEventArgs args)
        {
            
        }

        private void OnEntryAdded(object sender, EventArgs e)
        {
            // 1. Obține referința la câmpul Entry care a declanșat evenimentul
            var entry = (Entry)sender;

            // 2. Citește textul introdus de utilizator
            string subtask = entry.Text;

            // 3. Verifică dacă textul nu este gol sau format doar din spații
            if (!string.IsNullOrWhiteSpace(subtask))
            {
                // 4. Aici vei adăuga logica de salvare a subtask-ului în listă
                // TODO: add subtask to your list

                // 5. Golește câmpul după confirmare, pregătit pentru next input
                entry.Text = string.Empty;
            }
        }

    }
}
