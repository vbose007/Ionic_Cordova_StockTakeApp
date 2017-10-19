import { DateTime } from 'ionic-angular';

import { InventoryItemData } from "./inventoryItem.data";

export class WorkOrderData {

    Inventory: InventoryItemData[] = [];

    Id: number = 0;

    CreatedByUserId: string = null;

    CreaatedForCompanyId: string = null;

    CreatedDate: string = null;

    UserName:string = null;

    constructor(public WorkOrderNumber: string) { }

}
