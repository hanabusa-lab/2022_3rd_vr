var blePlugin = {
    // accessing Bluetooth device
    $devices: {},

    // Check Bluetooth device connection
    IsConnected: function (targetName) {
        console.log('>>> isConnect');

        var target = Pointer_stringify(targetName);
        console.log('target:' + target);

        var result = false;

        if (devices[target]) {
            console.log('device:' + devices[target]);
            result = devices[target].gatt.connected;
        }

        console.log('<<< isConnect');

        return result;
    },

    // connect Bluetooth device
    Connect: function (targetName) {
        console.log('>>> connect');

        var target = Pointer_stringify(targetName);
        console.log('target:' + target);

        var ACCELEROMETER_SERVICE_UUID = 'e95d0753-251d-470a-a062-fa1922dfa9a8';
        var ACCELEROMETER_DATA_CHARACTERISTIC_UUID = 'e95dca4b-251d-470a-a062-fa1922dfa9a8';
        var ACCELEROMETER_PERIOD_CHARACTERISTIC_UUID = 'e95dfb24-251d-470a-a062-fa1922dfa9a8';
        var BUTTON_SERVICE_UUID = 'e95d9882-251d-470a-a062-fa1922dfa9a8';
        var BUTTON_A_STATE_CHARACTERISTIC_UUID = 'e95dda90-251d-470a-a062-fa1922dfa9a8';
        var BUTTON_B_STATE_CHARACTERISTIC_UUID = 'e95dda91-251d-470a-a062-fa1922dfa9a8';
        //追加 aihara
        var UUID_UART_SERVICE = "6e400001-b5a3-f393-e0a9-e50e24dcca9e";
        var UUID_TX_CHAR_CHARACTERISTIC = "6e400002-b5a3-f393-e0a9-e50e24dcca9e";
        var UUID_RX_CHAR_CHARACTERISTIC = '6e400003-b5a3-f393-e0a9-e50e24dcca9e'

        var bluetoothServer;
        var accelerometerService;
        var buttonService;
        //add by aihara
        var uartService;

        // get Bluetooth device
        var options = {
            filters: [
                { namePrefix: 'BBC micro:bit' }
            ],
            //add aihara
            //optionalServices: [ACCELEROMETER_SERVICE_UUID, BUTTON_SERVICE_UUID]
            optionalServices: [UUID_UART_SERVICE]

        };
        navigator.bluetooth.requestDevice(options)
            .then(function (device) {
                console.log('id:' + device.id);
                console.log('name:' + device.name);

                // get notification after disconnect
                device.addEventListener('gattserverdisconnected', function (e) {
                    console.log('gattserverdisconnected');
                    SendMessage(target, 'OnDisconnected');
                });

                // connect to device
                return device.gatt.connect();
            })
            .then(function (server) {
                console.log('connected.');
                devices[target] = server.device;

                // get UART service
                bluetoothServer = server;
                return bluetoothServer.getPrimaryService(UUID_UART_SERVICE);
            })
            .then(function (service) {
                console.log('getPrimaryService');

                uartService = service;
                return uartService.getCharacteristic(UUID_TX_CHAR_CHARACTERISTIC);
            })
            .then(function (characteristic) {
                console.log('getCharacteristic');

                // get UART value
                return characteristic.startNotifications();
            })
            .then(function (characteristic) {
                console.log('startNotifications');

                // get UART value
                characteristic.addEventListener('characteristicvaluechanged', function (ev) {
                    var recvValue = new TextDecoder().decode(ev.target.value).replace(/\r?\n/g, '');
                    console.log("ble input value3=", recvValue);
                    //divide p:0,r:0,f1:0,f2:0
                    var p=0,r=0,f1=0,f2=0;
                    if (recvValue.charAt(0) == "p") {
                        var vals = recvValue.split(',');
                        console.log("vals=", vals)
                        for(var val of vals){
                            console.log("val=", val)
                            if(val.startsWith("p:")){
                                p = val.substring(2,val.length);
                                //parseFloat()    
                            }else if(val.startsWith("r:")){
                                r = val.substring(2,val.length);
                            }
                        }
                        console.log("p=",p, " r=",r);
                        SendMessage(target, 'OnGloveAngleChanged', JSON.stringify({p: p, r: r}));
                    }
                    if (recvValue.charAt(0) == "f") {
                        var vals = recvValue.split(',');
                        console.log("vals=", vals)
                        for(var val of vals){
                            console.log("val=", val)
                            if(val.startsWith("f1:")){
                                f1 = val.substring(3,val.length);
                            }else if(val.startsWith("f2:")){
                                f2 = val.substring(3,val.length);
                            }
                        }
                        console.log("f1=",f1, " f2=",f2);
                        SendMessage(target, 'OnGloveFingureChanged', JSON.stringify({f1: f1, f2: f2}));
                    }
             
                    //SendMessage(target, 'OnAccelerometerChanged', x);
                });
            })
            .catch(function (err) {
                console.log('err:' + err);

                if (devices[target]) {
                    if (devices[target].gatt.connected) {
                        devices[target].gatt.disconnect();
                    }
                    delete devices[target];
                }
            });

        console.log('<<< connect');
    },

    // disconnect bluetooth device
    Disconnect: function (targetName) {
        console.log('>>> disconnect');

        var target = Pointer_stringify(targetName);
        console.log('target:' + target);

        if (devices[target]) {
            console.log('device:' + devices[target]);
            // disconnect if connecting
            devices[target].gatt.disconnect();
            delete devices[target];
        }

        console.log('<<< disconnect');
    }
};
autoAddDeps(blePlugin, '$devices');
mergeInto(LibraryManager.library, blePlugin);
