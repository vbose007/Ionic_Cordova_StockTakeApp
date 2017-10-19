import { Component, ViewChild } from "@angular/core";
import { UserRequestPasswordReset } from "../../models/user.requestPassword.reset";
import { NavController } from "ionic-angular";
import { FormBuilder, FormGroup, Validators, FormControl } from "@angular/forms";
import { UsernameValidator } from "../../validators/username.validator";
import { debounce } from "rxjs/operator/debounce";
import { ValidatorConstants } from "../../validators/ValidatorConstants";
import { AccountApiService } from "../../services/APIService/account.api.service";
import { AlertHelper } from "../../helpers/alert.helper";
import { AlertController } from "ionic-angular";
import { LoginForm } from '../../pages/loginForm/loginForm.component';


@Component({
  selector: "registration-form",
  templateUrl: "./requestResetPasswordForm.form.html"
})
export class RequestResetPasswordForm {

  @ViewChild('signupSlider') signupSlider: any;

  resetPasswordForm: FormGroup;

  submitAttempt: boolean = false;
  invalidUsername = false;
  invalidConfirmPassword = false;

  strongRegex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{4,20})");

  constructor(public navCtrl: NavController, public formBuilder: FormBuilder, public usernameValidator: UsernameValidator,
    public accountService: AccountApiService, public alertHelper: AlertHelper, public alertCt: AlertController) {

    this.resetPasswordForm = formBuilder.group({
      username: ['', Validators.compose([Validators.required, Validators.maxLength(100), Validators.pattern(ValidatorConstants.EMAIL_REGEX)])]
    });
  }


  createTimeout(timeout) {
    return new Promise((resolve, reject) => {
      setTimeout(() => resolve(null), timeout);
    });
  }


  checkConfirmPasswordMatch(passwordCtrl: FormControl, confirmPasswordCtrl: FormControl) {

    confirmPasswordCtrl.markAsPending(true);

    if (confirmPasswordCtrl.value !== passwordCtrl.value) {
      this.invalidConfirmPassword = true;
    } else {
      this.invalidConfirmPassword = false;
    }
    this.createTimeout(100)
      .then(() => {
        confirmPasswordCtrl.updateValueAndValidity();
      });

  }

  reset() {



    let email = this.resetPasswordForm.controls["username"].value;

    let id = 0;

    if (this.resetPasswordForm.controls.username.valid) {
      var userRequestReset = new UserRequestPasswordReset(email);

      this.accountService.requestResetPassword(userRequestReset).subscribe((resp) => {
        console.log("success!");

        this.showMessageAlert("Reset request successful", "Please check your email to reset your account");

      }, (err) => {
        console.log(`"Error: ${err}"`);
        this.submitAttempt = true;
      });
    }
    else {
      console.log(`"Error: Invalid username"`);
      this.submitAttempt = true;
    }
  }

  showMessageAlert(title: string, msg: string) {
    let alert = this.alertCt.create({
      title: title,
      message: msg,
      buttons: [
        {
          text: "OK",
          handler: () => {
            console.log("Ok clicked");
            this.navCtrl.push(LoginForm);
          }
        }
      ]
    });

    alert.present();
  }
}
