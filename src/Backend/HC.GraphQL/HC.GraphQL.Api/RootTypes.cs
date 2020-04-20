using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Subscriptions;
using HotChocolate.Types;

namespace HC.GraphQL.Api
{
    public class RootTypes
    {
        public class Queries
        {
            public string Hello => "World!!!";

            [Authorize]
            public string PrivateHello => "Private hello message";
        }

        public class Mutations
        {
            public async Task<string> DoIt([Service]ITopicEventSender eventSender)
            {
                await eventSender.SendAsync("openSub", "Open mutation triggered");
                return "Done it";
            }

            [Authorize]
            public async Task<string> PrivateDoIt([Service]ITopicEventSender eventSender)
            {
                await eventSender.SendAsync("privateSub", "Private mutation triggered");
                return "Done it in private";
            }
        }


        public class Subscriptions
        {
            [SubscribeAndResolve]
            public async Task<IAsyncEnumerable<string>> DoItSubscription([Service]ITopicEventReceiver eventReceiver)
            {
                return await eventReceiver.SubscribeAsync<string, string>("openSub").ConfigureAwait(false);
            }

            [Authorize]
            [SubscribeAndResolve]
            public async ValueTask<IAsyncEnumerable<string>> OnSomeThingHappendSubscription(
                [Service]ITopicEventReceiver eventReceiver,
                CancellationToken cancellationToken)
            {
                return await eventReceiver.SubscribeAsync<string, string>("privateSub", cancellationToken);
            }
        }
    }
}
