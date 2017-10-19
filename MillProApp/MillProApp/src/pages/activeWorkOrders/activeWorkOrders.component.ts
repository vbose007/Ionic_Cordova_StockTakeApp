import { Component, Inject} from '@angular/core';
import { WorkOrderData } from '../../models/workOrder.data';
import { NavController, NavParams, Events, AlertController} from 'ionic-angular';
import { NewWorkOrderForm} from '../newWorkOrder/newWorkOrderForm.component';
import { WorkOrderService } from '../../services/workOrder.service';
import { EventsConstants } from "../../constants/events.constants";
import { WorkOrder } from "../workOrder/workOrder.component";
import { AccountApiService } from "../../services/APIService/account.api.service";
import { LoginForm } from "../loginForm/loginForm.component";


@Component({
    selector: 'active-work-orders',
    templateUrl: 'activeWorkOrders.html'
})
export class ActiveWorkOrders{

    activeWorkOrders: Array<WorkOrderData>;

    constructor(@Inject(NavController) private nav: NavController, @Inject(NavParams) navParams: NavParams, @Inject(Events) private events: Events, @Inject(AlertController) private alertCtrl: AlertController,  private workOrderService: WorkOrderService, @Inject(AccountApiService) private accountApi : AccountApiService) {
        let newWorkOrder: WorkOrderData = navParams.get('newWorkOrder');
        this.getWorkOrderList();

        this.events.subscribe(EventsConstants.workOrdersUpdated, () => { this.getWorkOrderList(); });

        //this.events.subscribe(EventsConstants.loggedOut, () => { this.nav.push(LoginForm); });
        // AccountApiService.isLoggedIn$.subscribe((loggedIn) => {
        //   if (!loggedIn) {
        //     this.nav.push(LoginForm);
        //   }
        // });
    }

    getWorkOrderList() {
        //this.activeWorkOrders = this.workOrderService.getActiveWorkOrders();
        var parentscope = this;
        this.workOrderService.getActiveWorkOrders().then((data) => {
            parentscope.activeWorkOrders = data as Array<WorkOrderData>;
        });

    }

    newWorkOrder() {
      this.nav.push(NewWorkOrderForm);
    }

    delete(workOrderData: WorkOrderData) {
        this.workOrderService.delete(workOrderData.WorkOrderNumber);
    }

    openWorkOrder(workOrderData: WorkOrderData) {

        this.nav.push(WorkOrder, { workOrder: workOrderData });
    }


    confirmDelete(workOrderData: WorkOrderData) {
        let deleteAlert = this.alertCtrl.create({
            title: 'Confirm Delete',
            message: 'Do you want to delete this work order?',
            buttons: [
                {
                    text: 'Cancel',
                    role: 'cancel',
                    handler: () => {
                        console.log('delete work order# ' + workOrderData.WorkOrderNumber + ' cancelled.');
                    }
                },
                {
                    text: 'Delete',
                    handler: () => {
                        console.log('delete work order# ' + workOrderData.WorkOrderNumber + ' Confirmed.');
                        this.delete(workOrderData);
                    }
                }
            ]
        });
        deleteAlert.present();
    }

}
