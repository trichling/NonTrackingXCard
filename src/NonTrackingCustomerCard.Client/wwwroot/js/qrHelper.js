var qrCode;
var qrScanner;

window.qrGenerator = {
    
    initializeQrCode: function (container) {
        qrCode = new QRCode(document.getElementById(container), { width: 600, height: 600 });      
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
                highlightCodeOutline: true
            },
        );
        qrScanner.start();
        

    },

     stopScan: function () {
       
        qrScanner.stop();
        

    }
};