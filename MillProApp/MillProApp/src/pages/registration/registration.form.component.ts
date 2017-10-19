import { Component, ViewChild} from "@angular/core";
import {UserProfile} from "../../models/user.profile";
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
  templateUrl: "./registration.form.html"
})
export class RegistrationForm {

  @ViewChild('signupSlider') signupSlider: any;

  slideOneForm: FormGroup;
  slideTwoForm: FormGroup;

  submitAttempt: boolean = false;
  invalidUsername = false;
  invalidConfirmPassword = false;

  strongRegex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{4,20})");

  constructor(public navCtrl: NavController, public formBuilder: FormBuilder, public usernameValidator: UsernameValidator,
    public accountService: AccountApiService, public alertHelper: AlertHelper, public alertCt : AlertController) {

    this.slideOneForm = formBuilder.group({
      firstName: [
        '', Validators.compose([Validators.maxLength(30), Validators.pattern('[a-zA-Z ]*'), Validators.required])
      ],
      lastName: [
        '', Validators.compose([Validators.maxLength(30), Validators.pattern('[a-zA-Z ]*'), Validators.required])
      ],
      company: ['', Validators.compose([Validators.maxLength(100)])],
      companyEmail: ['', Validators.compose([Validators.required, Validators.maxLength(100), Validators.pattern(ValidatorConstants.EMAIL_REGEX)])]
  });

    let usernamevalidation = (control) => { return usernameValidator.checkUsername(control); }
    this.slideTwoForm = formBuilder.group({
      username: ['', Validators.compose([Validators.required, Validators.maxLength(100), Validators.pattern(ValidatorConstants.EMAIL_REGEX)])],
      //username: ['', Validators.compose([Validators.required, Validators.pattern('[a-zA-Z]*')]), usernamevalidation],
      password: ['', Validators.compose([Validators.required, Validators.pattern(this.strongRegex)])],
      confirmPassword: ['', Validators.compose([Validators.required, Validators.pattern(this.strongRegex)])]
      //privacy: ['', Validators.required],
      //bio: ['']
    });
  }

  next() {
    this.signupSlider.slideNext();
  }

  prev() {
    this.signupSlider.slidePrev();
  }

  createTimeout(timeout) {
    return new Promise((resolve, reject) => {
      setTimeout(() => resolve(null), timeout);
    });
  }

  checkIsUsernameAvailable(usernameCtrl: FormControl) {
    usernameCtrl.markAsPending(true);
    usernameCtrl.markAsDirty(true);
    this.usernameValidator.checkUsername(usernameCtrl).then((result) => {
      if (result == null) {
        //usernameCtrl.valid = true;
        this.invalidUsername = false;
      } else {
        //usernameCtrl.valid = false;
        this.invalidUsername = true;
      }

      this.createTimeout(100)
        .then(() => {
          usernameCtrl.updateValueAndValidity();
        });
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

  save() {

    this.submitAttempt = true;

    if (!this.slideOneForm.valid) {
      this.signupSlider.slideTo(0);
    }
    else if (!this.slideTwoForm.valid) {
      this.signupSlider.slideTo(1);
    } else {
      //console.log(this.slideOneForm.value);
      //console.log(this.slideTwoForm.value);


      let firstNamestr = this.slideOneForm.controls["firstName"].value;
      let lastNamestr = this.slideOneForm.controls["lastName"].value;
      let company = this.slideOneForm.controls["company"].value;
      let companyEmail = this.slideOneForm.controls["companyEmail"].value;
      let email = this.slideTwoForm.controls["username"].value;
      let password = this.slideTwoForm.controls["password"].value;
      let confirmPassword = this.slideTwoForm.controls["confirmPassword"].value;
      let id = 0;

      var user = new UserProfile("0", firstNamestr, lastNamestr, company, companyEmail, email, password, confirmPassword);

      this.accountService.registerUser(user).subscribe((resp) => {
        console.log("success!");

        this.showMessageAlert("Register successful", "Registration successful");

      }, (err) => {
        console.log(`"Error: ${err}"`);
      });
    }
  }

  showMessageAlert(title:string, msg: string) {
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
