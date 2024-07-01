const copyText = (messageId) => {
    const copyText = document.getElementById(messageId);
    console.log(copyText)

    navigator.clipboard.writeText(copyText.textContent).then(function () {
        Swal.fire({
            icon: 'success',
            title: 'Testo copiato',
            background: '#222',
            color: '#fff',
            showConfirmButton: false,
            timer: 1500
        });
    }).catch(function (error) {
        Swal.fire({
            icon: 'error',
            title: 'Errore nel copiare il testo',
            showConfirmButton: false,
            timer: 1500
        });
        console.error("Errore nel copiare il testo: ", error);
    });
}