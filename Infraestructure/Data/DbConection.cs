using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Data
{
    public class DbConection
    {
        private readonly string _connectionString;

        public DbConection(string connectionString)
        {
            _connectionString = connectionString;
        }
        public SqlConnection CreateConection()
        {
            return new SqlConnection(_connectionString);
        }


    }
}
