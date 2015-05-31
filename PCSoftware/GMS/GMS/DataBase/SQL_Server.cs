using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace GMS
{
    /*****************************************************
    * summary：继承抽象类，实现抽象方法
    * date:2015-4-1
    * author:李捞扒
    * last modify:2015-4-25
    * 
    ****************************************************/
    class SQL_Server : DataBase
    {
        /// <summary>
        /// 数据库连接串
        /// </summary>
        //private String ConnString = @"Data Source = .;Initial Catalog =iot;Integrated Security =SSPI";
        private String ConnString = ConfigurationManager.ConnectionStrings["sqlserverconnstr"].ToString();
        /// <summary>
        /// 数据库连接
        /// </summary>
        private SqlConnection Conn = null;
        /// <summary>
        /// 数据库操纵
        /// </summary>
        private SqlCommand cmd = null;
        /// <summary>
        /// 超时（秒）
        /// </summary>
        private int TimeOut = 100;




        #region 构造方法
        /// <summary>
        /// 默认的数据库链接
        /// </summary>
        public SQL_Server()
        {
            //这里采用默认连接串
            ConnTo();
        }
        /// <summary>
        /// 利用用户提供的连接串连接数据库
        /// </summary>
        /// <param name="ConnStr">数据库连接串</param>
        public SQL_Server(string ConnStr)
        {
            //利用用户提供的连接串连接数据库，连接串要符合格式要求
            ConnString = ConnStr;
            ConnTo();
        }
        #endregion




        #region 查询
        /// <summary>
        /// 执行sql查询语句，返回IDataReader
        /// </summary>
        /// <param name="SqlString">完整的sql语句</param>
        /// <returns>查询结果：IDataReader</returns>
        public override IDataReader ExecuteReader(string SqlString)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            SqlDataReader reader=null;
            try
            {
                CommandTo(SqlString);
                if (cmd != null)
                {
                    reader = cmd.ExecuteReader();
                    
                    return reader;
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return null;
        }
        /// <summary>
        ///  执行sql查询语句，返回IDataReader
        ///  </summary>
        /// <param name="SqlString">存在占位符的sql语句</param>
        ///  <param name="parms">变元是sql语句的参数</param>
        /// <returns>查询结果：IDataReader</returns>
        public override IDataReader ExecuteReader(string SqlString, params IDataParameter[] parms)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            SqlDataReader reader=null;
            try
            {
                CommandTo(SqlString);
                if ((parms != null) && (cmd != null))
                {
                    foreach (IDataParameter pram in parms)
                        cmd.Parameters.Add(pram);
                    reader = cmd.ExecuteReader();
                    return reader;
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return null;
        }  




        /// <summary>
        /// 执行sql查询语句，返回DataTable
        /// </summary>
        /// <param name="SqlString">完整的sql语句</param>
        /// <returns>查询结果：DataTable</returns>
        public override DataTable ExecuteDataTable(string SqlString)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            try
            {
                CommandTo(SqlString);
                DataTable dt = new DataTable();
                if (cmd != null)
                {
                    dt.Load(cmd.ExecuteReader());
                    return dt;
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            return null;
        }
        /// <summary>
        /// 执行sql查询语句，返回DataTable
        /// </summary>
        /// <param name="SqlString">存在占位符的sql语句</param>
        /// <param name="parms">变元是sql语句的参数</param>
        /// <returns>查询结果：DataTable</returns>
        public override DataTable ExecuteDataTable(string SqlString, params IDataParameter[] parms)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            try
            {
                CommandTo(SqlString);
                DataTable dt = new DataTable();
                if ((parms != null) && (cmd != null))
                {
                    foreach (IDataParameter pram in parms)
                        cmd.Parameters.Add(pram);
                    dt.Load(cmd.ExecuteReader());
                    return dt;
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            return null;
        }




        /// <summary>  
        /// 执行sql查询语句，返回DataSet   
        /// </summary>  
        /// <param name="SQLString">完整的sql语句</param>  
        /// <returns>返回DataSet </returns>  
        public override DataSet ExecuteDataSet(string SqlString)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            try
            {
                CommandTo(SqlString);
                DataSet ds = new DataSet();
                if (cmd != null)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds, "ds");
                    return ds;
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            return null;
        }
        /// <summary>  
        /// 执行sql查询语句，返回DataSet 
        /// </summary>  
        /// <param name="SQLString">存在占位符的sql语句</param>  
        /// <param name="parms">变元是sql语句的参数</param>
        /// <returns>返回DataSet</returns>  
        public override DataSet ExecuteDataSet(string SqlString, params IDataParameter[] parms)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            try
            {
                CommandTo(SqlString);
                DataSet ds = new DataSet();
                if ((parms != null) && (cmd != null))
                {
                    foreach (IDataParameter pram in parms)
                        cmd.Parameters.Add(pram);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds, "ds");
                    cmd.Parameters.Clear();
                    return ds;
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            return null;
        }




        /// <summary>
        /// 执行sql查询语句，返回某一行数据
        /// </summary>
        /// <param name="SqlString">完整的sql语句</param>
        /// <returns>查询结果：返回某一行数据</returns>
        public override DataRow ExecuteDataTableRow(string SqlString,int row)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            try
            {
                CommandTo(SqlString);
                if (cmd != null)
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Rows.Count > 0 && dt.Rows.Count > row)
                        return dt.Rows[row-1];
                    else
                        AddError("查询行超出总行数", SqlString);
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            return null;
        }
        /// <summary>
        /// 执行sql查询语句，返回某一行数据
        /// </summary>
        /// <param name="SqlString">存在占位符的sql语句</param>
        /// <param name="parms">变元是sql语句的参数</param>
        /// <returns>查询结果：返回某一行数据</returns>
        public override DataRow ExecuteDataTableRow(string SqlString, int row, params IDataParameter[] parms)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            try
            {
                CommandTo(SqlString);
                DataTable dt = new DataTable();
                if ((parms != null) && (cmd != null))
                {
                    foreach (IDataParameter pram in parms)
                        cmd.Parameters.Add(pram);
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Rows.Count > 0 && dt.Rows.Count > row)
                        return dt.Rows[row-1];
                    else
                        AddError("查询行超出总行数", SqlString);
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            return null;
        }




        /// <summary>  
        /// 执行sql查询语句，返回查询结果集中首行的首列值即最左上角的值（object）。  
        /// </summary>  
        /// <param name="sqlString">完整的sql语句</param>  
        /// <returns>返回查询结果集中首行的首列值（object）</returns>  
        public override object ExecuteScalar(string SqlString)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            try
            {
                CommandTo(SqlString);
                if (cmd != null)
                {
                    object obj = cmd.ExecuteScalar();
                    if (!(Object.Equals(obj, null)) && !(Object.Equals(obj, System.DBNull.Value)))
                        return obj;
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            return null;
        }
        /// <summary>  
        /// 执行sql查询语句，返回查询结果集中首行的首列值即最左上角的值（object）。  
        /// </summary>  
        /// <param name="sqlString">完整的sql语句</param> 
        /// <param name="parms">变元是sql语句的参数</param>
        /// <returns>返回查询结果集中首行的首列值即最左上角的值（object）</returns>  
        public override object ExecuteScalar(string SqlString, params IDataParameter[] parms)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            try
            {
                CommandTo(SqlString);
                if ((parms != null) && (cmd != null))
                {
                    foreach (IDataParameter pram in parms)
                        cmd.Parameters.Add(pram);
                    object obj = cmd.ExecuteScalar();
                    if (!(Object.Equals(obj, null)) && !(Object.Equals(obj, System.DBNull.Value)))
                        return obj;
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            return null;
        }




        /// <summary>
        /// 执行sql查询语句，返回第一个数据的内容
        /// </summary>
        /// <param name="SqlString">完整的sql语句</param>
        /// <returns>查询结果：返回第一个数据的内容</returns>
        public override string ExecuteFirst(string SqlString)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            SqlDataReader ss = null;
            try
            {
                CommandTo(SqlString);
                if (cmd != null)
                {
                    ss = cmd.ExecuteReader();
                    string xx = "";
                    if (ss.Read())
                        xx = ss[0].ToString();
                    return xx;
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            finally
            {
                if (ss != null)
                    ss.Close();
            }
            return null;
        }
        /// <summary>
        /// 执行sql查询语句，返回第一个数据的内容
        /// </summary>
        /// <param name="SqlString">存在占位符的sql语句</param>
        /// <param name="parms">变元是sql语句的参数</param>
        /// <returns>查询结果：返回第一个数据的内容</returns>
        public override string ExecuteFirst(string SqlString, params IDataParameter[] parms)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            SqlDataReader ss = null;
            try
            {
                CommandTo(SqlString);
                if ((parms != null) && (cmd != null))
                {
                    foreach (IDataParameter pram in parms)
                        cmd.Parameters.Add(pram);
                    ss = cmd.ExecuteReader();
                    string xx = "";
                    if (ss.Read())
                        xx = ss[0].ToString();
                    return xx;
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            finally
            {
                if (ss != null)
                    ss.Close();
            }
            return null;
        }




        /// <summary>
        /// 执行sql查询语句，判断查询内容是否存在,返回是否
        /// </summary>
        /// <param name="SqlString">完整的sql语句</param>
        /// <returns>是否存在</returns>
        public override bool ExecuteExists(string SqlString)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            try
            {
                CommandTo(SqlString);
                DataTable dt = new DataTable();
                if (cmd != null)
                {
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Rows.Count > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            return false;
        }
        /// <summary>
        /// 执行sql查询语句，判断查询内容是否存在,返回是否
        /// </summary>
        /// <param name="SqlString">存在占位符的sql语句</param>
        /// <param name="parms">变元是sql语句的参数</param>
        /// <returns>是否存在</returns>
        public override bool ExecuteExists(string SqlString, params IDataParameter[] parms)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            try
            {
                CommandTo(SqlString);
                DataTable dt = new DataTable();
                if ((parms != null) && (cmd != null))
                {
                    foreach (IDataParameter pram in parms)
                        cmd.Parameters.Add(pram);
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Rows.Count > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            return false;
        }
        #endregion




        #region 增加、删除、修改
        /// <summary>
        /// 执行sql插入、删除、修改语句，返回受影响的行数
        /// -1：代表操作失败，其他均表示受影响的行数
        /// 对于语句的执行是否成功，由调用的函数判断
        /// </summary>
        /// <param name="SqlString">完整的sql语句</param>
        ///<returns>返回受影响的行数</returns>
        public override int ExecuteNonQuery(string SqlString)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            try
            {
                CommandTo(SqlString);
                if (cmd != null)
                    return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            return -1;
        }
        /// <summary>
        /// 执行sql插入、删除、修改语句，返回受影响的行数
        /// -1：代表操作失败，其他均表示受影响的行数
        /// </summary>
        /// <param name="SqlString">存在占位符的sql语句</param>
        /// <param name="parms">变元是sql语句的参数</param>
        ///<returns>返回受影响的行数</returns>
        public override int ExecuteNonQuery(string SqlString, params IDataParameter[] parms)
        {
            if (Conn == null || Conn.State != ConnectionState.Open)
                ConnTo();
            try
            {
                CommandTo(SqlString);
                if ((parms != null) && (cmd != null))
                {
                    cmd.Parameters.AddRange(parms);
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
            return -1;
        }
        #endregion





        #region 数据库操纵
        /// <summary>
        /// 实例化数据库的连接对象
        /// </summary>
        private void ConnTo()
        {
            Close();
            try
            {
                Conn = new SqlConnection(ConnString);
                Conn.Open();
            }
            catch (Exception e)
            {
                base.ErrorString += "数据库连接错误：" + e.Message + "\r\n连接串：" + ConnString + "\r\n";
                if (!string.IsNullOrEmpty(base.ErrorString) && base.ErrorString.Length > 1000)
                    base.ErrorString = null;
            }
        }




        /// <summary>
        /// 实例化数据库的操作对象
        /// </summary>
        /// <param name="SqlString"></param>
        private void CommandTo(string SqlString)
        {
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = Conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = SqlString;
                cmd.CommandTimeout = TimeOut;
            }
            catch (Exception e)
            {
                AddError(e.Message, SqlString);
            }
        }




        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        private void Close()
        {
            try
            {
                Conn.Close();
                Conn = null;
            }
            catch (Exception e)
            {
                base.ErrorString += "数据库连接错误：" + e.Message + "\r\n连接串：" + ConnString + "\r\n";
                if (!string.IsNullOrEmpty(base.ErrorString) && base.ErrorString.Length > 1000)
                    base.ErrorString = null;
            }
        }




        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sql"></param>
        private void AddError(string message, string sql)
        {
            base.ErrorString += "数据库连接错误：" + message + "\r\nSQL语句：" + sql + "\r\n";
            if (!string.IsNullOrEmpty(base.ErrorString) && base.ErrorString.Length > 1000)
                base.ErrorString = "";
        }
        #endregion


    }
}
