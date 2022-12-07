using System;

namespace SimAI.Core.SpeechToText {
    public class AmazonTranscribeClient : IsSpeechToTextProvider {
        private string _bucketUri = "https://s3.eu-west-1.amazonaws.com/{0}/{1}";
        private string _bucketName;

        public void Test() {

            //var client = new AmazonTranscribeServiceClient();
            //client.StartTranscriptionJobAsync(new StartTranscriptionJobRequest{})
        }
    }
}