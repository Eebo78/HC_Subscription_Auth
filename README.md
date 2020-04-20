# HC_Subscription_Auth

## Development Environment

- .Net Core 3.1

## Setup

1. Start the authserver.
2. Start the graphql server
3. Use postman to genereate an accesstoken
4. Paste token into .\src\Frontend\vue-poc\apollo.js
5. run yarn serve for frontend

## How to test

### GraphQL queries for testing

Using BananaCakePop add the Authorization header:

<code>
{
    "Authorization" : "Bearer token-here"
}
</code>

Test queries:

<code>
query openHello { hello }

query authHello { privateHello}
</code>

Test mutation. Notice in the frontend that the openMutation is updated with the result

<code>
mutation openMutation {
doIt
}

mutation authMutation {
privateDoIt
}
</code>
