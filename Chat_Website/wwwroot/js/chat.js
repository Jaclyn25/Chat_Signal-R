var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();
connection.on("ReceiveMessage", function (user, message, time, messageId) {
    var currentUser = document.getElementById("userInput").value;
    var roomId = document.getElementById("roomIdInput").value;
    var isMine = (user === currentUser);
    
    var nameHtml = (!isPrivateRoom && !isMine)
        ? `<div class="fw-bold small mb-1 text-primary">${user}</div>`
        : "";

    var msgHtml = `
        <div class="mb-3 d-flex ${isMine ? 'justify-content-end' : 'justify-content-start'}" id="msg-${messageId}">
            <div class="bubble ${isMine ? 'bubble-mine shadow-lg' : 'bubble-theirs shadow'}">
                ${nameHtml}
                <div class="message-content">${message}</div>
                <div class="timestamp-text d-flex align-items-center justify-content-end mt-1">
                    <span>${time}</span>
                    ${isMine ? `<i class="bi bi-check2 ms-1 seen-icon" id="seen-${messageId}"></i>` : ""}
                </div>
            </div>
        </div>`;
    $("#messagesList").append(msgHtml);
    
    if (!isMine) {
        connection.invoke("MarkAsSeen", messageId, roomId).catch(err => console.error(err));
    }

    var objDiv = document.getElementById("messagesList");
    objDiv.scrollTop = objDiv.scrollHeight;
});

connection.on("MessageSeen", function (messageId) {
    var icon = document.getElementById("seen-" + messageId);
    if (icon) {
        icon.classList.remove("bi-check2");
        icon.classList.add("bi-check2-all", "text-info");
    }
});

var typingTimer;
connection.on("UserTyping", function (user) {
    $("#typingIndicator").text(user + " is typing...").fadeIn();
    clearTimeout(typingTimer);
    typingTimer = setTimeout(function () {
        $("#typingIndicator").fadeOut();
    }, 2000);
});

connection.on("UserStatusUpdate", function (userId, isOnline) {
    var statusDot = $(".status-dot[data-user-id='" + userId + "']");
    if (isOnline) {
        statusDot.removeClass("bg-secondary").addClass("bg-success shadow-success");
    } else {
        statusDot.removeClass("bg-success shadow-success").addClass("bg-secondary");
    }
});

connection.start().then(function () {
    console.log("Connected to Hub Successfully!");
    var roomId = document.getElementById("roomIdInput").value;
    connection.invoke("JoinRoom", roomId).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error("Connection failed: ", err.toString());
});
document.getElementById("messageInput").addEventListener("input", function () {
    var roomId = document.getElementById("roomIdInput").value;
    connection.invoke("SendTyping", roomId).catch(function (err) {
        return console.error(err.toString());
    });
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    var roomId = document.getElementById("roomIdInput").value;

    if (message.trim() !== "") {
        connection.invoke("SendMessage", message, roomId).then(function () {
            document.getElementById("messageInput").value = ""; // تفريغ خانة الكتابة
        }).catch(function (err) {
            return console.error(err.toString());
        });
    }
    event.preventDefault(); 
});