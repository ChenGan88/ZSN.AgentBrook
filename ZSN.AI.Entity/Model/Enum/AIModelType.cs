using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ZSN.Utils.Core.Helpers;
using static log4net.Appender.ColoredConsoleAppender;

namespace ZSN.AI.Entity.Model.Enum
{
    /// <summary>
    /// AI类型
    /// </summary>
    public enum AIType
    {
        
        [Display(Name = "Open AI")]
        OpenAI = 1,
        /*
        [Display(Name = "Azure Open AI")]
        AzureOpenAI = 2,

        [Display(Name = "星火大模型")]
        SparkDesk = 4,

        [Display(Name = "LLamaFactory")]
        LLamaFactory = 6,

        [Display(Name = "Bge Embedding")]
        BgeEmbedding = 7,

        [Display(Name = "Bge Rerank")]
        BgeRerank = 8,

        [Display(Name = "StableDiffusion")]
        StableDiffusion = 9,
        */

        [Display(Name = "Ollama")]
        Ollama = 10,

        [Display(Name = "Ollama Embedding")]
        OllamaEmbedding = 11,

        [Display(Name = "Ollama Rerank")]
        OllamaRerank = 12,

        [Display(Name = "模拟输出")]
        Mock = 100,

    }
    public partial class AIOrganization
    {
        public AIOrganization() { }

        public int ID { get; set; }
        public string Name { get; set; }
    }
    public partial class AIOrganizationList
    {
        public AIOrganizationList()
        {
        }
        public List<AIOrganization> List()
        {
            return AIType.GetValues(typeof(AIType)).Cast<AIType>()
            .Select(e =>
            {
                var fieldInfo = typeof(AIType).GetField(e.ToString());
                var displayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();
                return new AIOrganization
                {
                    Name = displayAttribute != null ? displayAttribute.Name : e.ToString(),
                    ID = (int)e
                };
            })
            .ToList();
        }
    }

    
    /// <summary>
    /// 模型类型
    /// </summary>
    public enum AIModelType
    {
        [Display(Name = "会话模型")]
        Chat = 1,

        [Display(Name = "向量模型")]
        Embedding = 2,

        [Display(Name = "图片模型")]
        Image =3,

        [Display(Name = "排序模型")]
        Rerank =4
    }
    public partial class ModelType
    {
        public ModelType() { }

        public int ID { get; set; }
        public string Name { get; set; }
    }

    public partial class AIModelTypeList
    {
        public AIModelTypeList()
        {
        }
        public List<ModelType> List()
        {
            return AIModelType.GetValues(typeof(AIModelType)).Cast<AIModelType>()
            .Select(e =>
            {
                var fieldInfo = typeof(AIModelType).GetField(e.ToString());
                var displayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();
                return new ModelType
                {
                    Name = displayAttribute != null ? displayAttribute.Name : e.ToString(),
                    ID = (int)e
                };
            })
            .ToList();
        }
    }

}
