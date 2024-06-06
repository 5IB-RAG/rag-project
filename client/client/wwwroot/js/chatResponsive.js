
const connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

connection.on("SendMessage", function (user, message) {
    const mainDiv = document.createElement("div");
    mainDiv.className = "d-inline-flex flex-column align-self-end";

    const characterPictureDiv = document.createElement("div");
    characterPictureDiv.className = "character-picture-user align-self-end";

    const img = document.createElement("img");
    img.src = "img/user-icon.jpg";
    img.alt = "pupogelatoalcioccolato";

    characterPictureDiv.appendChild(img);

    const messageContainerDiv = document.createElement("div");
    messageContainerDiv.className = "d-inline-flex";

    const messageDiv = document.createElement("div");
    messageDiv.className = "message";

    const messageSpan = document.createElement("span");
    messageSpan.innerHTML = message; // Usa innerHTML per permettere HTML.Raw
    messageDiv.appendChild(messageSpan);

    messageContainerDiv.appendChild(messageDiv);

    mainDiv.appendChild(characterPictureDiv);
    mainDiv.appendChild(messageContainerDiv);
    document.querySelector(".chat").appendChild(mainDiv);
    //Chat

    // Crea il contenitore principale
    const chatMainDiv = document.createElement("div");
    chatMainDiv.className = "d-flex flex-column";

    // Crea la div per l'immagine del bot
    const chatPictureDiv = document.createElement("div");
    chatPictureDiv.className = "character-picture-bot";

    // Crea l'elemento img
    const chatImg = document.createElement("img");
    chatImg.src = "img/logo_full_no_nome.svg";
    chatImg.alt = "pupogelatoalcioccolato";

    // Aggiungi l'immagine alla div del bot
    chatPictureDiv.appendChild(chatImg);

    // Crea la div per la risposta
    const responseContainerDiv = document.createElement("div");
    responseContainerDiv.className = "d-inline-flex";

    const responseDiv = document.createElement("div");
    responseDiv.className = "response";

    // Crea lo span con il testo della risposta
    const responseSpan = document.createElement("span");
    responseSpan.innerHTML = '...'; // Usa innerHTML per permettere HTML.Raw

    // Aggiungi lo span alla div della risposta
    responseDiv.appendChild(responseSpan);

    // Aggiungi la div della risposta al contenitore della risposta
    responseContainerDiv.appendChild(responseDiv);

    // Aggiungi le divs al contenitore principale
    chatMainDiv.appendChild(chatPictureDiv);
    chatMainDiv.appendChild(responseContainerDiv);

    document.querySelector(".chat").appendChild(chatMainDiv);

    document.querySelector(".input-container form input").value = '';
    document.querySelector(".input-container form input").disable();
});

// connection.on("ReceiveMessage", function (user, message) {
//     // const msg = document.createElement("div");
//     // msg.className = user === "User" ? "message d-flex" : "response d-flex justify-content-end";
//     // msg.textContent = `${message}`;
//     document.getElementById("messageInput").enable();
//     //document.getElementById("messagesList").lastChild.textContent = message;
//     //document.getElementById("typingIndicator").style.display = 'none';
// });

connection.start(() => console.log("PARTITO!")).catch(function (err) {
    return console.error(err.toString());
});