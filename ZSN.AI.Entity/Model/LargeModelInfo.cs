using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ZSN.AI.Entity.Model.Enum;
namespace ZSN.AI.Entity
{
    /// <summary>
    /// tb_large_model_info
    /// </summary>
    public partial class LargeModelInfo
    {
        public LargeModelInfo() { }
        #region AutoField
        /// <summary>
        /// LargeModelID
        /// </summary>
        public Int32 LargeModelID { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string ModelKey { get; set; } = string.Empty;
        public string MICON { get; set; } = string.Empty;
        /// <summary>
        /// TypeCode
        /// </summary>
        public AIModelType TypeCode { get; set; } = 0;
        /// <summary>
        /// TypeName
        /// </summary>
        public string TypeName { get; set; } = string.Empty;

        public AIType ModelOrganizationID { get; set; } = 0;
        public string ModelOrganizationName { get; set; } = string.Empty;
        /// <summary>
        /// EndPoint
        /// </summary>
        public string EndPoint { get; set; } = string.Empty;
        public string MConfig { get; set; } = string.Empty;
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// SystemStatus
        /// </summary>
        public Int32 SystemStatus { get; set; } = (Int32)(0);
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// UpdateTime
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;



        #endregion
    }

    public partial class GptMsg
    {
        public GptMsg() { }
        public string role { get; set; } = string.Empty;
        public string content {  get; set; } = string.Empty;
    }
}
