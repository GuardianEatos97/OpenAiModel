using Azure;
using Azure.AI.OpenAI;
using OpenAiModel.Configuration;
using OpenAiModel.Models;
using OpenAiModel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAiModel.Services
{
    public class YodaAiAssistant : IAiAssistant
    {
        private ISettings _settings;
        private const string AssistantBehaviorDescription = "Master Yoda I am. Advice, fun facts and jokes I do tell.";

        public YodaAiAssistant(ISettings settings)
        {
            _settings = settings;
        }

        private IList<ChatMessage> BuildChatContext(IList<YodaChatMessage> chatInboundHistory, YodaChatMessage userMessage)
        {
            var chatContext = new List<ChatMessage>();

            chatContext.Add(new ChatMessage(ChatRole.System, AssistantBehaviorDescription));

            foreach (var chatMessage in chatInboundHistory)
                chatContext.Add(new ChatMessage(ChatRole.User, chatMessage.MessageBody));

            chatContext.Add(new ChatMessage(ChatRole.User, userMessage.MessageBody));

            return chatContext;

        }

        public ChatMessage GetCompletion(IList<YodaChatMessage> chatInboundHistory, YodaChatMessage userMessage)
        {
            var messages = BuildChatContext(chatInboundHistory, userMessage);

            var client = new OpenAIClient(new Uri(_settings.AzureOpenAiEndPoint), new AzureKeyCredential(_settings.AzureOpenAiKey));
            string deploymentName = "masteryodaopenai";
            string searchIndex = "finder";
            string fact = "Give me a fun fact"; 

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages =
                    {
                        new ChatMessage (ChatRole.System,"You are an AI bot that emulates a Master Yoda writing assistant who speaks in a Yoda style. You offer advice, fun facts and tell jokes. \r\nHere are some example of Master Yoda's style:\r\n - Patience you must have my young Padawan.\r\n - In a dark place we find ourselves, and a little more knowledge lights our way.\r\n - Once you start down the dark path, forever will it dominate your destiny. Consume you, it will."),
                        new ChatMessage (ChatRole.User,"Greetings Young Padawan. Patience you must have, for answers I shall provide."),
                       /* new ChatMessage(ChatRole.System, "What do you seek?"),
                        new ChatMessage(ChatRole.User, "Give me a fun fact"),*/
                    }
            };

            foreach (var message in messages)
                chatCompletionsOptions.Messages.Add(message);

            Response<ChatCompletions> response = client.GetChatCompletions(deploymentName, chatCompletionsOptions);

            ChatMessage responseMessage = response.Value.Choices[0].Message;

            return responseMessage;
        }
    }
}
