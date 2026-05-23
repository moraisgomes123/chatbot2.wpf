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

            var loader =
                new JsonResponseLoader("Data/responses.json");

            var responses =
                loader.LoadResponses();

            var service =
                new ResponseService(responses);

            _chatbot =
                new ChatbotEngine(service);

            new VoiceGreeting().PlayGreeting();

            // ===================================
            // ASK USER FOR NAME AT STARTUP
            // ===================================

            string welcomeMessage =
                _chatbot.ProcessMessage("");

            ChatDisplay.AppendText(
                "Bot: " + welcomeMessage + "\n\n");
        }

        private void SendButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            string input =
                UserInput.Text;

            if (string.IsNullOrWhiteSpace(input))
                return;

            // Show user message
            ChatDisplay.AppendText(
                "You: " + input + "\n");

            // Get bot response
            string response =
                _chatbot.ProcessMessage(input);

            // Show bot response
            ChatDisplay.AppendText(
                "Bot: " + response + "\n\n");

            // Clear input box
            UserInput.Clear();
        }
    }
}