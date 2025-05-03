using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;

namespace ConnectionUtils
{
    public abstract class ConnectionFactory
    {
        protected ConnectionFactory()
        {
        }

        private static ConnectionFactory instance;

        public static ConnectionFactory getInstance()
        {
            if (instance == null)
            {

                Assembly assem = Assembly.GetExecutingAssembly();
                Type[] types = assem.GetTypes();
                foreach (var type in types)
                {  Console.WriteLine(type.FullName);

                    if (type.IsSubclassOf(typeof(ConnectionFactory)))
                    {
                        Console.WriteLine($"Alegerea tipului de ConnectionFactory: {type.Name}");
                        instance = (ConnectionFactory)Activator.CreateInstance(type);
                    }
                }
            }
            return instance;
        }

        public abstract  IDbConnection createConnection(IDictionary<string,string> props);
    }




}