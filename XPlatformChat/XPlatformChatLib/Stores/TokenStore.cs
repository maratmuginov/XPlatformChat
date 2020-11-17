using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using XPlatformChat.Lib.Models;

namespace XPlatformChat.Lib.Stores
{
    public class TokenStore
    {
        private readonly HashSet<TokenGrant> _tokenGrants;
        public TokenStore()
        {
            _tokenGrants = new HashSet<TokenGrant>();
        }

        public bool TryAddTokenGrant(TokenGrant tokenGrant)
        {
            if (tokenGrant.Expiration < DateTime.Now)
                throw new ArgumentException();

            return _tokenGrants.Add(tokenGrant);
        }

        public TokenGrant GetTokenGrant()
        {
            return _tokenGrants.Where(IsNotExpired).OrderBy(t => t.Expiration).FirstOrDefault();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNotExpired(TokenGrant tokenGrant) => 
            tokenGrant.Expiration.CompareTo(DateTime.Now) > 0;
    }
}
