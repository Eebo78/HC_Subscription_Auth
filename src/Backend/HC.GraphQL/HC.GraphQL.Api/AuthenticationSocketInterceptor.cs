using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using HotChocolate.Server;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace HC.GraphQL.Api
{
    public class AuthenticationSocketInterceptor : ISocketConnectionInterceptor<HttpContext>
    {
        // This is the key to the auth token in the HTTP Context
        public static readonly string HTTP_CONTEXT_WEBSOCKET_AUTH_KEY = "websocket-auth-token";
        // This is the key that apollo uses in the connection init request
        public static readonly string WEBOCKET_PAYLOAD_AUTH_KEY = "authToken";

        private readonly IAuthenticationSchemeProvider _schemes;
        public AuthenticationSocketInterceptor(IAuthenticationSchemeProvider schemes)
        {
            _schemes = schemes;
        }
        public async Task<ConnectionStatus> OnOpenAsync(
            HttpContext context,
            IReadOnlyDictionary<string, object> properties,
            CancellationToken cancellationToken)
        {
            if (properties.TryGetValue(WEBOCKET_PAYLOAD_AUTH_KEY, out object token) &&
                token is string stringToken)
            {
                context.Items[HTTP_CONTEXT_WEBSOCKET_AUTH_KEY] = stringToken;
                context.Features.Set<IAuthenticationFeature>(new AuthenticationFeature
                {
                    OriginalPath = context.Request.Path,
                    OriginalPathBase = context.Request.PathBase
                });
                // Give any IAuthenticationRequestHandler schemes a chance to handle the request
                var handlers = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
                foreach (var scheme in await _schemes.GetRequestHandlerSchemesAsync())
                {
                    var handler = handlers.GetHandlerAsync(context, scheme.Name) as IAuthenticationRequestHandler;
                    if (handler != null && await handler.HandleRequestAsync())
                    {
                        return ConnectionStatus.Reject();
                    }
                }
                var defaultAuthenticate = await _schemes.GetDefaultAuthenticateSchemeAsync();
                if (defaultAuthenticate != null)
                {
                    var result = await context.AuthenticateAsync(defaultAuthenticate.Name);
                    if (result?.Principal != null)
                    {
                        context.User = result.Principal;
                        return ConnectionStatus.Accept();
                    }
                }
            }
            return ConnectionStatus.Reject();
        }
    }
}
