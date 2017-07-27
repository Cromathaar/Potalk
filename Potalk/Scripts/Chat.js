var pubnub;
var channel = "chat";

function init(publishKey, subscribeKey, authKey) {
    pubnub = new PubNub({
        publishKey: publishKey,
        subscribeKey: subscribeKey,
        authKey: authKey
    });
}

function addListener() {
    pubnub.addListener({
        status: function (statusEvent) {
        },
        message: function (message) {
            var chat = document.getElementById("chat");
            chat.value = chat.value + "\n" + message.message;
        },
        presence: function (presenceEvent) {
            // handle presence
        }
    });
}

function subscribe() {
    pubnub.subscribe({
        channels: [channel]
    });
}

function publish(message) {
    var publishConfig = {
        channel: channel,
        message: message
    };

    pubnub.publish(publishConfig, function (status, response) {
        console.log(status, response);
    });
}