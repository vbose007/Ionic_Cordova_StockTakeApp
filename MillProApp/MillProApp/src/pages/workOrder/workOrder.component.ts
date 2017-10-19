import { NavController, NavParams, AlertController, Events } from 'ionic-angular';
import { Component, Inject } from '@angular/core';
//import { WorkOrderService } from "../../services/workOrder.service";
import { WorkOrderData } from "../../models/workOrder.data";
import { BarcodeData, QrAndBarcodeScannerService } from "../../services/qrAndBarcodeScanner.service";
import { InventoryItemData } from "../../models/inventoryItem.data";
import { WorkOrderService } from "../../services/workOrder.service";
import { EventsConstants } from "../../constants/events.constants";


@Component({
    selector:'work-order',
    templateUrl: 'workOrder.html',
    providers: [QrAndBarcodeScannerService ]
})
export class WorkOrder{

    title: string;
    workOrderData: WorkOrderData;
    scannedItems: InventoryItemData[];
    email: string;
    validEmail: boolean = true;

    constructor(private nav: NavController, navParams: NavParams, private scanner: QrAndBarcodeScannerService, private workOrderService: WorkOrderService, private alertCtrl : AlertController, private events: Events) {
        this.workOrderData = navParams.get('workOrder');

        if (this.workOrderData) {
            this.title = 'Work Order #' + this.workOrderData.WorkOrderNumber;
        }

        this.events.subscribe(EventsConstants.scannedItemsUpdated, () => { this.refreshWorkOrder(); });
    }

    scan() {

        let scanData: any;

        let self = this;
        this.scanner.scan().then(
            function (result) {
                scanData = result;

                if (scanData != null) {

                 //   alert('received scanData : ' + scanData);
                    let productData: InventoryItemData = new InventoryItemData();

                    productData.ItemCode = scanData.text;

                    var existingItemIndex = self.workOrderData.Inventory.findIndex(x => x.ItemCode === productData.ItemCode);

                    //If item with same code exists then update qty
                    if (existingItemIndex >= 0) {
                        self.workOrderData.Inventory[existingItemIndex].Quantity++;
                        //alert("Quantity updated");
                    }
                    //else add as new item
                    else {
                        self.workOrderData.Inventory.push(productData);
                        //alert("new item added");
                    }

                    self.workOrderService.update(self.workOrderData).then(() => { console.log("Scanned products for WorkOrder#"+self.workOrderData.WorkOrderNumber+" have been updated with a new item.");});
                }
            });
    }


    refreshWorkOrder() {
        this.workOrderService.getActiveWorkOrder(this.workOrderData.WorkOrderNumber).then((data) => { this.workOrderData = data as WorkOrderData; });
    }

    deleteScannedItem(qrCode: string) {
        this.workOrderService.deleteScannedItem(this.workOrderData.WorkOrderNumber, qrCode);
    }

    confirmDeleteScannedItem(qrCode: string) {
        let deleteAlert = this.alertCtrl.create({
            title: 'Confirm Delete',
            message: 'Do you want to delete Item #'+qrCode+' ?',
            buttons: [
                {
                    text: 'Cancel',
                    role: 'cancel',
                    handler: () => {
                        console.log('delete item#'+qrCode+' from work order# ' + this.workOrderData.WorkOrderNumber + ' cancelled.');
                    }
                },
                {
                    text: 'Delete',
                    handler: () => {
                        console.log('delete item#' + qrCode +' from work order# ' + this.workOrderData.WorkOrderNumber + ' Confirmed.');
                        this.deleteScannedItem(qrCode);
                    }
                }
            ]
        });
        deleteAlert.present();
    }


    complete() {
      let parentScope = this;
      this.workOrderService.completeWorkOrder(this.workOrderData, this.email).then(()=> {parentScope.nav.pop()});
      //.subscribe(
      //    res => {
      //        console.log("Successfully completed workorder#" + this.workOrderData.WorkOrderNumber);
      //    },
      //    err => {
      //        console.log("Failed to complete workorder#" + this.workOrderData.WorkOrderNumber);
      //    });
    }


    isValidEmail(): boolean {

      var EMAIL_REGEXP = /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)+$/;

      if (this.email && this.email !== "" && (this.email.length <= 5 || !EMAIL_REGEXP.test(this.email))) {
        this.validEmail = false;
        return false;
      }

      return true;
    }
}
