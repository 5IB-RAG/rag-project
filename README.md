# Ermes Project

## Introduzione

Il progetto Ermes è il frutto dell'impegno e della dedizione della classe 5iB della scuola Iss Alessandro Greppi di Monticello Brianza (LC). Questo progetto, concepito come una sfida finale per concludere l'anno scolastico, ha visto la convergenza di conoscenze e competenze in diverse discipline, dalla programmazione informatica alla gestione documentale.

## Descrizione del Progetto

Ermes è una piattaforma intelligente e versatile, progettata per facilitare l'accesso e la comprensione dei documenti mediante l'uso di tecnologie all'avanguardia. Il fulcro del sistema è l'intelligenza artificiale, che analizza e interpreta i documenti caricati dagli utenti per fornire risposte dettagliate e contestualizzate.

## Tecnologie Utilizzate

- **Backend:** C# Ef-Core
- **Frontend:** HTML, CSS, JavaScript
- **Cloud Services:** Azure

## Caratteristiche Principali

### Caricamento dei File

La piattaforma supporta una vasta gamma di documenti, tra cui testi, presentazioni, fogli di calcolo e molto altro ancora. Gli utenti possono caricare questi file attraverso un'interfaccia intuitiva e user-friendly che accetta vari formati di file, rendendo il processo di caricamento semplice e diretto.

### Suddivisione in Chunk

I documenti caricati sono automaticamente suddivisi in "chunk", ovvero blocchi di testo di dimensioni gestibili. Questa suddivisione facilita l'elaborazione e l'analisi dei contenuti da parte dell'intelligenza artificiale, migliorando l'efficienza e la precisione delle risposte fornite.

### Embedding con Azure

La tecnologia Azure viene utilizzata per effettuare l'embedding dei documenti. Questo processo consente di rappresentare semanticamente i contenuti in modo accurato, facilitando l'analisi e l'estrazione delle informazioni rilevanti.

### Archiviazione nel Database

Tutti i documenti caricati, insieme alla loro rappresentazione tramite embedding, sono archiviati in un robusto database. Questo garantisce un accesso rapido e affidabile ai dati, oltre a permettere una facile gestione e ricerca dei documenti.

## Funzionalità Avanzate della Chat

### Interazione Naturale

Gli utenti possono interagire con il sistema attraverso una chat conversazionale, che permette di inviare domande e richieste in linguaggio naturale. Questo elimina la necessità di competenze tecniche particolari, rendendo l'interazione accessibile a tutti.

### Analisi del Contesto

Il sistema è in grado di comprendere il contesto della conversazione analizzando i messaggi precedenti. Questo permette di fornire risposte più precise e contestualizzate, migliorando la qualità dell'interazione utente-sistema.

### Feedback Intelligente

Ermes fornisce feedback intelligente agli utenti, suggerendo documenti correlati o fornendo chiarimenti su concetti specifici. Questo aiuta a migliorare la comprensione e l'efficacia dell'interazione.

### Tracciamento delle Fonti

Ogni risposta fornita dal sistema è accompagnata dalle informazioni sulla fonte da cui è stata ottenuta. Questo consente agli utenti di valutare l'affidabilità e la pertinenza delle informazioni ricevute.

## Autenticazione Utente e Gestione della Sicurezza

### Token di Accesso Sicuro

Per garantire la sicurezza e la privacy degli utenti, viene generato un token di accesso univoco per ciascun utente al momento dell'accesso. Questo token ha una durata di validità limitata a 12 ore e viene rinnovato automaticamente per garantire un accesso continuo e sicuro.

## Server Remoto e Architettura del Sistema

### Architettura Scalabile e Distribuita

Il sistema è basato su un'architettura scalabile e distribuita, progettata per gestire carichi di lavoro variabili. Questo garantisce prestazioni ottimali anche in condizioni di traffico elevato, assicurando che il sistema rimanga reattivo e affidabile.

### Monitoraggio e Manutenzione

Ermes è dotato di strumenti avanzati per il monitoraggio e la manutenzione, che consentono di identificare e risolvere tempestivamente eventuali problemi. Questo garantisce un funzionamento fluido e affidabile del sistema nel tempo.

## Principali Collaboratori

## Embedding

Questa sezione si è occupata dell'implementazione della tecnologia Azure per l'embedding dei documenti, rappresentando semanticamente i contenuti per facilitarne l'analisi e l'estrazione delle informazioni rilevanti.

- [Citterio Giorgio](https://github.com/GiorgioCitterio)
- [Cagliani Cristian](https://github.com/CristianCagliani)
- [Passoni Marco](https://github.com/MarcoPassoni)

## Database

Il team responsabile del database ha curato l'archiviazione dei documenti e delle loro rappresentazioni tramite embedding, garantendo un accesso rapido e affidabile ai dati, oltre a permettere una facile gestione e ricerca dei documenti.

- [Fikri Zakaria](https://github.com/ZakariaFikri05)
- [Ertola Andrea](https://github.com/AndreaErtola)
- [Arosio Alex](https://github.com/EuRedBoy)
- [Galimberti Daniele](https://github.com/DanieleGalimberti)

## Front-End

Questa squadra ha sviluppato l'interfaccia utente, rendendo il caricamento dei file semplice e diretto, e ha assicurato che l'interazione con la piattaforma fosse intuitiva e user-friendly.

- [Formenti Davide](https://github.com/DavForme)
- [Ghisoni Marco](https://github.com/MarcoGhisoni)
- [Locatelli Daniele](https://github.com/LocatelliDaniele)
- [Vaccarella Alessio](https://github.com/AlessioVaccarella)

## Conversazione

Il team ha lavorato sull'interfaccia conversazionale, permettendo agli utenti di interagire con il sistema tramite domande e richieste in linguaggio naturale, migliorando l'interazione utente-sistema.

- [Galimberti Daniele](https://github.com/DanieleGalimberti)
- [Passoni Marco](https://github.com/MarcoPassoni)
- [Cagliani Cristian](https://github.com/CristianCagliani)

## Parsing

Il responsabile di questa sezione ha implementato la suddivisione dei documenti in chunk, facilitando l'elaborazione e l'analisi dei contenuti da parte dell'intelligenza artificiale.

- [Panzeri Pietro](https://github.com/PietroPanzeri)
- [Elkhiraoui Adam](https://github.com/AdamElkhiraoui)
- [Panzeri Andrea](https://github.com/AndreaPanzeri)

## Autenticazione

Questa sezione ha gestito la sicurezza e la privacy degli utenti, implementando la generazione di token di accesso unici per garantire un accesso continuo e sicuro.

- [Citterio Giorgio](https://github.com/GiorgioCitterio)
- [Arosio Alex](https://github.com/EuRedBoy)

## Amministrazione Progetto

Il team di amministrazione del progetto ha coordinato tutte le attività, assicurando che ogni parte del progetto procedesse senza intoppi e che le scadenze venissero rispettate.

- [Arosio Alex](https://github.com/AlexArosio)
- [Panzeri Pietro](https://github.com/PietroPanzeri)

## Amministrazione Github

Il responsabile dell'amministrazione GitHub ha gestito il repository del progetto, coordinando merge, pull request, gestione dei branch e risoluzione dei conflitti per mantenere il progetto organizzato e aggiornato.

- [Passoni Marco](https://github.com/MarcoPassoni)

# Contributi

Il progetto "Ermes" è un'iniziativa collaborativa che accoglie con entusiasmo il contributo della community. Siamo grati per ogni tipo di contributo, che vada dalla segnalazione di bug e problemi alla proposta di nuove funzionalità e miglioramenti. Il vostro coinvolgimento è fondamentale per rendere "Ermes" sempre più utile e efficace per gli utenti.

## Come Contribuire

### Segnalazione di Bug e Problemi

Se riscontri un bug o un problema durante l'utilizzo di "Ermes", ti incoraggiamo a segnalarlo tramite la sezione delle _Issues_ di GitHub. Assicurati di includere tutte le informazioni pertinenti, come una descrizione dettagliata del problema, il comportamento atteso e qualsiasi passo necessario per riprodurre il bug.

### Proposta di Nuove Funzionalità

Se hai idee per nuove funzionalità o miglioramenti che potrebbero arricchire "Ermes" e rendere l'esperienza degli utenti ancora migliore, non esitare a condividerle con noi. Puoi farlo aprendo una nuova _Issue_ e fornendo una descrizione chiara e dettagliata della tua proposta.

### Contributi al Codice

Se sei interessato a contribuire direttamente al codice sorgente di "Ermes", ti invitiamo a fare una _pull request_ con le tue modifiche. Prima di farlo, però, ti consigliamo di leggere attentamente le linee guida per i contributi e di assicurarti che le tue modifiche siano conformi agli standard del progetto.

## Linee Guida per i Contributi

Per garantire un'esperienza di collaborazione positiva e costruttiva, ti preghiamo di seguire le seguenti linee guida quando contribuisci a "Ermes":

- Rispetta il codice di condotta del progetto e trattati con rispetto verso gli altri collaboratori.
- Assicurati che ogni contributo sia conforme agli standard di qualità e di stile del codice del progetto.
- Fornisci una documentazione chiara e completa per qualsiasi modifica o aggiunta al codice.
- Assicurati di testare accuratamente le tue modifiche prima di inviare una _pull request_, per evitare regressioni e problemi nel codice.

## Contatti

Se hai domande o dubbi su come contribuire a "Ermes", non esitare a contattare il team di sviluppo tramite la sezione delle _Issues_ di GitHub o attraverso altri canali di comunicazione indicati nella sezione _Contatti_ del repository.

Grazie per il tuo interesse e il tuo contributo a "Ermes". Siamo entusiasti di lavorare insieme per rendere questo progetto sempre migliore e più utile per tutti gli utenti.

# Licenza

Il progetto "Ermes" è distribuito sotto la Licenza Pubblica Generica GNU, versione 3 (GNU GPL v3.0). Questa licenza garantisce agli utenti la libertà di utilizzare, studiare, modificare e condividere il software, promuovendo la cooperazione e la condivisione delle conoscenze.

## Testo della Licenza GNU GPL v3.0

```
Copyright (C) 2024 RagProject

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see <https://www.gnu.org/licenses/>.

```

## Dettagli della Licenza GNU GPL v3.0

### Permessi

- **Uso commerciale:** La licenza GNU GPL v3.0 permette l'uso del software per scopi commerciali.
- **Distribuzione:** È possibile distribuire copie del software, sia in forma originale che modificata.
- **Modifiche:** Gli utenti possono modificare il software, a condizione che le modifiche siano rilasciate sotto la stessa licenza GNU GPL v3.0.
- **Uso privato:** La licenza consente l'uso privato del software.

### Limitazioni

- **Licenza Virale:** Qualsiasi opera derivata basata su questo software deve essere rilasciata sotto la stessa licenza GNU GPL v3.0. Questo assicura che tutte le modifiche rimangano libere e aperte.
- **Responsabilità:** Il software viene fornito "così com'è", senza alcuna garanzia esplicita o implicita. Gli autori non sono responsabili per eventuali danni derivanti dall'uso del software.

### Obblighi

- **Avviso di copyright e licenza:** Ogni copia o porzione sostanziale del software deve includere il testo della licenza e l'avviso di copyright originale.
- **Condivisione delle modifiche:** Le modifiche al software devono essere rese disponibili sotto la stessa licenza GNU GPL v3.0.

## Come Applicare la Licenza GNU GPL v3.0 al Proprio Progetto

Per applicare la Licenza GNU GPL v3.0 al tuo progetto, includi una copia del testo della licenza nel repository del progetto. Inoltre, aggiungi il seguente testo all'inizio di ogni file sorgente, dove appropriato:

```
Copyright (C) 2024 RagProject

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see <https://www.gnu.org/licenses/>.

```

## Riconoscimenti

Utilizzando il progetto "Ermes" sotto la Licenza GNU GPL v3.0, ci impegniamo a mantenere la trasparenza e l'apertura del software, incoraggiando la cooperazione e la condivisione delle conoscenze all'interno della community. Apprezziamo ogni contributo e suggerimento volto a migliorare il progetto e a renderlo più accessibile e utile per tutti gli utenti.
