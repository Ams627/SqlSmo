using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.SqlEnum;
using System.Collections.Specialized;

namespace SqlSmo
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Server srv = new Server();

                srv.ConnectionContext.ServerInstance = "ServerName";
                srv.ConnectionContext.LoginSecure = true;
                string dbName = "I";

                Database db = new Database();
                db = srv.Databases[dbName];

                StringBuilder sb = new StringBuilder();

                foreach (Table tbl in db.Tables)
                {
                    ScriptingOptions options = new ScriptingOptions();
                    options.ClusteredIndexes = true;
                    options.Default = true;
                    options.DriAll = true;
                    options.Indexes = true;
                    options.IncludeHeaders = true;

                    StringCollection coll = tbl.Script(options);
                    foreach (string str in coll)
                    {
                        sb.Append(str);
                        sb.Append(Environment.NewLine);
                    }
                }
                System.IO.StreamWriter fs = System.IO.File.CreateText("c:\\temp\\output.txt");
                fs.Write(sb.ToString());
                fs.Close();
            }
            catch (Exception ex)
            {
                var fullname = System.Reflection.Assembly.GetEntryAssembly().Location;
                var progname = Path.GetFileNameWithoutExtension(fullname);
                Console.Error.WriteLine($"{progname} Error: {ex.Message}");
            }

        }
    }
}
