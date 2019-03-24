using System;
using System.Collections.Generic;
using System.Linq;

namespace AuAt
{
    class Program
    {
        static void Main()
        {
            var someUser = new User() { Login = "SomeUser", Password = "SomePassword" };

            var someSystem = new SomeSystem();
            
            var auRes = someSystem.Authenticate(someUser.Login, someUser.Password);

            Console.WriteLine("available functions after authentication: " + string.Join(", ", auRes.Keys));

            if (auRes.Keys.Any(x => x == "getSessionId"))
            {
                var guid = ((Func<string, Guid?>)auRes["getSessionId"]).Invoke(someUser.Login);

                var atRes = ((Func<Guid, IDictionary<string, object>>)auRes["authorization"]).Invoke(guid.Value);

                Console.WriteLine("available functions after authorization:  " + string.Join(", ", atRes.Keys));
            }
        }
    }

    public class SomeSystem
    {
        private IDictionary<string, object> _functions = new Dictionary<string, object>();

        public IDictionary<string, object> Authenticate(string lgn, string psw)
        {
            var user = _userStorage.FirstOrDefault(x => string.Equals(x.Login, lgn) && string.Equals(x.Password, psw));

            if (user != null)
            {
                _sessionStorage.Add(new Session() { Login = user.Login, SessionID = Guid.NewGuid() });

                return _functions.Where(x => new string[] {"authentication", "getSessionId", "authorization" }.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            }
            else
                return _functions.Where(x => new string[] { "authentication" }.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }



        public SomeSystem()
        {
            Func<string, string, IDictionary<string, object>> authentication = (lgn, psw) => 
            {
                return Authenticate(lgn, psw);
            };

            Func<string, Guid?> getSessionId = (lgn) => 
            {
                return _sessionStorage.First(x => x.Login == lgn).SessionID;
            };

            Func<Guid, IDictionary<string, object>> authorization = (sessionId) =>
            {
                if (_sessionStorage.All(x => x.SessionID == sessionId))
                    return _functions;
                else
                    return _functions.Where(x => new string[] { "getSessionId", "authorization" }.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            };

            Func<string> yetAnotherFunction = () => { Console.WriteLine("SomeFunction"); return "OK"; };

            _functions["authentication"] = authentication;
            _functions["getSessionId"] = getSessionId;
            _functions["authorization"] = authorization;
            _functions["yetAnotherFunction"] = authentication;
        }

        private List<User> _userStorage = new List<User>()
        {
            new User() { Login = "SomeUser", Password = "SomePassword" }
        };

        private List<Session> _sessionStorage = new List<Session>();
    }

    public class User
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }

    public class Session
    {
        public Guid SessionID { get; set; }

        public string Login { get; set; }
    }
}
