const { Timestamp } = require('google-protobuf/google/protobuf/timestamp_pb');
const { ReadingPacket, ReadingMessage } = require('./meterreader_pb.js');
const { ReadingStatus } = require('./enums_pb.js');
const { MeterReadingServiceClient } = require('./meterreader_grpc_web_pb.js');

const theLog = document.getElementById('theLog');
const theButton = document.getElementById('theButton');

function addToLog(msg) {
    const div = document.createElement('div');
    div.innerText = msg;
    theLog.appendChild(div);
}

theButton.addEventListener('click', () => {
    try {
        addToLog('Starting service call !!!')
        const packet = new ReadingPacket();
        packet.setSuccessful(ReadingStatus.SUCCESS);

        const reading = new ReadingMessage();
        reading.setCustomerid(1);
        reading.setReadingvalue(1000);

        const time = new Timestamp();
        const now = Date.now();
        time.setSeconds(Math.round(now / 1000));

        reading.setReadingtime(time);

        reading.setSuccessful()

        packet.addReadings(reading);

        addToLog('Calling service');
        const client = new MeterReadingServiceClient(window.location.origin);

        client.addReading(packet, {}, function (error, response) {
            if (error)
                addToLog(`Error: ${error}`);
            else
                addToLog(`Success: ${response.getNotes()}`);
        })
    } catch (e) {
        addToLog('Exception has occured')
        addToLog(e);
    }
})