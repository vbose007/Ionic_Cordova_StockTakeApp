import { UserInfoPage } from './../pages/userInfo/userInfo.component';
import { ActiveWorkOrders } from '../pages/activeWorkOrders/activeWorkOrders.component';
import { NewWorkOrderForm } from '../pages/newWorkOrder/newWorkOrderForm.component';
import { WorkOrder } from '../pages/workOrder/workOrder.component';
import { RegistrationForm } from '../pages/registration/registration.form.component';
import { LoginForm } from '../pages/loginForm/loginForm.component';
import { Component, ViewChild } from '@angular/core';
import { MenuController, Nav, Platform } from 'ionic-angular';
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { Events } from 'ionic-angular';
import { HomePage } from '../pages/home/home';
import { ListPage } from '../pages/list/list';
import { EventsConstants } from "../constants/events.constants";
import { AlertController } from 'ionic-angular';
import { AccountApiService } from "../services/APIService/account.api.service";
import { ElementRef } from '@angular/core';
import { ApiConstants } from "../constants/api.constants";
import { Storage } from "@ionic/storage";


@Component({
  templateUrl: 'app.html'
})
export class MyApp {
  @ViewChild(Nav) nav: Nav;

  rootPage: any = LoginForm;//RegistrationForm;//HomePage;

  pages: Array<{ title: string, component: any }>;

  isLoggedIn: Boolean = false;

  constructor(public platform: Platform, public statusBar: StatusBar, public splashScreen: SplashScreen,
    public events: Events, public alertCtrl: AlertController, public accountService: AccountApiService, public menuCtrl: MenuController,
    public storage: Storage) {
    this.initializeApp();

    this.renderMenu();

    this.events.subscribe(EventsConstants.loggedIn, (username) => {
      this.isLoggedIn = true;
      this.renderMenu();
    });

    this.events.subscribe(EventsConstants.loggedOut, () => {
      this.isLoggedIn = false;
      this.storage.ready().then(() => {
        this.storage.remove(ApiConstants.UserName);
      });
      this.renderMenu();
      this.nav.setRoot(LoginForm);
    });
  }

  renderMenu() {
    if (!this.isLoggedIn) {
      this.pages = [
        { title: 'Home', component: HomePage },
        { title: 'Registration', component: RegistrationForm },
        { title: 'Login', component: LoginForm },
      ];
    } else {
      this.pages = [
        { title: 'Active WorkOrders', component: ActiveWorkOrders },
        { title: 'New Work Order', component: NewWorkOrderForm },
        { title: 'My Profile', component: UserInfoPage }
        // { title: 'Work Order', component: WorkOrder},
        // { title: 'Home', component: HomePage },
        // { title: 'List', component: ListPage },
      ];
    }
  }

  initializeApp() {
    this.platform.ready().then(() => {
      // Okay, so the platform is ready and our plugins are available.
      // Here you can do any higher level native things you might need.
      this.statusBar.styleDefault();
      this.splashScreen.hide();
    });
  }

  presentLogout() { ///<-- call this function straight with static button in html
    let alert = this.alertCtrl.create({
      title: 'Confirm Log Out',
      message: 'Are you sure you want to log out?',
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          handler: () => {
            console.log('Cancel clicked');
          }
        },
        {
          text: 'Log Out',
          handler: () => {
            this.accountService.logout().then((a) => {
              console.log('Logged out');
              this.menuCtrl.close();
            });
          }
        }
      ]
    });
    alert.present();
  }

  openPage(page) {
    // Reset the content nav to have just this page
    // we wouldn't want the back button to show in this scenario
    this.nav.setRoot(page.component);
  }
}
