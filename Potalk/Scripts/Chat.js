var pubnub;
var channel = "chat";
var nickname;

function initPubNub(publishKey, subscribeKey, authKey, username) {
    pubnub = new PubNub({
        publishKey: publishKey,
        subscribeKey: subscribeKey,
        authKey: authKey
    });

    nickname = username;

    addListener();
    subscribe();
}

function addListener() {
    pubnub.addListener({
        status: function (statusEvent) {
            if (statusEvent.category === "PNConnectedCategory") {
                publishService(nickname + " joins the channel");
            }
        },
        message: function (message) {
            var jsonMessage = JSON.parse(message.message);
            var chat = document.getElementById("chat");

            if (jsonMessage.IsService) {
                chat.value = chat.value + "\n" + jsonMessage.Message;
            }
            else {
                chat.value = chat.value + "\n" + jsonMessage.Nickname + ": " + jsonMessage.Message;
            }
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
    var jsonMessage = {
        "Nickname": nickname,
        "Message": message,
        "IsService": false
    };

    var publishConfig = {
        channel: channel,
        message: JSON.stringify(jsonMessage)
    };

    pubnub.publish(publishConfig, function (status, response) {
        console.log(status, response);
    });
}

function publishService(message) {
    var jsonMessage = {
        "Nickname": nickname,
        "Message": message,
        "IsService": true
    };

    var publishConfig = {
        channel: channel,
        message: JSON.stringify(jsonMessage)
    };

    pubnub.publish(publishConfig, function (status, response) {
        console.log(status, response);
    });
}