var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();
connection.on("ReceiveMessage", function (user, message, time, messageId, profileImageUrl) {
    var currentUser = document.getElementById("userInput").value;
    var roomId = document.getElementById("roomIdInput").value;
    var isMine = (user === currentUser);
    
    var userInitial = user.charAt(0).toUpperCase();
    var profileImageHtml = profileImageUrl 
        ? `<img src="${profileImageUrl}" alt="${user}" class="rounded-circle me-2" style="width: 24px; height: 24px; object-fit: cover;" />`
        : `<div class="rounded-circle me-2 d-flex align-items-center justify-content-center bg-indigo text-white fw-bold" style="width: 24px; height: 24px; font-size: 12px;">${userInitial}</div>`;
    
    var nameHtml = (!isPrivateRoom && !isMine)
        ? `<div class="d-flex align-items-center mb-2">${profileImageHtml}<div class="fw-bold small text-primary">${user}</div></div>`
        : "";

    var msgHtml = `
        <div class="mb-3 d-flex ${isMine ? 'justify-content-end' : 'justify-content-start'}" id="msg-${messageId}">
            <div class="bubble ${isMine ? 'bubble-mine shadow-lg' : 'bubble-theirs shadow'}">
                ${nameHtml}
                <div class="message-content">${message}</div>
                <div class="timestamp-text d-flex align-items-center justify-content-end mt-1">
                    <span>${time}</span>
                    ${isMine ? `<i class="bi bi-check2-all ms-1 seen-icon" id="seen-${messageId}" style="color: #0084FF;"></i>` : ""}
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