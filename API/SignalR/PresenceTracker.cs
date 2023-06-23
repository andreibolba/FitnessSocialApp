﻿
namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> _onlineUsers = new Dictionary<string, List<string>>();

        public Task UserConnected(string username, string connectionId)
        {
            lock(_onlineUsers)
            {
                if (_onlineUsers.ContainsKey(username))
                {
                    _onlineUsers[username].Add(connectionId);
                }
                else
                {
                    _onlineUsers.Add(username, new List<string>());
                }
            }
            return Task.CompletedTask;
        }

        public Task UserDisconnected(string username, string connectionId)
        {
            lock (_onlineUsers)
            {
                if (!_onlineUsers.ContainsKey(username))
                {
                    return Task.CompletedTask;
                }
                _onlineUsers[username].Remove(connectionId);

                if(_onlineUsers[username].Count==0 ) {
                    _onlineUsers.Remove(username);
                }
            }
            return Task.CompletedTask;
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUser;
            lock(_onlineUsers)
            {
                onlineUser = _onlineUsers.OrderBy(k=>k.Key).Select(k=>k.Key).ToArray();
            }

            return Task.FromResult(onlineUser);
        }
    }
}
