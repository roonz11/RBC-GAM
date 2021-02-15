using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RBC_GAM.Data;
using System;

namespace RBC_GAM_Tests
{
    public abstract class TestWithSqlLite : IDisposable
    {
        private const string InMemoryCnnectionString = "Data Source=finanicalInstrument.db";
        private readonly SqliteConnection _sqliteConnection;
        protected readonly FinInstContext _dbContext;

        public TestWithSqlLite()
        {
            _sqliteConnection = new SqliteConnection(InMemoryCnnectionString);
            _sqliteConnection.Open();
            var options = new DbContextOptionsBuilder<FinInstContext>()
                .UseSqlite(_sqliteConnection)
                .Options;
            _dbContext = new FinInstContext(options);
            _dbContext.Database.EnsureCreated();
        }
        public void Dispose()
        {
            _sqliteConnection.Close();
        }
    }
}
