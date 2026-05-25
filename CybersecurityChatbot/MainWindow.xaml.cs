using System.Windows;
using CybersecurityChatbot.Chatbot;
using CybersecurityChatbot.Services;

namespace CybersecurityChatbot
{
    public partial class MainWindow : Window
    {
        private readonly ChatbotEngine _chatbot;

        public MainWindow()
        {
            InitializeComponent();

            // =========================
            // ASCII BANNER (YOUR ORIGINAL)
            // =========================
            AsciiBanner.Text = @"
╔══════════════════════════════════════════════════════════════╗
║        ░█████╗░██╗░░░██╗██████╗░███████╗██████╗░             ║
║        ██╔══██╗╚██╗░██╔╝██╔══██╗██╔════╝██╔══██╗             ║
║        ██║░░╚═╝░╚████╔╝░██████╦╝█████╗░░██████╔╝             ║
║        ██║░░██╗░░╚██╔╝░░██╔══██╗██╔══╝░░██╔══██╗             ║
║        ╚█████╔╝░░░██║░░░██████╦╝███████╗██║░░██║             ║
║        ░╚════╝░░░░╚═╝░░░╚═════╝░╚══════╝╚═╝░░                ║
║                 CYBERSECURITY AWARENESS BOT                  ║
║                  Stay Safe Online!                           ║
╚══════════════════════════════════════════════════════════════╝
";

            // =========================
            // LOAD RESPONSES
            // =========================
            var loader = new JsonResponseLoader("Data/responses.json");
            var responses = loader.LoadResponses();

            var service = new ResponseService(responses);

            _chatbot = new ChatbotEngine(service);

            // =========================
            // GREETING (NO DUPLICATION BUG)
            // =========================
            ChatDisplay.AppendText("Chatbot: Hello! What is your name?\n\n");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = UserInput.Text;

            if (string.IsNullOrWhiteSpace(input))
                return;

            // Show user message
            ChatDisplay.AppendText("You: " + input + "\n");

            // Get chatbot response
            string response = _chatbot.ProcessMessage(input);

            // Show chatbot response
            ChatDisplay.AppendText("Chatbot: " + response + "\n\n");

            // Clear input
            UserInput.Clear();
        }
    }
}
