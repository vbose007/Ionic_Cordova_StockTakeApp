import { FormControl } from '@angular/forms';
import { AccountApiService } from "../services/APIService/account.api.service";
import { AlertHelper } from "../helpers/alert.helper";
import { Injectable } from "@angular/core";

@Injectable()
export class UsernameValidator {

  constructor(public accountApi: AccountApiService, public alertHelper: AlertHelper) {
    
  }

  checkUsername(control: FormControl): any {

    let parentScope = this;
    return new Promise(resolve => {

    let username = control.value.toLowerCase();

        parentScope.accountApi.isUserNameTaken(username).subscribe(
        (resp) => {
          if (resp === true) {
            resolve({
              "username taken": true
            });
          } else {
            resolve(null);
          }
        },
        (err) => {
            //this.alertHelper.showErrorAlert(`Error validating username.${username} : \n ${JSON.stringify(err)}` );
            console.log(`Error validating username.${username} : ${JSON.stringify(err)}`);
        }
      );
    });
  }

}
