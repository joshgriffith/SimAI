using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using SimAI.Core.Automation;
using SimAI.Core.Entities;
using SimAI.Core.Intent;
using SimAI.Core.Intent.Dialects;
using SimAI.Core.OpenAI;
using SimAI.Core.ResponseHandlers;
using SimAI.Core.Runtime;
using SimAI.Core.Skills;
using SimAI.Core.TextToSpeech;
using SimAI.Core.Tokenization;
using SimAI.Knowledge.Skills;

namespace SimAI.Test.Utilities {
    public static class TestHelper {
        private static IsTokenizer _tokenizer;
        public static IsTokenizer Tokenizer => _tokenizer ??= new CatalystTokenizer();

        public static OpenAIProvider GetProvider() {
            var container = new RuntimeContainer();

            var client = new OpenAIClient("APIKEY");
            var database = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            return new OpenAIProvider(client, database, container.Get<ILogger<OpenAIProvider>>());
        }

        public static IntentRouter GetMouseCursor() {
            //return null;
            var router = new IntentRouter(GetProvider(), new ImperativeIntentDialect(), null);
            var cursor = new MouseCursor();

            router.Use(cursor)
                .Route("move mouse to the center of the screen", mouse => mouse.MoveMouse(50, 50))
                .Route("move to the top left", data => data.MoveMouse(0, 0))
                .Route("move most of the way to the right", data => data.MoveMouse(80, 50))
                .Route("move to where the sun sets", data => data.MoveMouse(0, 50))
                .Route("move the mouse half way to the bottom left", data => data.MoveMouse(75, 25))
                .Route("move cursor 30% from the left and 80% from the top", data => data.MoveMouse(30, 80))
                .Route("move to the bottom right", data => data.MoveMouse(100, 100));

            return router;
        }
        
        public static Agent GetAgent() {
            var provider = GetProvider();
            var entityProvider = new EntityProvider();
            var agent = new Agent();

            agent.Use(provider);
            agent.Use(Tokenizer);
            agent.Use<IntentPlanner>();
            agent.Use(entityProvider);
            agent.Use<IntentRouter>();
            agent.Use<IntentResolver>();
            agent.Use<IntentSummarizer>();
            agent.Use<UberduckClient>();
            agent.Use(new UnknownRequestHandler());

            agent.Use(new SkillProvider()
                .Use(new MathSkills())
                .Use(new FileSystemSkills())
                .Use(new TimeSkills())
            );

            return agent;
        }
    }
}