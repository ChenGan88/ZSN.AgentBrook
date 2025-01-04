
using ZSN.AI.Entity;
using ZSN.AI.Entity.Model.KmsDetail;

namespace ZSN.AI.Entity.Model
{
    public class ImportKMSTaskDTO
    {

        public ImportType ImportType { get; set; }

        public string KmsId { get; set; }

        public string Url { get; set; } = "";


        public string Text { get; set; } = "";

        public string FilePath { get; set; } = "";

        public string FileName { get; set; } = "";

        public bool IsQA { get; set; } = false;
    }


    public class ImportKMSTaskReq : ImportKMSTaskDTO
    {
        public KnowledgeBaseFileInfo KnowledgeBaseFile { get; set; } = new KnowledgeBaseFileInfo();
    }

    public enum ImportType
    {
        File = 1,
        Url = 2,
        Text = 3,
        Excel=4
    }

    public class QAModel
    {
        public string ChatModelId { get; set; }
        public string Context { get; set; }
    }
}
