import { AlertController } from "ionic-angular";
import { Injectable } from "@angular/core";

@Injectable()
export class AlertHelper {

  private alertCtrl: AlertController = null;

  constructor(alertCt : AlertController) { this.alertCtrl = alertCt; }

  public showErrorAlert(msg: string) {
    let alert = this.alertCtrl.create({
      title: 'Error',
      message: msg,
      buttons: [
        {
          text: "OK"
        }
      ]
    });

    alert.present();
  }
}
