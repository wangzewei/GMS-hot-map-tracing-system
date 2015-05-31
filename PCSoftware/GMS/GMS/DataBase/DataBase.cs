using System;
using System.Collections.Generic;
using System.Text;

using System.Configuration;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.OracleClient;

namespace GMS
{
    public abstract class DataBase
    {
        /*****************************************************
        * summary：抽象产品类，提供抽象方法。
        * date:2015-4-24
        * author:李捞扒
        * last modify:2015-4-25
        * 
        * IDataReader和IDataParameter代替了泛型的使用
        ****************************************************/
        /// <summary>
        /// 将所有错误信息汇总到该字符串中
        /// </summary>
        public string ErrorString;

        /// <summary>
        /// 将ErrorString初始化为空
        /// </summary>
        public DataBase()
        {
            ErrorString = "";
        }


        #region 查询的相关方法
        //传入完整的sql语句，执行查询操作，返回的是IDataReader
        public abstract IDataReader ExecuteReader(string SqlString);
        //传入的sql语句和sql语句的参数，执行查询操作，返回的是IDataReader
        public abstract IDataReader ExecuteReader(string SqlString, params IDataParameter[] parms);



        //传入完整的sql语句，执行查询操作，返回的是DataTable
        public abstract DataTable ExecuteDataTable(string SqlString);
        //传入的sql语句和sql语句的参数，执行查询操作，返回的是DataTable
        public abstract DataTable ExecuteDataTable(string SqlString, params IDataParameter[] parms);



        //传入完整的sql语句，执行查询操作，返回的是DataSet
        public abstract DataSet ExecuteDataSet(string SqlString);
        //传入的sql语句和sql语句的参数，执行查询操作，返回的是DataSet
        public abstract DataSet ExecuteDataSet(string SqlString, params IDataParameter[] parms);



        //传入完整的sql语句、行数（从1开始），执行查询操作，返回的是DataRow
        public abstract DataRow ExecuteDataTableRow(string SqlString, int row);
        //传入的sql语句、行数（从1开始）和sql语句的参数，执行查询操作，返回的是DataRow
        public abstract DataRow ExecuteDataTableRow(string SqlString, int row, params IDataParameter[] parms);



        //传入完整的sql语句，执行查询操作，返回的是首行的首列值（Object型）
        public abstract object ExecuteScalar(string SqlString);
        //传入的sql语句和sql语句的参数，执行查询操作，返回的是首行的首列值（Object型）
        public abstract object ExecuteScalar(string SqlString, params IDataParameter[] parms);



        //传入完整的sql语句，执行查询操作，返回的是第一个数据的内容（String型）
        public abstract string ExecuteFirst(string SqlString);
        //传入的sql语句和sql语句的参数，执行查询操作，返回的是第一个数据的内容（String型）
        public abstract string ExecuteFirst(string SqlString, params IDataParameter[] parms);



        //传入完整的sql语句，执行查询操作，返回的是查询内容是否存在
        public abstract bool ExecuteExists(string SqlString);
        //传入的sql语句和sql语句的参数，执行查询操作，返回的是查询内容是否存在
        public abstract bool ExecuteExists(string SqlString, params IDataParameter[] parms);
        #endregion



        #region 修改，插入，删除
        //传入完整的sql语句，执行修改，插入，删除操作，返回的是受影响行数
        public abstract int ExecuteNonQuery(string SqlString);
        //传入的sql语句和sql语句的参数，执行修改，插入，删除操作，返回的是受影响行数
        public abstract int ExecuteNonQuery(string SqlString, params IDataParameter[] parms);
        #endregion
    }
}
