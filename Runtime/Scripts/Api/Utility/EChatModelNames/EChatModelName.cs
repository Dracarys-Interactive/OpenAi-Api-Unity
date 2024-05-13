using System;

namespace OpenAi.Api
{
    public static class UTEChatModelName
    {
        public static string GetModelName(EEngineName name)
        {
            switch (name)
            {
                case EEngineName.gpt_35_turbo:
                    return UTModelNames.gpt_35_turbo;
                case EEngineName.gpt_4_turbo:
                    return UTModelNames.gpt_4_turbo;
                case EEngineName.gpt_4:
                    return UTModelNames.gpt_4;
                case EEngineName.gpt_4o:
                    return UTModelNames.gpt_4o;
            }

            throw new ArgumentException($"Invalid enum value provided when getting chat model name. Value provided: {name}");
        }
    }
}