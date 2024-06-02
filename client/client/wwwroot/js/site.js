function openFile(filename) {
    // Simulazione di apertura del file
    alert("Apertura del file: " + filename);
}

document.getElementById("chatBtn").addEventListener("click", function () {
    document.getElementById("chatSection").style.display = "block";
    document.getElementById("fileSection").style.display = "none";
    document.getElementById("chatBtn").style.transform = 'translateZ(200px)';
    document.getElementById("fileBtn").style.transform = 'translateZ(-200px)';
});

document.getElementById("fileBtn").addEventListener("click", function () {
    document.getElementById("fileSection").style.display = "block";
    document.getElementById("chatSection").style.display = "none";
    document.getElementById("chatBtn").style.transform = 'translateZ(-200px)';
    document.getElementById("fileBtn").style.transform = 'translateZ(200px)';
});

document.getElementById("chatBtn1").addEventListener("click", function () {
    document.getElementById("chatSection1").style.display = "block";
    document.getElementById("fileSection1").style.display = "none";
    document.getElementById("chatBtn1").style.transform = 'translateZ(200px)';
    document.getElementById("fileBtn1").style.transform = 'translateZ(-200px)';
});

document.getElementById("fileBtn1").addEventListener("click", function () {
    document.getElementById("fileSection1").style.display = "block";
    document.getElementById("chatSection1").style.display = "none";
    document.getElementById("chatBtn1").style.transform = 'translateZ(-200px)';
    document.getElementById("fileBtn1").style.transform = 'translateZ(200px)';
});

document.getElementById('attachButton').addEventListener('click', function () {
    document.getElementById('fileInput').click();
});

document.addEventListener('DOMContentLoaded', function () {
    const newChatBtn = document.getElementById('newChatBtn');
    const chatSection = document.getElementById('chatSection');

    newChatBtn.addEventListener('click', function () {
        // Creare un nuovo elemento di chat
        const newChat = document.createElement('div');
        newChat.classList.add('file');

        // Creare un nuovo pulsante di chat
        const newChatButton = document.createElement('button');
        newChatButton.textContent = `Nuova chat ${document.querySelectorAll('#chatSection .file').length}`;

        // Aggiungere il pulsante alla nuova chat
        newChat.appendChild(newChatButton);

        // Aggiungere la nuova chat alla sezione delle chat
        chatSection.appendChild(newChat);
    });

    // Per l'offcanvas
    const newChatBtnOffcanvas = document.getElementById('newChatBtnOffcanvas');
    const chatSection1 = document.getElementById('chatSection1');

    newChatBtnOffcanvas.addEventListener('click', function () {
        // Creare un nuovo elemento di chat
        const newChat = document.createElement('div');
        newChat.classList.add('file');

        // Creare un nuovo pulsante di chat
        const newChatButton = document.createElement('button');
        newChatButton.textContent = `Nuova chat ${document.querySelectorAll('#chatSection1 .file').length + 1}`;

        // Aggiungere il pulsante alla nuova chat
        newChat.appendChild(newChatButton);

        // Aggiungere la nuova chat alla sezione delle chat
        chatSection1.appendChild(newChat);
    });
});




