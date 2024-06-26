﻿using NUnit.Framework;

using OpenAi.Api.V1;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.TestTools;

namespace OpenAi.Api.Test
{
    public class V1PlayTests
    {
        private TestManager test;
        private OpenAiApiV1 api;

        [OneTimeSetUp]
        public void OneTimeSetup() => test = TestManager.Instance;

        [SetUp]
        public void SetUp() => api = test.CleanAndProvideApi();

        #region Engines Requests
        [UnityTest]
        public IEnumerator EnginesListAsync()
        {
            Task<ApiResult<EnginesListV1>> resTask = api.Engines.ListEnginesAsync();

            while (!resTask.IsCompleted) yield return new WaitForEndOfFrame();

            ApiResult<EnginesListV1> res = resTask.Result;

            if (!test.TestApiResultHasResponse(res)) Assert.That(false);

            bool isResultDataNotEmpty = res.Result.data != null && res.Result.data.Length > 0;
            test.LogTest("Result data is not empty", isResultDataNotEmpty);

            Assert.That(isResultDataNotEmpty);
        }
        #endregion

        #region Chat Completion Requests

        [UnityTest]
        public IEnumerator ChatCompletions_TestAllRequestParamsString()
        {
            ApiResult<ChatCompletionV1> result = null;

            MessageV1 message = new MessageV1();
            message.role = MessageV1.MessageRole.user;
            message.content = "hello";

            List<MessageV1> messages = new List<MessageV1>();
            messages.Add(message);

            ChatCompletionRequestV1 req = new ChatCompletionRequestV1()
            {
                model = "gpt-4-turbo",
                messages = messages,
                frequency_penalty = 0,
                presence_penalty = 0,
                logit_bias = new Dictionary<string, int>() { { "123", -100 }, { "111", 100 } },
                stop = "###",
                stream = false,
                max_tokens = 8,
                n = 1,
                temperature = 0,
                top_p = 1,
                user = ""
            };

            yield return api.Chat.Completions.CreateChatCompletionCoroutine(test, req, (r) => result = r);

            if (!test.TestApiResultHasResponse(result)) Assert.That(false);

            bool doesResultObjectExist = result.Result.choices != null && result.Result.choices.Length > 0;
            test.LogTest("Does non empty result object exist", doesResultObjectExist);

            Assert.That(doesResultObjectExist);
        }

        [UnityTest]
        public IEnumerator ChatCompletions_TestAllRequestParamsArray()
        {
            ApiResult<ChatCompletionV1> result = null;

            MessageV1 message = new MessageV1();
            message.role = MessageV1.MessageRole.user;
            message.content = "hello";

            List<MessageV1> messages = new List<MessageV1>();
            messages.Add(message);

            ChatCompletionRequestV1 req = new ChatCompletionRequestV1()
            {
                model = "gpt-4-turbo",
                messages = messages,
                frequency_penalty = 0,
                presence_penalty = 0,
                logit_bias = new Dictionary<string, int>() { { "123", -100 }, { "111", 100 } },
                stop = new string[] { "stop1", "stop2" },
                stream = false,
                max_tokens = 8,
                n = 1,
                temperature = 0,
                top_p = 1,
                user = ""
            };

            yield return api.Chat.Completions.CreateChatCompletionCoroutine(test, req, (r) => result = r);

            if (!test.TestApiResultHasResponse(result)) Assert.That(false);

            bool doesResultObjectExist = result.Result.choices != null && result.Result.choices.Length > 0;
            test.LogTest("Does non empty result object exist", doesResultObjectExist);

            Assert.That(doesResultObjectExist);
        }

        [UnityTest]
        public IEnumerator ChatCompletionsCreateCoroutine()
        {
            ApiResult<ChatCompletionV1> result = null;

            MessageV1 message = new MessageV1();
            message.role = MessageV1.MessageRole.user;
            message.content = "hello";

            List<MessageV1> messages = new List<MessageV1>();
            messages.Add(message);

            ChatCompletionRequestV1 req = new ChatCompletionRequestV1()
            {
                model = "gpt-4-turbo",
                messages = messages,
                n = 8
            };

            yield return api.Chat.Completions.CreateChatCompletionCoroutine(test, req, (r) => result = r);

            if (!test.TestApiResultHasResponse(result)) Assert.That(false);
            bool doesResultObjectExist = result.Result.choices != null && result.Result.choices.Length > 0;
            test.LogTest("Does non empty result object exist", doesResultObjectExist);

            Assert.That(doesResultObjectExist);
        }

        [UnityTest]
        public IEnumerator ChatCompletionsCreateAsync()
        {
            MessageV1 message = new MessageV1();
            message.role = MessageV1.MessageRole.user;
            message.content = "hello";

            List<MessageV1> messages = new List<MessageV1>();
            messages.Add(message);

            ChatCompletionRequestV1 req = new ChatCompletionRequestV1()
            {
                model = "gpt-4-turbo",
                messages = messages,
                max_tokens = 8
            };

            Task<ApiResult<ChatCompletionV1>> resTask = api.Chat.Completions.CreateChatCompletionAsync(req);

            while (!resTask.IsCompleted) yield return new WaitForEndOfFrame();

            ApiResult<ChatCompletionV1> res = resTask.Result;

            Assert.That(test.TestApiResultHasResponse(res));
        }

        [UnityTest]
        public IEnumerator ChatCompletionsCreateCoroutine_EventStream()
        {
            ApiResult<ChatCompletionV1> result = null;
            List<ChatCompletionV1> partials = new List<ChatCompletionV1>();
            bool isComplete = false;

            MessageV1 message = new MessageV1();
            message.role = MessageV1.MessageRole.user;
            message.content = "hello";

            List<MessageV1> messages = new List<MessageV1>();
            messages.Add(message);

            ChatCompletionRequestV1 req = new ChatCompletionRequestV1()
            {
                model = "gpt-4-turbo",
                messages = messages,
                max_tokens = 8
            };

            yield return api.Chat.Completions.CreateChatCompletionCoroutine_EventStream(
                test,
                req,
                (r) => result = r,
                (i, l) => partials.Add(l),
                () => isComplete = true
            );

            float timer = 10f;
            while (!isComplete && timer > 0)
            {
                timer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            test.LogTest("Stream was completed", isComplete);

            if (!test.TestApiResultSuccess(result)) Assert.That(false);

            bool partialsNotEmpty = partials != null && partials.Count > 0;
            test.LogTest("Partial reponses were received", partialsNotEmpty);

            Assert.That(isComplete && partialsNotEmpty);
        }

        [UnityTest]
        public IEnumerator ChatCompletionsCreateAsync_EventStream()
        {
            ApiResult<ChatCompletionV1> result = null;
            List<ChatCompletionV1> completions = new List<ChatCompletionV1>();
            bool isComplete = false;

            MessageV1 message = new MessageV1();
            message.role = MessageV1.MessageRole.user;
            message.content = "hello";

            List<MessageV1> messages = new List<MessageV1>();
            messages.Add(message);

            ChatCompletionRequestV1 req = new ChatCompletionRequestV1()
            {
                model = "gpt-4-turbo",
                messages = messages,
                max_tokens = 8,
                stream = true
            };

            Task engineTask = api.Chat.Completions.CreateChatCompletionAsync_EventStream(
                req,
                (r) => result = r,
                (i, c) => completions.Add(c),
                 () => isComplete = true
            );

            while (!engineTask.IsCompleted) yield return new WaitForEndOfFrame();

            test.LogTest("Stream was completed", isComplete);

            if (!test.TestApiResultSuccess(result)) Assert.That(false);

            bool completionsNotEmpty = completions != null && completions.Count > 0;
            test.LogTest("Partial reponses were received", completionsNotEmpty);

            Assert.That(isComplete && completionsNotEmpty);
        }

        [UnityTest]
        public IEnumerator ChatCompletionsCreateAsync_Multiple()
        {
            MessageV1 message = new MessageV1();
            message.role = MessageV1.MessageRole.system;
            message.content = "You are Yoda from Star Wars.";

            List<MessageV1> messages = new List<MessageV1>();
            messages.Add(message);

            message = new MessageV1();
            message.role = MessageV1.MessageRole.user;
            message.content = "Is Vader good or evil?";
            messages.Add(message);

            ChatCompletionRequestV1 req = new ChatCompletionRequestV1()
            {
                model = "gpt-4-turbo",
                messages = messages
            };

            Task<ApiResult<ChatCompletionV1>> resTask = api.Chat.Completions.CreateChatCompletionAsync(req);

            while (!resTask.IsCompleted) yield return new WaitForEndOfFrame();

            ApiResult<ChatCompletionV1> res = resTask.Result;

            Assert.That(test.TestApiResultHasResponse(res));
        }
        #endregion
    }
}

