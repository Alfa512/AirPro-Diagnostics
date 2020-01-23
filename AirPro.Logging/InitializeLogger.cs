using System;
using System.Data;

namespace AirPro.Logging
{
    public static partial class Logger
    {
        private static bool _initialized;
        private static string _connectionString;

        private static string ConnectionString => !string.IsNullOrEmpty(_connectionString)
            ? _connectionString : throw new NullReferenceException("Connection String Not Initialized.");

        public static void Initialize(string connectionString, bool reset = false)
        {
            if (_initialized && !reset && !string.IsNullOrEmpty(_connectionString)) return;

            _connectionString = connectionString;
            _initialized = true;
        }
    }
}
