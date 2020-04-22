/* eslint-disable @typescript-eslint/camelcase */
import { User, UserManager, WebStorageStateStore } from "oidc-client";

export default class AuthService {
  private userManager: UserManager;

  constructor() {
    const AUTH0_DOMAIN = "https://localhost:5001";

    const settings: any = {
      userStore: new WebStorageStateStore({ store: window.localStorage }),
      authority: AUTH0_DOMAIN,
      client_id: "spa",
      redirect_uri: "http://localhost:8080/callback.html",
      response_type: "code",
      scope: "openid profile api1",
      post_logout_redirect_uri: "http://localhost:8080/",
      filterProtocolClaims: true,
      loadUserInfo: true,
    };
    this.userManager = new UserManager(settings);
  }

  public async getAccessToken() {
    const user = await this.userManager.getUser();
    return user && user.access_token;
  }

  public getUser(): Promise<User | null> {
    return this.userManager.getUser();
  }

  public login(): Promise<void> {
    return this.userManager.signinRedirect();
  }

  public logout(): Promise<void> {
    return this.userManager.signoutRedirect();
  }
}
