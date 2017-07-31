var pubnub;
var channel = "chat";
var username;

function init(publishKey, subscribeKey, authKey, username) {
    pubnub = new PubNub({
        publishKey: publishKey,
        subscribeKey: subscribeKey,
        authKey: authKey,
        uuid: username
    });

    this.username = username;

    addListener();
    subscribe();
}

function addListener() {
    pubnub.addListener({
        status: function (statusEvent) {
            if (statusEvent.category === "PNConnectedCategory") {
                getOnlineUsers();
            }
        },
        message: function (message) {
            var jsonMessage = JSON.parse(message.message);
            var chat = document.getElementById("chat");
            chat.value = chat.value + "\n" + jsonMessage.Username + ": " + jsonMessage.Message;
        },
        presence: function (presenceEvent) {
            if (presenceEvent.action === 'join') {
                PutStatusToChat(presenceEvent.uuid, "joins the channel");
            }
            else if (presenceEvent.action === 'timeout') {
                PutStatusToChat(presenceEvent.uuid, "was disconnected due to timeout");
            }
        }
    });
}

function PutStatusToChat(username, status) {
    var chat = document.getElementById("chat");
    chat.value = chat.value + "\n" + username + " " + status;
}

function subscribe() {
    pubnub.subscribe({
        channels: [channel],
        withPresence: true
    });
}

function getOnlineUsers() {
    pubnub.hereNow({
        channels: [channel],
        includeUUIDs: true,
        includeState: false
    },
        function (status, response) {
            var occupants = response.channels[channel].occupants;
            for (var i = 0; i < occupants.length; i++) {
                var users = document.getElementById("users");
                users.value = users.value + "\n" + occupants[i].uuid;
            }
        });
}

function publish(message) {
    var jsonMessage = {
        "Username": username,
        "Message": message
    };

    var publishConfig = {
        channel: channel,
        message: JSON.stringify(jsonMessage)
    };

    pubnub.publish(publishConfig);
}