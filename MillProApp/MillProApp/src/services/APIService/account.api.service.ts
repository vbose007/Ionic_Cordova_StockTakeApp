import { Events } from 'ionic-angular';

import { Injectable } from "@angular/core";
import { Http, Headers, RequestOptions, Response, ResponseContentType } from "@angular/http";
import { Observable } from "rxjs/Rx";
import { ApiConstants } from "../../constants/api.constants";
import { UserProfile } from "../../models/user.profile";
import { UserRequestPasswordReset } from "../../models/user.requestPassword.reset";
import { Storage } from "@ionic/storage";
import "rxjs/add/operator/toPromise";
import { EventsConstants } from "../../constants/events.constants";

@Injectable()
export class AccountApiService {


  //public static isLoggedIn$: Observable<boolean> = Observable.of(false);

  constructor(public http: Http, public storage: Storage, public events: Events) { }

  isUserNameTaken(username: string): Observable<boolean> {
    username = encodeURIComponent(username);
    let url = ApiConstants.accountsBaseUrl + "/checkUserNameTaken?username=" + username;

    return this.http.get(url).map((result) => {
      return result.text() === "true";
    });
  }

  login(username: string, password: string): Promise<void> {
    var headers = new Headers();
    headers.append("Content-Type", "application/x-www-form-urlencoded");

    username = encodeURIComponent(username);
    password = encodeURIComponent(password);

    var options = new RequestOptions({
      //method: RequestMethod.Post,
      //url: ApiConstants.baseUrl,
      headers: headers,
      //body: `"username='${username}'&password='${password}'&grant_type=password"`,
      responseType: ResponseContentType.Json
    });

    let body = `username=${username}&password=${password}&grant_type=password`;

    let loginUrl = ApiConstants.accountTokenUrl;

    return this.http.post(loginUrl, body, options).map((resp) => resp.json()).toPromise().then((resp) => {
      console.log("login success!");
      this.storage.ready().then(() => {
        let accessTokenDetails = { "access_token": resp.access_token, "token_type": resp.token_type };
        //JSON.stringify({"access_token": resp.access_token, "token_type": resp.token_type})
        this.storage.set(ApiConstants.UserTokenStorageKey, JSON.stringify(accessTokenDetails)).then(() => {
          //AccountApiService.isLoggedIn$ = Observable.of(true);

            this.events.publish(EventsConstants.loggedIn, username);
        });
      })
        .catch((err) => {
          console.log(`"Login failed. Error : ${JSON.stringify(err)}"`);
          throw err;
        });
    });



    //return this.storage.ready().then(() => {

    //  let userToken: any;
    //  this.http.post(loginUrl, body, options).toPromise().then((resp) => {
    //        console.log("login success!");
    //        userToken = resp;
    //      },
    //      (err) => {
    //        console.log(`"Login failed. Error : ${JSON.stringify(err)}"`);
    //        throw err;

    //      })
    //      .then(() => {
    //        return this.storage.set(ApiConstants.UserTokenStorageKey, userToken.json()).then(() => {
    //          AccountApiService.isLoggedIn$ = Observable.of(true);
    //        });
    //      })
    //      .catch((err) => {
    //        console.log(`"Failed to store user token : ${JSON.stringify(err)}"`);
    //        throw err;
    //      });
    //});
  }

  logout(): Promise<Response> {
    return this.getRequesttHeaderWithAuthToken()
      .then((headers) => {
        return new RequestOptions({
          headers: headers
        });
      },
      (err) => {
        if (err.error === ApiConstants.NotLoggedInError) {
        }

        return null;
      })
      .then((options) => {
        if (options === null || options === undefined) {
          return new Promise<Response>(() => null);
        }

        this.storage.remove(ApiConstants.UserTokenStorageKey);
        this.events.publish(EventsConstants.loggedOut);

        return this.http.post(ApiConstants.logoutUrl, "", options).toPromise();
      });
  }



  registerUser(user: UserProfile): Observable<Response> {
    var headers = new Headers();
    headers.append("Content-Type", "application/json");

    var options = new RequestOptions({
      headers: headers,
      responseType: ResponseContentType.Json
    });


    let registerUrl = ApiConstants.accountRegistrationUrl;

    return this.http.post(registerUrl, JSON.stringify(user), options);
  }


  updateUserProfile(user: UserProfile): Promise<Response> {
    let parentScope = this;
    return this.getRequesttHeaderWithAuthToken()
      .then((headers) => {
        return new RequestOptions({
          headers: headers
          //responseType: ResponseContentType.Json
        });
      },
        (err) => {
           if (err.error === ApiConstants.NotLoggedInError) {
            this.events.publish(EventsConstants.loggedOut);
           }

          throw err;
        }
      )
      .then((options) => {
        if (options === null || options === undefined) {
          return new Promise<Response>(() => null);
        }
        return this.http.post(ApiConstants.accountUserProfile,
          JSON.stringify(user),
          options).toPromise();
      },
      (err) => {throw err;}
    );
  };



  requestResetPassword(userResetRequest: UserRequestPasswordReset): Observable<Response> {
    var headers = new Headers();
    headers.append("Content-Type", "application/json");

    var options = new RequestOptions({
      headers: headers,
      responseType: ResponseContentType.Json
    });


    let requestResetUrl = ApiConstants.requestResetUrl;

    return this.http.post(requestResetUrl, JSON.stringify(userResetRequest), options);
  }

  getRequesttHeaderWithAuthToken(): Promise<Headers> {
    var headers = new Headers();
    headers.append("Content-Type", "application/json");
    headers.append("Accept", "application/json");

    return this.storage.ready().then(() => {
      return this.storage.get(ApiConstants.UserTokenStorageKey).then((res) => {

        if (res !== null && res !== undefined) {
          var accessTokenDetails = JSON.parse(res);

          var access_token = accessTokenDetails.access_token;

          var token_type = accessTokenDetails.token_type;

          headers.append("Authorization", `${token_type} ${access_token}`);
        } else {
          throw { error: ApiConstants.NotLoggedInError };
        }
        return headers;
      });
    });
  }


  getUserProfile(): Promise<Response>{
    let parentScope = this;
    return this.getRequesttHeaderWithAuthToken()
      .then((headers) => {
        return new RequestOptions({
          headers: headers
          //responseType: ResponseContentType.Json
        });
      },
        (err) => {
          if (err.error === ApiConstants.NotLoggedInError) {
            //parentScope.navCtrl.push(LoginForm);
          }

          return null;
        }
      )
      .then((options) => {
        if (options === null || options === undefined) {
          return new Promise<Response>(() => null);
        }
        return this.http.get(ApiConstants.accountUserProfile,options).toPromise();
      });
  }

}
