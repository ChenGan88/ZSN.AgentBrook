using System;
using System.Data;
using Npgsql;
using System.Text;
using ZSN.AI.Entity;
using ZSN.AI.DAL;
using ZSN.Utils.Core.Data;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using NpgsqlTypes;
using NPOI.SS.Formula.Functions;

namespace ZSN.AI.DAL.Postgres
{
    public class KnowledgeBaseFileChunkInfoManage:IKnowledgeBaseFileChunkInfoManage
    {
        ///表链接
        private string ConnectionName = "KnowledgeBaseDb";
        ///表名称
        private string TableName = "xl-kms";
        ///表字段
        private const string TableField = "id,tags,content,payload";

        public string SetConnectionName(string connName)
        {
            return ConnectionName = connName;
        }
        public bool KnowledgeBaseFileChunkInfo_Delete(string FileID,string KnowledgeBaseID)
        {
            string _TableName = $"xl-{KnowledgeBaseID}kms";
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("DO $$");
            strSql.AppendLine("BEGIN");
            strSql.AppendLine($"IF EXISTS (SELECT 1 FROM pg_tables WHERE schemaname = 'public' AND tablename = '{_TableName}') THEN");
            strSql.Append("delete from ");
            strSql.Append($"\"{_TableName}\"");
            strSql.Append(" where id like 'd=" + FileID + "//%' ;");
            strSql.AppendLine("END IF;");
            strSql.AppendLine("END $$;");

            int rows = DbHelper.ExecuteNonQuery(DbConfig.GetDbInfo(ConnectionName), CommandType.Text, strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public KnowledgeBaseFileChunkInfo KnowledgeBaseFileChunkInfo_DataRowToModel(DataRow row)
        {
            KnowledgeBaseFileChunkInfo model = new KnowledgeBaseFileChunkInfo();
            if (row != null)
            {
                if (row["id"] != null)
                {
                    model.id = row["id"].ToString();
                }
                if (row["tags"] != null)
                {
                    model.tags = row["tags"].ToString();
                }
                if (row["content"] != null)
                {
                    model.content = row["content"].ToString();
                }
            }
            return model;
        }
        public DataTable KnowledgeBaseFileChunkInfo_GetListByPage(string KnowledgeBaseID,int size, int index, string where, out int pagetotal, out int total)
        {
            string _TableName = $"xl-{KnowledgeBaseID}kms";
            List<T> results = new List<T>();
            pagetotal = 0;
            total = 0;

            // 确保where子句以"WHERE "开头，如果不是，则添加
            where = string.IsNullOrWhiteSpace(where) ? string.Empty : "WHERE " + where;

            // 计算总记录数
            string countQuery = $"SELECT COUNT(*) FROM {_TableName} {where} ;";

            // 分页查询
            string query = $@"
            SELECT {TableField} FROM {_TableName}
            {where}
            ORDER BY id
            LIMIT {size} OFFSET {(index - 1) * size};"+ countQuery;

            DataSet ds = DbHelper.ExecuteDataset(DbConfig.GetDbInfo(ConnectionName), CommandType.Text, query);

            total = 0;
            if (ds.Tables.Count > 1)
            {
                total = Convert.ToInt32((long)ds.Tables[1].Rows[0][0]);
                if (total % size == 0)
                {
                    pagetotal = total / size;
                }
                else
                {
                    pagetotal = total / size + 1;
                }
                return ds.Tables[0];
            }
            else
            {
                pagetotal = 0;
                return null;
            }
        }
    }
}
