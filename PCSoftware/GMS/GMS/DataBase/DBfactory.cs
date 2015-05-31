using System;
using System.Collections.Generic;
using System.Text;


using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace GMS
{
    /*****************************************************
    * summary：操作数据库的接口
    * date:2015-4-1
    * author:李捞扒
    * last modify:2015-4-25
    * 
    * 简单工厂模式。3种具体产品（Concrete_Product）泛化于抽象产品（Product),对应的三种具体构造者（ConcreteCreator）泛化于抽象构造者Creator),
    *               抽象构造者Creator)关联于抽象产品（Product）。客户端直接与具体构造者发生交互
    *               
    * 可改进:
    * 1：改为静态需添加static并且给各个方法添加关闭连接操作
    * 2：因为数据库的连接数有限制，所以最好关闭。但是如果关闭了，下次又要实例化，增加了时间成本。 
    * 
    *******************************************************/
    public class DBfactory
    {
        #region 返回具体数据库实例的静态方法
        //默认连接参数，获取SQL_Server数据库对象
        public static DataBase getSQL_ServerInst()
        {
            return new SQL_Server();
        }
        //用户自定义的数据库连接串，获取SQL_Server数据库对象
        public static DataBase getSQL_ServerInst(string connstr)
        {
            return new SQL_Server(connstr);
        }


        //默认连接参数，获取MySQL数据库对象
        public static DataBase getMySQLInst()
        {
            return new MySQL();
        }
        //用户自定义的数据库连接串，获取MySQL数据库对象
        public static DataBase getMySQLInst(string connstr)
        {
            return new MySQL(connstr);
        }


        //默认连接参数，获取Oracle数据库对象
        public static DataBase getOracleInst()
        {
            return new Oracle();
        }
        //用户自定义的数据库连接串，获取Oracle数据库对象
        public static DataBase getOracleInst(string connstr)
        {
            return new Oracle(connstr);
        }
        #endregion
    }

}
