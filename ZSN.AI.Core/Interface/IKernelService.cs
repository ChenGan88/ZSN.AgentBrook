
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using ZSN.AI.Entity;
using ZSN.AI.Entity.Model;

namespace ZSN.AI.Core.Interface
{
    public interface IKernelService
    {
        Kernel GetKernel(LargeModelInfo ModelInfo);
        Kernel GetKernelByAIModelID(int modelid);
        void ImportFunctions(LargeModelConfig ModelConfig, Kernel _kernel);
        void ImportFunctions(Kernel _kernel, object type, string pluginName);
        Task<string> HistorySummarize(Kernel _kernel, ChatHistory history);
        Task<string> PromptFunctionCall(Kernel _kernel, CallFunction callFunction, KernelArguments parameter);
    }
}
