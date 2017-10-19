import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { UserProfile } from './../../models/user.profile';
import { AccountApiService } from './../../services/APIService/account.api.service';
import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { ValidatorConstants } from "../../validators/ValidatorConstants";


@Component({
  selector: 'user-info',
  templateUrl: 'userInfo.html'
})
export class UserInfoPage {

  userProfile: UserProfile;
  userProfileEditForm: FormGroup;
  isEditMode: boolean = false;

  constructor(public navCtrl: NavController, public navParams: NavParams, public accountApi: AccountApiService, public formBuilder: FormBuilder,) {

    this.userProfileEditForm = formBuilder.group({
      firstName: [
        '', Validators.compose([Validators.maxLength(30), Validators.pattern('[a-zA-Z ]*'), Validators.required])
      ],
      lastName: [
        '', Validators.compose([Validators.maxLength(30), Validators.pattern('[a-zA-Z ]*'), Validators.required])
      ],
      company: ['', Validators.compose([Validators.maxLength(100)])],
      companyEmail: ['', Validators.compose([Validators.required, Validators.maxLength(100), Validators.pattern(ValidatorConstants.EMAIL_REGEX)])],
      email: ['', Validators.compose([Validators.required, Validators.maxLength(100), Validators.pattern(ValidatorConstants.EMAIL_REGEX)])]
  });

    this.getUserProfileInfo();
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad UserInfoPage');
  }

  getUserProfileInfo(){
    let parentScope = this;
    this.accountApi.getUserProfile().then((resp) => {
      var userInfo = JSON.parse(resp.text());
      parentScope.mapUserInfo(userInfo);
      console.log("UserProfile: "+ JSON.stringify(parentScope.userProfile));
      console.log("UserProfile Obj: ");
      console.log(parentScope.userProfile);

    })
  }

  mapUserInfo(userInfo: any){
    this.userProfile = new UserProfile(userInfo.Id,userInfo.FirstName, userInfo.LastName, userInfo.CompanyName, userInfo.CompanyEmail, userInfo.Email, null, null);

    this.userProfileEditForm.controls.firstName.setValue(userInfo.FirstName);
    this.userProfileEditForm.controls.lastName.setValue(userInfo.LastName);
    this.userProfileEditForm.controls.email.setValue(userInfo.Email);
    this.userProfileEditForm.controls.company.setValue(userInfo.CompanyName);
    this.userProfileEditForm.controls.companyEmail.setValue(userInfo.CompanyEmail);
  }

  save(){

    let user: UserProfile = new UserProfile( "0",

    this.userProfileEditForm.controls.firstName.value,
    this.userProfileEditForm.controls.lastName.value,
    this.userProfileEditForm.controls.company.value,
    this.userProfileEditForm.controls.companyEmail.value,
    this.userProfileEditForm.controls.email.value,
    "",
    "");

    this.accountApi.updateUserProfile(user)
    .then((resp) =>{
      this.userProfile.firstName = user.firstName;
      this.userProfile.lastName = user.lastName;
      this.userProfile.companyName = user.companyName;
      this.userProfile.companyEmail = user.companyEmail;
      this.userProfile.email = user.email;
      this.isEditMode = false;
    })
    .catch((err) => {
      alert(err);
    });


    //console.log(this.userProfile);
  }

  edit(){
    this.isEditMode = true;
  }
}
