namespace CybersecurityChatbot.Chatbot
{
    public static class UIFormatter
    {
        public static string FormatBotMessage(string message)
        {
            return "Bot: " + message;
        }

        public static string FormatUserMessage(string message)
        {
            return "You: " + message;
        }
    }
}