import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import { apolloProvider } from "./GraphQL/apollo";

Vue.config.productionTip = false;

new Vue({
  router,
  apolloProvider,
  store,
  render: (h) => h(App),
}).$mount("#app");
