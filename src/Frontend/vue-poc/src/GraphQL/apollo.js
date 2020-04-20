import Vue from "vue";
import { ApolloClient } from "apollo-client";
import { HttpLink } from "apollo-link-http";
import { split, ApolloLink } from "apollo-link";
import { InMemoryCache } from "apollo-cache-inmemory";
import { getMainDefinition } from "apollo-utilities";
import VueApollo from "vue-apollo";
import { SubscriptionClient } from "subscriptions-transport-ws";

const token =
  "eyJhbGciOiJSUzI1NiIsImtpZCI6IkpnVHZCZ0MxelZLdUota1ZPdDZwTXciLCJ0eXAiOiJhdCtqd3QifQ.eyJuYmYiOjE1ODczNzA4ODIsImV4cCI6MTU4NzM3NDQ4MiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6InJlc291cmNlYXBpIiwiY2xpZW50X2lkIjoiY2xpZW50Iiwic2NvcGUiOlsiYXBpLnJlYWQiXX0.IjK4rof11UKkNTvRmwwgG1PZ29hOnAUluXSHa8D21caj4Bgfc_Z9tjw-GCJEe0wDAE0XakcC5SlRDEn46sIAJPd8E8-8ktqbKvqL9fIwEnCF_jTiHiOzFX5_56VE_Ev0I1ypcTN6LkkTomZOItSaQ9YpBwPI0IjkfUSXhpFm2s3RUz9Ewl3b-gLpSnFBoeb9xb2ZwbQSu7quGJWaAJCzo4tWjxKtZjIZvPrHWM40X2oBEoygj5SeDBxf-67dY3MN5_vEimrTW7SB0Msbn0Ghus14aLFo-tgL1ou_2kKLEN74Ca1u0mgcYdpiotNl7M7cbYXubvLrNvPolHIwXM5c3A";

const httpLink = new HttpLink({
  uri: `https://localhost:6001`,
});

const authMiddleware = new ApolloLink((operation, forward) => {
  // add the authorization to the headers

  operation.setContext({
    headers: {
      authorization: token ? `Bearer ${token}` : null,
    },
  });

  return forward(operation);
});

const wsLink = new SubscriptionClient(
  `wss://localhost:6001`, //?access_token=${token}`,
  {
    reconnect: true,
    connectionParams: {
      authToken: token, //WEBOCKET_PAYLOAD_AUTH_KEY = "authToken
    },
  }
);

// using the ability to split links, you can send data to each link
// depending on what kind of operation is being sent
const link = split(
  // split based on operation type
  ({ query }) => {
    const definition = getMainDefinition(query);
    return (
      definition.kind === "OperationDefinition" &&
      definition.operation === "subscription"
    );
  },
  wsLink,
  httpLink
);

// Create the apollo client
export const apolloClient = new ApolloClient({
  link: authMiddleware.concat(link),
  cache: new InMemoryCache(),
  connectToDevTools: true,
});

// Install the Vue plugin

Vue.use(VueApollo);

export const apolloProvider = new VueApollo({
  defaultClient: apolloClient,
});
