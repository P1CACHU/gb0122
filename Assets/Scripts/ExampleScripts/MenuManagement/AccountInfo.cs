namespace ExampleGB
{
    public readonly struct AccountInfo
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string _email;

        public AccountInfo(string name, string password, string email)
        {
            _username = name;
            _password = password;
            _email = email;
        }

        public string Username => _username;
        public string Password => _password;
        public string Email => _email;
    }
}