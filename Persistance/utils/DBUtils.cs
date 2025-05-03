using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConnectionUtils;
namespace Persistance.utils
{
    public static class DBUtils
    {
		

        private static IDbConnection instance = null;


        public static IDbConnection getConnection(IDictionary<string,string> props)
        {
            if (instance == null || instance.State == System.Data.ConnectionState.Closed)
            {
                instance = getNewConnection(props);
                instance.Open();
            }
            return instance;
        }

        private static IDbConnection getNewConnection(IDictionary<string,string> props)
        {
			
            return ConnectionUtils.ConnectionFactory.getInstance().createConnection(props);


        }
    }
}

