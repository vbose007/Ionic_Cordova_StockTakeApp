import { Injectable, Inject } from "@angular/core";
import { Http, RequestOptions, RequestMethod, Headers, Response, ResponseContentType } from "@angular/http";
import { WorkOrderData } from "../../models/workOrder.data";
import 'rxjs/add/operator/map';
import { Observable, Subscription } from "rxjs";
import { ApiConstants } from "../../constants/api.constants";
import 'rxjs/add/operator/timeout';
import 'rxjs/add/operator/toPromise';
import { AccountApiService } from "./account.api.service";
import { NavController, Events } from "ionic-angular";
import { LoginForm } from "../../pages/loginForm/loginForm.component";
import { EventsConstants } from "../../constants/events.constants";



@Injectable()
export class APIService {


    constructor(public http: Http, public accountApi: AccountApiService, public events : Events) {
    }

    //Not being used at the moment
    getWorkOrder(workOrderNumber: string): Observable<Response> {

        let workOrder: WorkOrderData = null;

        return this.http.get(ApiConstants.baseUrl + workOrderNumber).map(result => result.json());
    }

    postWorkOrder(workOrder:WorkOrderData, email:string): Promise<Response>{
        //var headers = new Headers();
        //headers.append("Content-Type", "application/json");
      let parentScope = this;
      return this.accountApi.getRequesttHeaderWithAuthToken()
        .then((headers) => {
          return new RequestOptions({
            headers: headers
            //responseType: ResponseContentType.Json
          });
        },
          (err) => {
             if (err.error === ApiConstants.NotLoggedInError) {
              this.events.publish(EventsConstants.loggedOut);
               //parentScope.navCtrl.push(LoginForm);
             }

            return null;
          }
        )
        .then((options) => {
          if (options === null || options === undefined) {
            return new Promise<Response>(() => null);
          }
          return this.http.post(ApiConstants.baseUrl,
            JSON.stringify({ "workOrder": workOrder, "toEmail": email }),
            options).toPromise();
        });
    }
}
