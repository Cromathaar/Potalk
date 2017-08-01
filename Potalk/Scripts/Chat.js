var pubnub;
var chatChannel;
var textToSpeechChannel;
var username;

function init(publishKey, subscribeKey, authKey, username, chatChannel, textToSpeechChannel) {
    pubnub = new PubNub({
        publishKey: publishKey,
        subscribeKey: subscribeKey,
        authKey: authKey,
        uuid: username
    });

    this.username = username;
    this.chatChannel = chatChannel;
    this.textToSpeechChannel = textToSpeechChannel;

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
            if (message.channel === chatChannel) {
                var jsonMessage = JSON.parse(message.message);
                var chat = document.getElementById("chat");
                if (chat.value !== "") {
                    chat.value = chat.value + "\n";
                    chat.scrollTop = chat.scrollHeight;
                }

                chat.value = chat.value + jsonMessage.Username + ": " + jsonMessage.Message;
            }
            else if (message.channel === textToSpeechChannel) {
                if (message.publisher !== username) {
                    var audio = new Audio(message.message.speech);
                    audio.play();
                }
            }
        },
        presence: function (presenceEvent) {
            if (presenceEvent.channel === chatChannel) {
                if (presenceEvent.action === 'join') {
                    if (!UserIsOnTheList(presenceEvent.uuid)) {
                        AddUserToList(presenceEvent.uuid);
                    }

                    PutStatusToChat(presenceEvent.uuid, "joins the channel");
                }
                else if (presenceEvent.action === 'timeout') {
                    if (UserIsOnTheList(presenceEvent.uuid)) {
                        RemoveUserFromList(presenceEvent.uuid);
                    }

                    PutStatusToChat(presenceEvent.uuid, "was disconnected due to timeout");
                }
            }
        }
    });
}

function getOnlineUsers() {
    pubnub.hereNow({
        channels: [chatChannel],
        includeUUIDs: true,
        includeState: false
    },
        function (status, response) {
            var occupants = response.channels[chatChannel].occupants;
            for (var i = 0; i < occupants.length; i++) {
                if (!UserIsOnTheList(occupants[i].uuid)) {
                    AddUserToList(occupants[i].uuid);
                }
            }
        });
}

function UserIsOnTheList(username) {
    var userFound = false;

    for (i = 0; i < document.getElementById("chatusers").length; ++i) {
        if (document.getElementById("chatusers").options[i].value === username) {
            userFound = true;
            break;
        }
    }

    return userFound;
}

function RemoveUserFromList(username) {
    var chatusers = document.getElementById("chatusers");

    for (var i = 0; i < chatusers.length; i++) {
        if (chatusers.options[i].value === username)
            chatusers.remove(i);
    }
}

function AddUserToList(username) {
    var chatusers = document.getElementById("chatusers");
    var option = document.createElement("option");
    option.text = username;
    chatusers.add(option);
}

function PutStatusToChat(username, status) {
    var chat = document.getElementById("chat");
    chat.value = chat.value + "\n" + username + " " + status;
}

function subscribe() {
    pubnub.subscribe({
        channels: [chatChannel, textToSpeechChannel],
        withPresence: true
    });
}

function publish(message) {
    var jsonMessage = {
        "Username": username,
        "Message": message
    };

    var publishConfig = {
        channel: chatChannel,
        message: JSON.stringify(jsonMessage)
    };

    pubnub.publish(publishConfig);

    jsonMessage = {
        "text": message
    };

    publishConfig = {
        channel: textToSpeechChannel,
        message: jsonMessage
    };

    pubnub.publish(publishConfig);
}