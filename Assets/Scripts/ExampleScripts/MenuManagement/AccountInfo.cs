namespace ExampleGB
{
    public readonly struct AccountInfo
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string _email;
        private readonly LogInType _logInType;

        public AccountInfo(LogInType type, string name, string password, string email = null)
        {
            _username = name;
            _password = password;
            _email = email;
            _logInType = type;
        }

        public string Username => _username;
        public string Password => _password;
        public string Email => _email;
        public LogInType LogIn => _logInType;
    }
}