<template>
  <div class="home">
    <p v-if="isLoggedIn">User: {{ username }} AccessToken: {{ at }}</p>
    <button @click="login" v-if="!isLoggedIn">Login</button>
    <button @click="logout" v-if="isLoggedIn">Logout</button>
    <OpenHello />
    <PrivateHello />
    <OpenMutation />
    <PrivateMutation />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import OpenHello from "@/components/OpenHello.vue";
import PrivateHello from "@/components/PrivateHello.vue";
import OpenMutation from "@/components/OpenMutation.vue";
import PrivateMutation from "@/components/PrivateMutation.vue";
import AuthService from "@/services/AuthService";

const auth = new AuthService();

@Component({
  name: "Home",
  components: {
    OpenHello,
    PrivateHello,
    OpenMutation,
    PrivateMutation
  }
})
export default class Home extends Vue {
  public currentUser = "";
  public accessTokenExpired: boolean | undefined = false;
  public isLoggedIn = false;
  public at = "";
  get username(): string {
    return this.currentUser;
  }

  public accessToken = () => this.at;

  public login() {
    auth.login();
  }

  public logout() {
    auth.logout();
  }

  public mounted() {
    auth.getUser().then(user => {
      if (user !== null) {
        this.currentUser = user.profile.name ?? "";
        this.accessTokenExpired = user.expired;
        this.at = user.access_token;
      }

      this.isLoggedIn = user !== null && !user.expired;
    });
  }
}
</script>
