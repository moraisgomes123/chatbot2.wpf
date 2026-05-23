using System;
using System.IO;
using NAudio.Wave;

namespace CybersecurityChatbot.Chatbot
{
    public class VoiceGreeting
    {
        public void PlayGreeting()
        {
            try
            {
                string path = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Audio",
                    "greeting.wav");

                if (!File.Exists(path))
                    return;

                using (var audioFile = new AudioFileReader(path))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);

                    outputDevice.Play();

                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(200);
                    }
                }
            }
            catch
            {
            }
        }
    }
}