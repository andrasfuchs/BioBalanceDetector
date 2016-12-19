using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDDriver.Models
{
    public class SessionsCache
    {
        private static List<MultiplexerSession> sessionList = new List<MultiplexerSession>();

        public static MultiplexerSession GetSession(string sessionId)
        {
            MultiplexerSession result = null;

            lock (sessionList)
            {
                result = sessionList.FirstOrDefault(s => s.SessionId == sessionId);

                if (result == null)
                {
                    result = new MultiplexerSession();
                    sessionList.Add(result);
                }
            }

            return result;
        }

        public static string[] ListSessionIds()
        {
            return sessionList.Select(ms => ms.SessionId).ToArray();
        }
    }
}
