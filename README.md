## Projekat rade:
---
- Student 1: RA 44/2018 Žarko Blagojević
- Student 2: RA 5/2018 Nikola Milosavljević
- Student 3: RA 3/2018 Srđan Šuković 
## Neophodne stvari za pokretanje projekta
---
Kako bi se pokrenuo projekat neophodno je instalirati sledeće programe
- [Microsoft SQL server](https://go.microsoft.com/fwlink/?linkid=866662)
- [SQL server management studio](https://aka.ms/ssmsfullsetup)
- [Visual studio](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=community&rel=17)
- [Node i npm](https://nodejs.org/dist/v16.13.2/node-v16.13.2-x64.msi)

Pored ovih stvari neophodno je postaviti i određene **environment promenljive**. 
| Naziv promenljive | Vrednost | Značenje |
| :---: |:---:|:---:|
| ISA_FRONT_DIR | .\\..\\..\frontend | putanja do direktorijuma gde se nalazi front
| ApiKey | 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 | ključ koji se koristi za SendGrid

---
Nakon instaliranja svih programa i podešavanja promenljivih okruženja, neophodno je otići u direktorijum *frontend* i kroz neku konzolu (git bash, powershell) pokrenuti 
```
npm install
```
Nakon instaliranja svih zavisnosti za vue koje su neophodne, pokrenuti *ISA.sln* iz direktorijuma *Solution*. Neophodno je instalirati dodatni extension za visual studio radi pokretanja *acceptance testova*. Za to se koristi specflow. [Link za instrukcije kako instalirati specflow](https://docs.specflow.org/projects/getting-started/en/latest/GettingStarted/Step1.html). Kako bi se ovi testovi pokrenuli, neophodno je proći 8 koraka. Kako je ovo dodatna stvar koja nije bila neophodna iz specifikacije, ovaj deo se može demonstrirati na odbrani od strane studenata.

## Pokretanje aplikacije
---
Pre pokretanja aplikacije, neophodno je uraditi deploy baze. Potrebno je dvokliknuti na skriptu *LOCAL.Model.Publish.xml*

<p align="center">
    <img src="ReadMeImgs/PublishScript.png" alt="Start up project"
        style="width: 350px;"/>
</p>

U prozoru koji se otvori potrebno je upisati pored promenljive Environment vrednost "*local*" i pritisnuti *Publish*

<p align="center">
    <img src="ReadMeImgs/PublishPub.png" alt="Start up project"
        style="width: 550px;"/>
</p>

Ako se sve izvršilo kako treba, trebao bi se dobiti sledeći ispis.

<p align="center">
    <img src="ReadMeImgs/PublishResult.png" alt="Start up project"
        style="width: 350px;"/>
</p>

Dok je pokrenut visual studio, u *Solution explorer-u* koji se nalazi sa desne strane, u direktorijumu *Web* nalazi se *API* projekat koji treba da se postavi kao StartUp projekat. Pritiskom na desni taster miša i izborom opcije *Set as startup project* postiže se željeni rezultat.

<p align="center">
    <img src="ReadMeImgs/StartUpProj.png" alt="Start up project"
        style="width: 350px;"/>
</p>

Nakon toga pritiskom na F5 pokreće se aplikacija, i trebalo bi da se automatski otvori izabrani browser sa sledećim prikazom 

<p align="center">
    <img src="ReadMeImgs/BasicStartApp.png" alt="Start up project"
        style="width: 750px;"/>
</p>

Gađanjem putanje koja se dobije kada se obriše označeni deo linka, dobiće se prikaz osnovne stranice aplikacije. Kako se front podiže svaki put kao zasebna aplikacija, postoji mogućnost da će biti neophodno malo sačekati da se učita frontend.

<p align="center">
    <img src="ReadMeImgs/FrontUrl.png" alt="Start up project"
        style="width: 550px;"/>
</p>

Ako su svi koraci ispraćeni kako treba, osnovna stranica za koja bi trebalo da se pojavi je sledeća.

<p align="center">
    <img src="ReadMeImgs/Front.png" alt="Start up project"
        style="width: 750px;"/>
</p>

## PDF-ovi
---
Dokumenti o rešenju konfliktnih situacija se nalaze u direktorijumu *KonfliktneSituacije*.

Dokumenti o skalabilnosti se nalaze u direktorijumu *Skalabilnost*

## DevOps flow
---
Implementiran je microflow za deploy aplikacije i pokretanje aplikacije na serveru, kao i microflow za gašenje aplikacije na serveru. Za hosting je korišćena AWS ec2 instanca koju smo dobili od Kešelj Strahinje, koji je organizator [AWS User Group-e](https://www.meetup.com/awsnovisad/) u Novom Sadu.

Pokretanje ovih flow-ova se postiže tako što se na [linku](https://github.com/zarkoblagojevic00/isa_fishing_airbnb) repozitorijuma, ode na tab [actions](https://github.com/zarkoblagojevic00/isa_fishing_airbnb/actions) i tu se nalaze dva flowa.
- [Automated deploy to server](https://github.com/zarkoblagojevic00/isa_fishing_airbnb/actions/workflows/deploy-to-server.yml)
- [Automated stop of api on server](https://github.com/zarkoblagojevic00/isa_fishing_airbnb/actions/workflows/stop-api-on-server.yml)

Pokretanje workflow-a se radi kroz browser pritiskom na dugme run workflow. Iako se workflow izvrši relativno brzo, samoj aplikaciji je neophodno neko vreme da se podigne i da može da se pogodi kroz browser. Kroz nekih 2,3 minuta trebalo bi da aplikacija bude vidljiva na [linku](http://35.180.11.147:8080/), i da to bude isti frontend koji se video kao i pokretanjem aplikacije lokalno.

**Nakon završetka rada sa aplikacijom moli se korisnik da pokrene drugi workflow "automated stop of api on server" kako bismo sačuvali instancu servera.**

<p align="center">
    <img src="ReadMeImgs/RunWorkFlow.png" alt="Start up project"
        style="width: 750px;"/>
</p>