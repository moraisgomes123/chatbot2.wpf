namespace CybersecurityChatbot.Chatbot
{
    public static class UIFormatter
    {
        public static string Bot(string message)
        {
            return "Chatbot: " + message;
        }

        public static string User(string userName, string message)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return "You: " + message;

            return $"{userName}: {message}";
        }
    }
}
