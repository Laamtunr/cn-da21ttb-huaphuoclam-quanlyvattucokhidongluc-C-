using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMuonTraVatTu.DAL
{
    public class DapperDbContext : IDisposable
    {
        private static IDbConnection _connection;
        private static IDbTransaction _transaction;
        private static bool _isClosed = false;
        private static int _transactionDepth = 0;
        private static string _connectionString;
        static DapperDbContext()
        {
            _connectionString = System.Configuration.ConfigurationManager.
                    ConnectionStrings["QuanLyMuonTraVatTuDb"].ConnectionString;
        }

        public static IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                    _connection = new SqlConnection(_connectionString);

                if (_isClosed)
                    _connection.Open();

                return _connection;
            }
        }

        public static IDbTransaction Transaction
        {
            get
            {
                if (_isClosed)
                    _connection.Open();
                return _transaction;
            }
        }

        public static void BeginTransaction()
        {
            if (_transactionDepth == 0)
                _transaction = _connection.BeginTransaction();
            _transactionDepth++;
        }

        public static void Commit()
        {
            if (_transactionDepth == 0)
                throw new InvalidOperationException("No transaction is in progress");
            _transactionDepth--;
            if (_transactionDepth == 0)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public static void Rollback()
        {
            if (_transactionDepth == 0)
                throw new InvalidOperationException("No transaction is in progress");
            _transactionDepth = 0;
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        public static void Close()
        {
            if (_isClosed)
                return;
            if (_transactionDepth > 0)
                Rollback();
            _connection.Close();
            _isClosed = true;
        }

        public void Dispose()
        {
            Close();
        }
    }
}
