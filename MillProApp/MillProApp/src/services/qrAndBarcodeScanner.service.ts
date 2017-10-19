import { BarcodeScanner } from '@ionic-native/barcode-scanner';

import { Injectable } from "@angular/core";

@Injectable()
export class QrAndBarcodeScannerService {

  constructor(private barcodeScanner: BarcodeScanner){

  }
    scan = function () {

        let scanData: BarcodeData;

      return this.barcodeScanner.scan()
        .then((result) => {
          if (!result.cancelled) {
            const barcodeData = new BarcodeData(result.text, result.format);

            scanData = barcodeData;
            //alert('scanData : ' + scanData.text);
            return scanData;
          }
        })
        .catch((err) => {
          alert(err);
        });


    }
}

export class BarcodeData {
    constructor(
        public text: string,
        public format: string
    ) { }
}
