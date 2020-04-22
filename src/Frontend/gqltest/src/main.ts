import Vue from "vue";
import App from "./App.vue";
import router from "./router";

import { ApolloClient } from "apollo-client";
import { HttpLink } from "apollo-link-http";
import { InMemoryCache } from "apollo-cache-inmemory";
import VueApollo from "vue-apollo";
import { ApolloLink, split } from "apollo-link";
import { setContext } from "apollo-link-context";
import AuthService from "@/services/AuthService";
import { getMainDefinition } from "apollo-utilities";
import { WebSocketLink } from "apollo-link-ws";
import { SubscriptionClient } from "subscriptions-transport-ws";

const auth = new AuthService();

const authMiddleware = setContext(async (_, { headers }) => {
  const token = await auth.getAccessToken();
  return {
    headers: {
      ...headers,
      authorization: token ? `Bearer ${token}` : null,
    },
  };
});

const wsClient = new SubscriptionClient("wss://localhost:6001", {
  reconnect: true,
  lazy: true,
  connectionParams: async () => {
    const token = await auth.getAccessToken();
    return {
      authToken: token,
    }; //WEBOCKET_PAYLOAD_AUTH_KEY = "authToken
  },
});

const wsLink = new WebSocketLink(wsClient);
const httpLink = ApolloLink.from([
  authMiddleware,
  new HttpLink({ uri: "https://localhost:6001" }),
]);

const link = split(
  ({ query }) => {
    const diff = getMainDefinition(query);
    return (
      diff.kind === "OperationDefinition" && diff.operation === "subscription"
    );
  },
  wsLink,
  httpLink
);
// Cache implementation
const cache = new InMemoryCache();

// Create the apollo client
const apolloClient = new ApolloClient({
  link,
  cache,
});
Vue.use(VueApollo);

const apolloProvider = new VueApollo({
  defaultClient: apolloClient,
});

Vue.config.productionTip = false;

new Vue({
  router,
  apolloProvider,
  render: (h) => h(App),
}).$mount("#app");
