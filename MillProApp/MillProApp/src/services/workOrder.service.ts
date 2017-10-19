import { WorkOrderData } from '../models/workOrder.data'
import { Storage } from '@ionic/storage';
import { AlertController, Events } from "ionic-angular";
import { Injectable, Inject } from "@angular/core";
import { SQLite } from '@ionic-native/sqlite';
import { EventsConstants } from "../constants/events.constants";
import { APIService } from "./APIService/api.service";
import { Observable } from "rxjs/Observable";
import { Response } from "@angular/http";
import { Subscription } from "rxjs/Subscription";
import { ApiConstants } from "../constants/api.constants";
import "rxjs/add/operator/toPromise";

@Injectable()
export class WorkOrderService {

  private activeWorkOrders: Array<WorkOrderData> = [];

  constructor(private storage: Storage, @Inject(AlertController) private alertCtrl: AlertController, @Inject(Events) private events: Events, @Inject(APIService) private apiService: APIService) {
    var parentScope = this;
    this.getActiveWorkOrders().then((data) => { parentScope.activeWorkOrders = data as Array<WorkOrderData>; });
  }


  private showErrorAlert(msg: string) {
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

  getActiveWorkOrders(): Promise<Array<WorkOrderData>> {
    let workOrders: Array<WorkOrderData> = [];

    var parentScope = this;
    //this._storage.forEach(function (value: any, key: string, index: Number) {
    //    workOrders.push(value);
    //});
    return this.storage.ready().then(() => this.storage.get(ApiConstants.UserName).then((userName) => {

      return this.storage.get("workOrders").then((result) => {

        if (result != null) {
          result.forEach(function (value: any, key: string, index: Number) {
            var workOrder = JSON.parse(value) as WorkOrderData;
            if (workOrder.CreatedByUserId === userName) {
              workOrders.push(workOrder);
            }
          });
        }

        this.activeWorkOrders = workOrders;
        return workOrders;

      });
    }));
  }


  createNew(workOrderNumber: string): Promise<void> {

    if (!workOrderNumber || workOrderNumber.trim() === "") {
      this.showErrorAlert("Please enter a valid Work Order Number");
      return new Promise<void>(null);
    }

    let workOrder = new WorkOrderData(workOrderNumber);


    var parentScope = this;
    return this.storage.ready().then(() => this.storage.get(ApiConstants.UserName).then((userName) => {
      return this.storage.get("workOrders").then(
        (result) => {
          console.log("In here : ", result);
          workOrder.CreatedByUserId = userName;

          if (result != null) {

            var index = result.findIndex(x => JSON.parse(x).WorkOrderNumber === workOrderNumber && JSON.parse(x).CreatedByUserId === userName);

            if (index >= 0) {
              //show alert
              console.log("Existing Record Found !");

              parentScope.showErrorAlert('A Work Order with #' + workOrder.WorkOrderNumber + ' already exists');

            } else {
              console.log("No existing record Found. Creating new Work Order");

              result.push(JSON.stringify(workOrder));
              //create record

              parentScope.storage.set("workOrders", result).then(() => {

                parentScope.activeWorkOrders.push(workOrder);

                parentScope.events.publish(EventsConstants.workOrdersUpdated);

                console.log("Active work orders updated");

              });
            }
          }
          else {
            console.log("No existing record Found. Creating First Work Order");

            let workOrders = [];

            workOrders.push(JSON.stringify(workOrder));

            //create record
            parentScope.storage.set("workOrders", workOrders).then(() => {

              this.events.publish(EventsConstants.workOrdersUpdated);

              console.log("Active work orders updated");

            });
          }

        },
        (error) => {
          console.log(JSON.stringify(error));

        })
    })
    );
  }


  delete(workOrderNumberToDelete: string) {

    var parentScope = this;
    this.storage.ready().then(() => this.storage.get(ApiConstants.UserName).then((userName) => {
      this.storage.get("workOrders").then(
        (result) => {

          if (result != null) {

            var index = result.findIndex(x => JSON.parse(x).WorkOrderNumber === workOrderNumberToDelete);

            if (index < 0) {
              //show alert
              console.log("No Record Found !");
              parentScope.showErrorAlert('Work Order with #' + workOrderNumberToDelete + ' does not exist.');

            } else {
              console.log("Record Found. Deleting work Order");

              result.splice(index, 1);

              //create record
              parentScope.storage.set("workOrders", result).then(() => {

                //this.events.publish(EventsConstants.workOrdersUpdated);

                console.log("Active work orders updated");

              });
            }
            this.events.publish(EventsConstants.workOrdersUpdated);
          }
          else {
            console.log("No records to found for deletion.");

            //show alert
            parentScope.showErrorAlert('Work Order with #' + workOrderNumberToDelete + ' does not exist.');

          }

        },
        (error) => { console.log(JSON.stringify(error)); });
    }));
  }


  update(workOrder: WorkOrderData): Promise<void> {

    var parentScope = this;

    return this.storage.ready().then(() => {
      return this.storage.get(ApiConstants.UserName).then((userName) => {

        return this.storage.ready().then(() => this.storage.get("workOrders").then(
          (result) => {

            if (result != null) {

              var index = result.findIndex(x => JSON.parse(x).WorkOrderNumber === workOrder.WorkOrderNumber && JSON.parse(x).CreatedByUserId === userName);

              if (index < 0) {
                //show alert
                console.log("Record Not Found !");

                parentScope.showErrorAlert('A Work Order #' + workOrder.WorkOrderNumber + ' does not exists');
                //let notFoundAlert = parentScope.alertCtrl.create({
                //    title: 'Error',
                //    message: 'A Work Order #' + workOrder.WorkOrderId + ' does not exists',
                //    buttons: [
                //        {
                //            text: "OK"
                //        }
                //    ]
                //});

                //notFoundAlert.present();
              } else {
                console.log("Record Found. Updating Work Order#" + workOrder.WorkOrderNumber);

                //update record
                result[index] = JSON.stringify(workOrder);

                //parentScope.storage.set("workOrders", result);

                parentScope.storage.set("workOrders", result).then(() => {

                  this.events.publish(EventsConstants.workOrdersUpdated);

                  console.log("Active work orders updated");

                });

                //this.events.publish(EventsConstants.workOrdersUpdated);
              }
            }
            else {
              console.log("No existing records Found.");

              parentScope.showErrorAlert('No Records to Update');

            }



          },
          (error) => { console.log(JSON.stringify(error)); }));
      });
    });
  }

  getActiveWorkOrder(workOrderId: string): Promise<WorkOrderData> {
    return this.storage.ready().then(() => {
      return this.storage.get(ApiConstants.UserName).then((userName) => {
        return this.getActiveWorkOrders().then((data) => {
          if (data != null) {

            let workOrder = data.find(x => x.WorkOrderNumber === workOrderId && x.CreatedByUserId === userName);

            return workOrder;
          }
          return null;
        });
      });
    });
  }

  deleteScannedItem(workOrderNumber: string, qrCode: string) {

    var parentScope = this;
    this.storage.ready().then(() =>
    this.storage.get(ApiConstants.UserName).then((userName) => {
      this.storage.get("workOrders").then(
        (result) => {

          if (result != null) {

            let indexOfWorkOrderToUpdate = result.findIndex(x => JSON.parse(x).WorkOrderNumber === workOrderNumber);

            if (indexOfWorkOrderToUpdate < 0) {
              //show alert
              console.log("No Record Found !");
              parentScope.showErrorAlert('Work Order with #' + workOrderNumber + ' does not exist.');

            } else {
              console.log("Record Found.");
              let workOrderToUpdate = JSON.parse(result[indexOfWorkOrderToUpdate]) as WorkOrderData;

              let indexOfScannedItemToDelete = workOrderToUpdate.Inventory.findIndex(i => i.ItemCode === qrCode);

              if (indexOfScannedItemToDelete >= 0) {

                if (workOrderToUpdate.Inventory[indexOfScannedItemToDelete].Quantity > 1) {
                  console.log("Decrementing Quantity for item#" + qrCode + " from workOrder#" + workOrderToUpdate.WorkOrderNumber);
                  workOrderToUpdate.Inventory[indexOfScannedItemToDelete].Quantity--;
                }
                else {
                  console.log("Deleting item#" + qrCode + " from workOrder#" + workOrderToUpdate.WorkOrderNumber);
                  workOrderToUpdate.Inventory.splice(indexOfScannedItemToDelete, 1);
                }

                result[indexOfWorkOrderToUpdate] = JSON.stringify(workOrderToUpdate);

                //update record

                parentScope.storage.set("workOrders", result).then(() => {

                  this.events.publish(EventsConstants.scannedItemsUpdated);

                  console.log("Active work orders updated");

                });


              } else {
                parentScope.showErrorAlert('Scanned Item#' + qrCode + ' does not exist in Work Order#' + workOrderNumber);
              }

            }
          }
          else {
            console.log("No records to found for deletion.");

            parentScope.showErrorAlert('Work Order with #' + workOrderNumber + ' does not exist.');
          }

        },
        (error) => { console.log(JSON.stringify(error)); });
    }));
  }


  completeWorkOrder(workOrder: WorkOrderData, toEmail: string): Promise<void> {//Subscription {
    let parentScope = this;

    
    return this.storage.get(ApiConstants.UserName).then((userName) => {
      if (workOrder.CreatedByUserId === userName) {
        return this.apiService.postWorkOrder(workOrder, toEmail)
          .then(
          (resp) => {
            parentScope.delete(workOrder.WorkOrderNumber);
            console.log(`Successfully completed workorder#${workOrder.WorkOrderNumber}`);
          },
          (err) => {
            console.log(`Failed to complete workorder#${workOrder.WorkOrderNumber}`);
            parentScope.showErrorAlert(
              `Failed to complete workorder#${workOrder.WorkOrderNumber} : ${JSON.stringify(err)} `);
          });
      }
    });
    //.subscribe(
    //  (resp) => {
    //    parentScope.delete(workOrder.WorkOrderNumber);
    //    console.log(`Successfully completed workorder#${workOrder.WorkOrderNumber}`);
    //  },
    //  (err) => {
    //    console.log(`Failed to complete workorder#${workOrder.WorkOrderNumber}`);
    //    parentScope.showErrorAlert(
    //      `Failed to complete workorder#${workOrder.WorkOrderNumber} : ${JSON.stringify(err)} `);
    //  }
    //);
  }


}
