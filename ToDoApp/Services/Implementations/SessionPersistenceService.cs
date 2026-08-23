using Supabase.Gotrue.Interfaces;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ToDoApp.Services.Implementations
{
    public class SessionPersistenceService : IGotrueSessionPersistence<Session>
    {
        private const string SessionKey = "supabase_session";

        public bool ShouldPersist { get; set; }

        public void SaveSession(Session session)
        {
            if (!ShouldPersist)
            {
                return;
            }

            var serializedSession = JsonSerializer.Serialize(session);
            SecureStorage.SetAsync(SessionKey, serializedSession).Wait();
        }

        public void DestroySession()
        {
            SecureStorage.Remove(SessionKey);
        }

        public Session? LoadSession()
        {
            var serializedSession = SecureStorage.GetAsync(SessionKey).Result;

            System.Diagnostics.Debug.WriteLine($"LoadSession: {(string.IsNullOrEmpty(serializedSession) ? "NULL/EMPTY" : "FOUND DATA")}");

            if (string.IsNullOrEmpty(serializedSession))
            {
                return null;
            }

            return JsonSerializer.Deserialize<Session>(serializedSession);
        }
    }
}
