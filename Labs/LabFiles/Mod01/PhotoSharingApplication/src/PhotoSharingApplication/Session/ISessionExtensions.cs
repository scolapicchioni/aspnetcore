using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSharingApplication.Session
{
    public static class ISessionExtensions
    {
        private const string favoritesKey = "Favorites";
        public static List<int> GetOrCreateFavoriteIds(this ISession session)
        {
            List<int> favoriteIds = null;
            byte[] favoriteIdsBytes = session.Get(favoritesKey);

            if (favoriteIdsBytes != null && favoriteIdsBytes.Length > 0)
            {
                string json = System.Text.Encoding.UTF8.GetString(favoriteIdsBytes);
                favoriteIds = JsonConvert.DeserializeObject<List<int>>(json);
            }
            else
            {
                favoriteIds = new List<int>();
            }
            return favoriteIds;
        }

        public static void AddFavoriteId(this ISession session, int id) {
            List<int> favoriteIds = session.GetOrCreateFavoriteIds();
            if(!favoriteIds.Contains(id))
                favoriteIds.Add(id);

            string json = JsonConvert.SerializeObject(favoriteIds);
            byte[] serializedResult = System.Text.Encoding.UTF8.GetBytes(json);

            session.Set(favoritesKey, serializedResult);
        }
    }
}
