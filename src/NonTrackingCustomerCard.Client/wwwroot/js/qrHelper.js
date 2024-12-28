var qrCode;
var qrScanner;

window.qrGenerator = {
    
    qrcodeJs: {
        initializeQrCode: function (container) {
            // qrCode = new QRCode(document.getElementById(container), { width: 340, height: 340 }); 
            // qrCode.clear();     

            var containerElement = document.getElementById(container);
            qrCode = new QRCode(containerElement, { width: containerElement.offsetWidth, height: containerElement.offsetHeight });
            qrCode.clear();

            // Initialize ResizeObserver to watch for changes in the container size
            resizeObserver = new ResizeObserver(entries => {
                for (let entry of entries) {
                    const width = entry.contentRect.width;
                    const height = entry.contentRect.height;
                    qrCode._htOption.width = width;
                    qrCode._htOption.height = height;
                    qrCode.clear();
                    qrCode.makeCode(qrCode._oQRCode);
                    //qrCode.resize(width, height);
                }
            });

            resizeObserver.observe(containerElement);
        },

        clearQrCode: function () {
            qrCode.clear();
        },

        generateQrCode: function (data) {
            // stringify json the data
        const jsonData = JSON.stringify(data);
        qrCode.clear();
        qrCode.makeCode(jsonData);
        },
    },

    initializeQrCode: function (container) {
        var containerElement = document.getElementById(container);
        qrCode = new QRious({element: containerElement, size: 300});

        resizeObserver = new ResizeObserver(entries => {
            for (let entry of entries) {
                const width = entry.contentRect.width;
                const height = entry.contentRect.height;
                var squareSize = Math.min(width, height);
                qrCode.size = squareSize;
            }
        });

        resizeObserver.observe(containerElement);
    },

    clearQrCode: function () {
        qrCode.set({value: ''});
    },

    generateQrCode: function (data) {
        // stringify json the data
       const jsonData = JSON.stringify(data);
       qrCode.set({value: jsonData});
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