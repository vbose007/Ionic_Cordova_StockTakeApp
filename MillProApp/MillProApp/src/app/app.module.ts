import { UserInfoPage } from './../pages/userInfo/userInfo.component';
import { HttpModule } from '@angular/http';
import { WorkOrderService } from '../services/workOrder.service';
import { QrAndBarcodeScannerService } from '../services/qrAndBarcodeScanner.service';
import { APIService } from '../services/APIService/api.service';
import { AccountApiService } from '../services/APIService/account.api.service';
import { UsernameValidator } from '../validators/username.validator';
import { AlertHelper } from '../helpers/alert.helper';
import { IonicStorageModule } from '@ionic/storage';
import { ActiveWorkOrders } from '../pages/activeWorkOrders/activeWorkOrders.component';
import { NewWorkOrderForm } from '../pages/newWorkOrder/newWorkOrderForm.component';
import { WorkOrder } from '../pages/workOrder/workOrder.component';
import { RegistrationForm } from '../pages/registration/registration.form.component';
import { LoginForm } from '../pages/loginForm/loginForm.component';
import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';
import { IonicApp, IonicErrorHandler, IonicModule } from 'ionic-angular';

import { MyApp } from './app.component';
import { HomePage } from '../pages/home/home';
import { ListPage } from '../pages/list/list';

import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { BarcodeScanner } from "@ionic-native/barcode-scanner";
import { RequestResetPasswordForm } from "../pages/requestResetPassword/requestResetPasswordForm.form.component";
@NgModule({
  declarations: [
    MyApp,
    ActiveWorkOrders,
    NewWorkOrderForm,
    WorkOrder,
    RegistrationForm,
    LoginForm,
    UserInfoPage,
    HomePage,
    ListPage,
    RequestResetPasswordForm
  ],
  imports: [
    BrowserModule,
    HttpModule,
    IonicModule.forRoot(MyApp),
    IonicStorageModule.forRoot({
      name: 'millProApp_db',
      //driverOrder: ['sqlite', 'indexeddb', 'websql'] //Use this for actual device
      driverOrder: ['websql', 'indexeddb', 'sqlite'] //Use this for browser testing
    })
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    ActiveWorkOrders,
    NewWorkOrderForm,
    WorkOrder,
    RegistrationForm,
    LoginForm,
    UserInfoPage,
    HomePage,
    ListPage,
    RequestResetPasswordForm
  ],
  providers: [
    StatusBar,
    SplashScreen,
    {provide: ErrorHandler, useClass: IonicErrorHandler},
    QrAndBarcodeScannerService,
    APIService,
    AccountApiService,
    UsernameValidator,
    AlertHelper,
    WorkOrderService,
    BarcodeScanner,
    Storage
  ]
})
export class AppModule {}
