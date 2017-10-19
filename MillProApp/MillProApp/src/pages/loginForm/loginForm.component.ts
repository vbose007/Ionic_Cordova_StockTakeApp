import { WorkOrderData } from './../../models/workOrder.data';
import { WorkOrder } from './../workOrder/workOrder.component';
import { Component, ViewChild } from "@angular/core";
import { UserProfile } from "../../models/user.profile";
import { NavController, App } from "ionic-angular";
import { FormBuilder, FormGroup, Validators, FormControl } from "@angular/forms";
import { UsernameValidator } from "../../validators/username.validator";
import { debounce } from "rxjs/operator/debounce";
import { ValidatorConstants } from "../../validators/ValidatorConstants";
import { AccountApiService } from "../../services/APIService/account.api.service";
import { AlertHelper } from "../../helpers/alert.helper";
import { Storage } from '@ionic/storage';
import { ActiveWorkOrders } from "../activeWorkOrders/activeWorkOrders.component";
import { Page2 } from "../page2/page2";
import { ApiConstants } from "../../constants/api.constants";
import { RequestResetPasswordForm } from "../requestResetPassword/requestResetPasswordForm.form.component";

@Component({
  selector: "login-form",
  templateUrl: "./loginForm.html"
})
export class LoginForm {

  @ViewChild('signupSlider')
  signupSlider: any;

  loginForm: FormGroup;

  submitAttempt: boolean = false;
  invalidUsername = false;

  strongRegex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{4,20})");

  constructor(private nav: NavController,
    //protected app: App,
    public formBuilder: FormBuilder,
    public usernameValidator: UsernameValidator,
    public accountService: AccountApiService,
    public alertHelper: AlertHelper,
    public storage: Storage) {

    //this.navCtrl = app.getActiveNav();
    //let usernamevalidation = (control) => { return usernameValidator.checkUsername(control); }
    this.loginForm = formBuilder.group({
      username: [
        '',
        Validators.compose([
          Validators.required, Validators.maxLength(100), Validators.pattern(ValidatorConstants.EMAIL_REGEX)
        ])
      ],
      password: ['', Validators.compose([Validators.required])],
    });
  }



  createTimeout(timeout) {
    return new Promise((resolve, reject) => {
      setTimeout(() => resolve(null), timeout);
    });
  }

  login() {

    this.submitAttempt = true;

    if (this.loginForm.valid) {

      console.log(this.loginForm.value);

      let email = this.loginForm.controls["username"].value;
      let password = this.loginForm.controls["password"].value;
      let parentScope = this;
      this.accountService.login(email, password).then(() => {
        this.storage.ready().then(() => {
          this.storage.set(ApiConstants.UserName, email).then(() => {
            this.nav.setRoot(ActiveWorkOrders);
          });
        });
      }
      ).catch((err) => { console.log(`"Login form: Error in trying to login.: ${err}"`); });
    }
  }

  resetPassword() {
    this.nav.setRoot(RequestResetPasswordForm);
  }


}
