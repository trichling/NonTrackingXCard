var qrCode;
var qrScanner;

window.qrGenerator = {
    
    initializeQrCode: function (container) {
        qrCode = new QRCode(document.getElementById(container), { width: 340, height: 340 }); 
        qrCode.clear();     
    },

    clearQrCode: function () {
        qrCode.clear();
    },

    generateQrCode: function (data) {
        // stringify json the data
       const jsonData = JSON.stringify(data);
       qrCode.clear();
       qrCode.makeCode(jsonData);
    }
}

window.qrScanHelper = {

    startScan: function (dotNetObject) {
        qrScanner = new QrScanner(
            document.getElementById("qrScanner"),
            result => {
                console.log('decoded qr code:', result)
                dotNetObject.invokeMethodAsync('OnQrCodeScanned', result.data);
            },
            { 
                highlightScanRegion: true,
                highlightCodeOutline: true,
              
            },
        );
        qrScanner.start();
        

    },

     stopScan: function () {
       
        qrScanner.stop();
        

    }
};