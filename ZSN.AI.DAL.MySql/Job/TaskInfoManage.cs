using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text;
using ZSN.AI.Entity;
using ZSN.AI.DAL;
using Newtonsoft.Json;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
namespace ZSN.AI.DAL.MySql
{
    public partial class TaskInfoManage : ITaskInfoManage
    {
        ///表链接
        private string TaskInfoConnectionName = "JobDb";
        ///表名称
        private const string TaskInfoTableName = "tb_task_info";
        ///表字段
        private const string TaskInfoTableField = "TaskID,TaskType,TaskConfig,CreateTime,UpdateTime,State,Results,LoopType,IntervalValue,RepeatValue,RedoCount,FromTaskID,FromMainTaskID ";
        ///添加用表字段
        private const string TaskInfoTableFieldForAdd = "TaskID,TaskType,TaskConfig,CreateTime,UpdateTime,State,Results,LoopType,IntervalValue,RepeatValue,RedoCount,FromTaskID,FromMainTaskID";
        ///添加用表字段value
        private const string TaskInfoTableFieldAltForAdd = "@TaskID,@TaskType,@TaskConfig,@CreateTime,@UpdateTime,@State,@Results,@LoopType,@IntervalValue,@RepeatValue,@RedoCount,@FromTaskID,@FromMainTaskID";
        public string SetConnectionName(string connName)
        {
            return TaskInfoConnectionName = connName;
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string TaskInfo_Add(TaskInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ");
            strSql.Append(TaskInfoTableName);
            strSql.Append(" (");
            strSql.Append(TaskInfoTableFieldForAdd);
            strSql.Append(") values (");
            strSql.Append(TaskInfoTableFieldAltForAdd);
            strSql.Append(")");
            strSql.Append(";select @@IDENTITY");
            MySqlParameter[] parameters = {
             new MySqlParameter("@TaskID", MySqlDbType.VarChar,64),
 new MySqlParameter("@TaskType", MySqlDbType.Int32),
 new MySqlParameter("@TaskConfig", MySqlDbType.JSON),
 new MySqlParameter("@CreateTime", MySqlDbType.DateTime,16),
 new MySqlParameter("@UpdateTime", MySqlDbType.DateTime,16),
 new MySqlParameter("@State", MySqlDbType.Int32),
 new MySqlParameter("@Results", MySqlDbType.JSON),

 new MySqlParameter("@LoopType", MySqlDbType.Int32),
 new MySqlParameter("@IntervalValue", MySqlDbType.JSON),
 new MySqlParameter("@RepeatValue", MySqlDbType.Int32),
 new MySqlParameter("@RedoCount", MySqlDbType.Int32),
             new MySqlParameter("@FromTaskID", MySqlDbType.VarChar,64),
             new MySqlParameter("@FromMainTaskID", MySqlDbType.VarChar,64),

                    };
            parameters[0].Value = model.TaskID;
            parameters[1].Value = (int)model.TaskType;
            parameters[2].Value = JsonConvert.SerializeObject(model.TaskConfig);
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.UpdateTime;
            parameters[5].Value = model.State;
            parameters[6].Value = JsonConvert.SerializeObject(model.Results);


            parameters[7].Value = model.LoopType;
            parameters[8].Value = JsonConvert.SerializeObject(model.IntervalValue);
            parameters[9].Value = model.RepeatValue;
            parameters[10].Value = model.RedoCount;
            parameters[11].Value = model.FromTaskID;
            parameters[12].Value = model.FromMainTaskID;

            object obj = DbHelper.ExecuteScalar(DbConfig.GetDbInfo(TaskInfoConnectionName), CommandType.Text, strSql.ToString(), parameters);
            if (obj == null)
            {
                return String.Empty;
            }
            else
            {
                return model.TaskID;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool TaskInfo_Update(TaskInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ");
            strSql.Append(TaskInfoTableName);
            strSql.Append(" set ");
			strSql.Append("TaskType=@TaskType,");
strSql.Append("TaskConfig=@TaskConfig,");
strSql.Append("CreateTime=@CreateTime,");
strSql.Append("UpdateTime=@UpdateTime,");
strSql.Append("State=@State,");
strSql.Append("Results=@Results,");
            strSql.Append("LoopType=@LoopType,");
            strSql.Append("IntervalValue=@IntervalValue,");
            strSql.Append("RepeatValue=@RepeatValue,");
            strSql.Append("RedoCount=@RedoCount, ");
            strSql.Append("FromTaskID=@FromTaskID, ");
            strSql.Append("FromMainTaskID=@FromMainTaskID ");

            strSql.Append(" where TaskID=@TaskID");
            MySqlParameter[] parameters = {
				 new MySqlParameter("@TaskID", MySqlDbType.VarChar,64),
 new MySqlParameter("@TaskType", MySqlDbType.Int32,10),
 new MySqlParameter("@TaskConfig", MySqlDbType.JSON),
 new MySqlParameter("@CreateTime", MySqlDbType.DateTime,16),
 new MySqlParameter("@UpdateTime", MySqlDbType.DateTime,16),
 new MySqlParameter("@State", MySqlDbType.Int32,10),
 new MySqlParameter("@Results", MySqlDbType.JSON),

 new MySqlParameter("@LoopType", MySqlDbType.Int32),
 new MySqlParameter("@IntervalValue", MySqlDbType.JSON),
 new MySqlParameter("@RepeatValue", MySqlDbType.Int32),
 new MySqlParameter("@RedoCount", MySqlDbType.Int32),
                 new MySqlParameter("@FromTaskID", MySqlDbType.VarChar,64),
                 new MySqlParameter("@FromMainTaskID", MySqlDbType.VarChar,64),

            };
			 parameters[0].Value = model.TaskID;
 parameters[1].Value = model.TaskType;
 parameters[2].Value = JsonConvert.SerializeObject(model.TaskConfig);
 parameters[3].Value = model.CreateTime;
 parameters[4].Value = model.UpdateTime;
 parameters[5].Value = model.State;
 parameters[6].Value = JsonConvert.SerializeObject(model.Results);

            parameters[7].Value = model.LoopType;
            parameters[8].Value = JsonConvert.SerializeObject(model.IntervalValue);
            parameters[9].Value = model.RepeatValue;
            parameters[10].Value = model.RedoCount;
            parameters[11].Value = model.FromTaskID;
            parameters[12].Value = model.FromMainTaskID;

            int rows = DbHelper.ExecuteNonQuery(DbConfig.GetDbInfo(TaskInfoConnectionName),CommandType.Text,strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TaskInfo_Update(string taskID, TaskState state, Results results)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ");
            strSql.Append(TaskInfoTableName);
            strSql.Append(" set ");

            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("State=@State,");
            strSql.Append("Results=@Results");

            strSql.Append(" where TaskID=@TaskID");
            MySqlParameter[] parameters = {
                 new MySqlParameter("@TaskID", MySqlDbType.VarChar,64),
 new MySqlParameter("@UpdateTime", MySqlDbType.DateTime,16),
 new MySqlParameter("@State", MySqlDbType.Int32),
 new MySqlParameter("@Results", MySqlDbType.JSON),
            };
            parameters[0].Value = taskID;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = state;
            parameters[3].Value = JsonConvert.SerializeObject(results);

            int rows = DbHelper.ExecuteNonQuery(DbConfig.GetDbInfo(TaskInfoConnectionName), CommandType.Text, strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool TaskInfo_Delete(string taskID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ");
            strSql.Append(TaskInfoTableName);
            strSql.Append(" where TaskID=@TaskID");
            MySqlParameter[] parameters = {
					new MySqlParameter("@TaskID", MySqlDbType.VarChar, 64)
			};
            parameters[0].Value = taskID;
            int rows = DbHelper.ExecuteNonQuery(DbConfig.GetDbInfo(TaskInfoConnectionName), CommandType.Text,strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool TaskInfo_DeleteList(string taskIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ");
            strSql.Append(TaskInfoTableName);
            strSql.Append(" where TaskID in (" + taskIDlist + ")  ");
            int rows = DbHelper.ExecuteNonQuery(DbConfig.GetDbInfo(TaskInfoConnectionName), CommandType.Text,strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public TaskInfo TaskInfo_GetModel(string taskID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            strSql.Append(TaskInfoTableField);
            strSql.Append(" from ");
            strSql.Append(TaskInfoTableName);
            strSql.Append(" where TaskID=@TaskID");
            strSql.Append(" limit 1");
            MySqlParameter[] parameters = {
					new MySqlParameter("@TaskID", MySqlDbType.VarChar, 64)
			};
            parameters[0].Value = taskID;
            TaskInfo model = new TaskInfo();
            DataSet ds = DbHelper.ExecuteDataset(DbConfig.GetDbInfo(TaskInfoConnectionName), CommandType.Text,strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return TaskInfo_DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        public TaskInfo TaskInfo_GetModelByFromTaskID(string FromTaskID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            strSql.Append(TaskInfoTableField);
            strSql.Append(" from ");
            strSql.Append(TaskInfoTableName);
            strSql.Append(" where FromTaskID=@FromTaskID");
            strSql.Append(" limit 1");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@FromTaskID", MySqlDbType.VarChar, 64)
            };
            parameters[0].Value = FromTaskID;
            TaskInfo model = new TaskInfo();
            DataSet ds = DbHelper.ExecuteDataset(DbConfig.GetDbInfo(TaskInfoConnectionName), CommandType.Text, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return TaskInfo_DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public TaskInfo TaskInfo_DataRowToModel(DataRow row)
        {
            TaskInfo model = new TaskInfo();
            if (row != null)
            {
				if (row["TaskID"] != null )
                {
					model.TaskID = row["TaskID"].ToString();
                }
				if (row["TaskType"] != null )
                {
                        model.TaskType = (NodeType)int.Parse(row["TaskType"].ToString());
                }
				if (row["TaskConfig"] != null )
                {
					model.TaskConfig = JsonConvert.DeserializeObject<TaskConfig>(row["TaskConfig"].ToString());
                }
                if (row["LoopType"] != null)
                {
                    model.LoopType = (LoopType)int.Parse(row["LoopType"].ToString());
                }
                if (row["IntervalValue"] != null)
                {
                    model.IntervalValue = JsonConvert.DeserializeObject<IntervalValue>(row["IntervalValue"].ToString());
                }
                if (row["RepeatValue"] != null)
                {
                    model.RepeatValue = int.Parse(row["RepeatValue"].ToString());
                }
                if (row["RedoCount"] != null)
                {
                    model.RedoCount = int.Parse(row["RedoCount"].ToString());
                }
                if (row["CreateTime"] != null )
                {
					model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
				if (row["UpdateTime"] != null )
                {
					model.UpdateTime = DateTime.Parse(row["UpdateTime"].ToString());
                }
				if (row["State"] != null )
                {
                        model.State = (TaskState)int.Parse(row["State"].ToString());
                }
				if (row["Results"] != null )
                {
					model.Results = JsonConvert.DeserializeObject<Results>(row["Results"].ToString());
                }
                if (row["FromTaskID"] != null)
                {
                    model.FromTaskID = row["FromTaskID"].ToString();
                }
                if (row["FromMainTaskID"] != null)
                {
                    model.FromMainTaskID = row["FromMainTaskID"].ToString();
                }
            }
            return model;
        }

        public DataSet TaskInfo_GetList(int State, int taskType,DateTime StartTime, int ToState, int length)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            strSql.Append(TaskInfoTableField);
            strSql.Append(" FROM ");
            strSql.Append(TaskInfoTableName);

            string _where = " where State=@State and TaskType=@TaskType and CreateTime<=@StartTime order by CreateTime ASC limit @length";

            strSql.Append(_where + " ;");
            strSql.Append(" update " + TaskInfoTableName + " set State=@ToState " + _where + " ;");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@State", MySqlDbType.Int32),
                    new MySqlParameter("@TaskType",MySqlDbType.Int32),
                    new MySqlParameter("@ToState",MySqlDbType.Int32),
                    new MySqlParameter("@StartTime",MySqlDbType.DateTime,16),
                    new MySqlParameter("@length",MySqlDbType.Int32),
            };
            parameters[0].Value = State; 
            parameters[1].Value = taskType; 
            parameters[2].Value = ToState;
            parameters[3].Value = StartTime;
            parameters[4].Value = length;

            return DbHelper.ExecuteDataset(DbConfig.GetDbInfo(TaskInfoConnectionName), CommandType.Text, strSql.ToString(), parameters);
        }
        public DataSet TaskInfo_GetList(int State, string taskTypeStr, DateTime StartTime, int ToState, int length)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            strSql.Append(TaskInfoTableField);
            strSql.Append(" FROM ");
            strSql.Append(TaskInfoTableName);

            string _where = " where State=@State and TaskType in("+ taskTypeStr + ") and CreateTime<=@StartTime order by CreateTime ASC limit @length";

            strSql.Append(_where + " ;");
            strSql.Append(" update " + TaskInfoTableName + " set State=@ToState " + _where + " ;");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@State", MySqlDbType.Int32),
                    new MySqlParameter("@ToState",MySqlDbType.Int32),
                    new MySqlParameter("@StartTime",MySqlDbType.DateTime,16),
                    new MySqlParameter("@length",MySqlDbType.Int32),
            };
            parameters[0].Value = State;
            parameters[1].Value = ToState;
            parameters[2].Value = StartTime;
            parameters[3].Value = length;

            return DbHelper.ExecuteDataset(DbConfig.GetDbInfo(TaskInfoConnectionName), CommandType.Text, strSql.ToString(), parameters);
        }

        public bool TaskInfo_SetState(List<string> TaskID,int ToState)
        {
            bool re = true;
            if (TaskID.Count > 0)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" update " + TaskInfoTableName + " set State="+ ToState + "  where TaskID in("+ String.Join(",", TaskID.Select(n => $"'{n}'")) +");");

                int rows = DbHelper.ExecuteNonQuery(DbConfig.GetDbInfo(TaskInfoConnectionName), CommandType.Text, strSql.ToString());
                re = rows > 0;
            }

            return re;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet TaskInfo_GetList(string strWhere = "")
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            strSql.Append(TaskInfoTableField);
            strSql.Append(" FROM ");
            strSql.Append(TaskInfoTableName);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelper.ExecuteDataset(DbConfig.GetDbInfo(TaskInfoConnectionName), CommandType.Text,strSql.ToString());
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet TaskInfo_GetList(int top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            strSql.Append(TaskInfoTableField);
            strSql.Append(" FROM ");
            strSql.Append(TaskInfoTableName);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            if (filedOrder.Trim() != "")
            {
                strSql.Append(" order by " + filedOrder);
            }
            if (top > 0)
            {
                strSql.Append(" limit " + top.ToString() + " ");
            }
            return DbHelper.ExecuteDataset(DbConfig.GetDbInfo(TaskInfoConnectionName),CommandType.Text,strSql.ToString());
        }
        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int TaskInfo_GetRecordCount(string strWhere = "")
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM ");
            strSql.Append(TaskInfoTableName);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelper.ExecuteScalar(DbConfig.GetDbInfo(TaskInfoConnectionName),CommandType.Text,strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet TaskInfo_GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            strSql.Append(TaskInfoTableField);
            strSql.Append(" FROM ");
            strSql.Append(TaskInfoTableName);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + orderby);
            if (startIndex >= 0 && endIndex >= startIndex)
            {
                strSql.Append(" limit " + startIndex + ", " + (endIndex - startIndex));
            }
            return DbHelper.ExecuteDataset(DbConfig.GetDbInfo(TaskInfoConnectionName),CommandType.Text,strSql.ToString());
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">页标</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="pagetotal">总页数</param>
        /// <param name="total">总数</param>
        /// <param name="orderType">排序规则， 默认降序，1降序，0升序</param>
        /// <param name="showName">显示字段，默认全部</param>
        /// <param name="orderKey">排序key，默认主键</param>
        public DataTable TaskInfo_GetListByPage(int pageSize, int pageIndex, string strWhere, out int pagetotal, out int total, int orderType = 1, string showName = "*", string orderKey = "TaskID")
        {
            MySqlParameter[] parameters = {
                    new MySqlParameter("@tableName", MySqlDbType.VarChar, 255),
                    new MySqlParameter("@showFName", MySqlDbType.VarChar, 500),
                    new MySqlParameter("@selectWhere", MySqlDbType.VarChar, 500),
                    new MySqlParameter("@selectOrder", MySqlDbType.VarChar, 500),
                    new MySqlParameter("@pageNo", MySqlDbType.Int32),
                    new MySqlParameter("@pageSize", MySqlDbType.Int32)
            };
            parameters[0].Value = "tb_task_info";
            parameters[1].Value = showName;
            parameters[2].Value = strWhere;
            parameters[3].Value = orderKey + (orderType == 0 ? " ASC" : " DESC");
            parameters[4].Value = pageIndex;
            parameters[5].Value = pageSize;
            DataSet ds = DbHelper.ExecuteDataset(DbConfig.GetDbInfo(TaskInfoConnectionName),CommandType.StoredProcedure, "CommonPagenation", parameters);
            total = 0;
            if (ds.Tables.Count > 1)
            {
                total = Convert.ToInt32((long)ds.Tables[1].Rows[0][0]);
                if (total % pageSize == 0)
                {
                    pagetotal = total / pageSize;
                }
                else
                {
                    pagetotal = total / pageSize + 1;
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
