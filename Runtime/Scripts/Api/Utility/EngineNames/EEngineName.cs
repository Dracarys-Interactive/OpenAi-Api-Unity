using System;

namespace OpenAi.Api
{
    public enum EEngineName
    {
        // GPT-3
        ada,
        babbage,
        curie,
        davinci,
        text_ada_001,
        text_babbage_001,
        text_curie_001,
        // GPT 3-5
        gpt_35_turbo,
        text_davinci_003,
        text_davinci_002,
        code_davinci_002,
        // GPT-4
        gpt_4_turbo,
        gpt_4,
    }

    public static class UTEEngineName
    {
        public static string GetEngineName(EEngineName name)
        {
            switch (name)
            {
                case EEngineName.ada:
                    return UTModelNames.ada;
                case EEngineName.babbage:
                    return UTModelNames.babbage;
                case EEngineName.curie:
                    return UTModelNames.curie;
                case EEngineName.davinci:
                    return UTModelNames.davinci;
                case EEngineName.text_ada_001:
                    return UTModelNames.text_ada_001;
                case EEngineName.text_babbage_001:
                    return UTModelNames.text_babbage_001;
                case EEngineName.text_curie_001:
                    return UTModelNames.text_curie_001;
                case EEngineName.gpt_35_turbo:
                    return UTModelNames.gpt_35_turbo;
                case EEngineName.text_davinci_003:
                    return UTModelNames.text_davinci_003;
                case EEngineName.text_davinci_002:
                    return UTModelNames.text_davinci_002;
                case EEngineName.code_davinci_002:
                    return UTModelNames.code_davinci_002;
                case EEngineName.gpt_4_turbo:
                    return UTModelNames.gpt_4_turbo;
                case EEngineName.gpt_4:
                    return UTModelNames.gpt_4;
            }

            throw new ArgumentException($"Invalid enum value provided when getting model name. Value provided: {name}");
        }
    }
}