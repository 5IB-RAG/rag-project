const copyText = (messageId) => {
    const copyText = document.getElementById(messageId);
    console.log(copyText)

    navigator.clipboard.writeText(copyText.textContent).then(function () {
        alert("Testo copiato negli appunti: " + copyText.textContent)
    }).catch(function (error) {
        console.error("Errore nel copiare il testo: ", error);
    });
}