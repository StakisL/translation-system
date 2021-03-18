namespace TranslateSystem.Persistence.Settings
{
    public class BasicConnectionProvider : IConnectionProvider
    {
        private readonly string _connectionString;

        public BasicConnectionProvider(string cs)
        {
            _connectionString = cs;
        }

        public string GetConnection() => _connectionString;
    }
}
