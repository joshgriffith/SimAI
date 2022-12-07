using System;
using System.IO;
using System.Media;
using System.Threading.Tasks;

namespace SimAI.Core.TextToSpeech {
    public interface IsTextToSpeechProvider {
        Task<Stream> GetStreamAsync(string text, string voice);
    }

    public static class IsTextToSpeechProviderExtensions {
        public static async Task SpeakAsync(this IsTextToSpeechProvider provider, string speech, string voice = "glados") {
            await using var stream = await provider.GetStreamAsync(speech, voice);
            
            var player = new SoundPlayer(stream);
            player.PlaySync();
        }
    }
}