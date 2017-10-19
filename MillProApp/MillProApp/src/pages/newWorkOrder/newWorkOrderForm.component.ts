import { ActiveWorkOrders } from './../activeWorkOrders/activeWorkOrders.component';
import { Component, Inject } from '@angular/core';
import { NavController } from 'ionic-angular';
import{Events} from 'ionic-angular';
import {WorkOrderData} from '../../models/workOrder.data';
import { WorkOrderService } from '../../services/workOrder.service';
import {EventsConstants} from '../../constants/events.constants';

@Component({
    selector: 'new-work-order-form',
    templateUrl: 'newWorkOrderForm.html'
})
export class NewWorkOrderForm {

    newWorkOrder: WorkOrderData;

    constructor(private nav: NavController, private events: Events, /*@Inject(WorkOrderService)*/ private workOrderService: WorkOrderService) {
        this.newWorkOrder = new WorkOrderData("");
    }

    createNew() {
        this.workOrderService.createNew(this.newWorkOrder.WorkOrderNumber).then(() => {
            this.events.publish(EventsConstants.workOrdersUpdated);
            this.goToActiveWorkOrdersPage();
        });
    }

    cancel() {
      this.goToActiveWorkOrdersPage();
    }

    goToActiveWorkOrdersPage(){
      if(this.nav.canGoBack()){
        this.nav.pop();
      }
      else{
        this.nav.push(ActiveWorkOrders);
      }

    }
}
