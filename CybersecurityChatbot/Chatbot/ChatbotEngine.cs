using CybersecurityChatbot.Services;

namespace CybersecurityChatbot.Chatbot
{
    public class ChatbotEngine
    {
        private readonly ResponseService _service;

        private readonly ConversationContext _context;

        private readonly SentimentAnalyzer _sentiment;

        private readonly MemoryManager _memory;

        private readonly FollowUpHandler _followUp;

        public ChatbotEngine(ResponseService service)
        {
            _service = service;

            _context = new ConversationContext();

            _sentiment = new SentimentAnalyzer();

            _memory = new MemoryManager();

            _followUp = new FollowUpHandler();
        }

        public string ProcessMessage(string input)
        {
            // ===================================
            // ASK USER NAME FIRST
            // ===================================

            if (_context.WaitingForName)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    return "Hello! What is your name?";
                }

                // ===================================
                // CLEAN USER NAME (COMPATIBLE VERSION)
                // ===================================

                string cleanName = input.Trim();
                string lowerName = cleanName.ToLower();

                if (lowerName.StartsWith("my name is"))
                {
                    cleanName = cleanName.Substring(10).Trim();
                }
                else if (lowerName.StartsWith("i am"))
                {
                    cleanName = cleanName.Substring(4).Trim();
                }
                else if (lowerName.StartsWith("i'm"))
                {
                    cleanName = cleanName.Substring(3).Trim();
                }

                _context.UserName = cleanName;
                _context.WaitingForName = false;

                return $"Nice to meet you, {_context.UserName}! " +
                       $"I'm your Cybersecurity Assistant. How can I help you today?";
            }

            string lowerInput = input.ToLower();

            // ===================================
            // MEMORY SYSTEM
            // ===================================

            if (lowerInput.Contains("i like")
                || lowerInput.Contains("i am interested in")
                || lowerInput.Contains("my favorite topic is"))
            {
                string topic = lowerInput
                    .Replace("i like", "")
                    .Replace("i am interested in", "")
                    .Replace("my favorite topic is", "")
                    .Trim();

                _memory.SaveFavoriteTopic(_context, topic);

                return $"Great {_context.UserName}! I'll remember that you're interested in {topic}.";
            }

            // ===================================
            // SENTIMENT DETECTION
            // ===================================

            string mood = _sentiment.DetectSentiment(lowerInput);

            if (mood == "worried")
            {
                return $"{_context.UserName}, it’s understandable to feel worried about online threats. Let me help you stay safe with cybersecurity tips.";
            }

            if (mood == "frustrated")
            {
                return $"I understand {_context.UserName}. Cybersecurity can feel overwhelming sometimes, but small safety habits help a lot.";
            }

            if (mood == "curious")
            {
                return $"That’s great, {_context.UserName}! Curiosity helps you stay safe online.";
            }

            // ===================================
            // FOLLOW-UP HANDLING
            // ===================================

            if (_followUp.IsFollowUp(lowerInput))
            {
                _context.TopicDepth++;

                return _service.GetFollowUpResponse(_context.LastTopic);
            }

            // ===================================
            // SAVE LAST TOPIC
            // ===================================

            _context.LastTopic = input;

            string response = _service.GetResponse(input);

            // ===================================
            // PERSONALIZED MEMORY
            // ===================================

            if (!string.IsNullOrWhiteSpace(_context.FavoriteTopic))
            {
                response +=
                    $"\n\nSince you're interested in {_context.FavoriteTopic}, remember to stay safe online, {_context.UserName}.";
            }

            return response;
        }
    }
}