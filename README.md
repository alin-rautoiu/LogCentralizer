LogTracker
==============
Ziceam ieri de un mic proiect, menit sa centralizeze log-urile obtinute de la un server FTP.
 
Date de intrare:
- Serverul de FTP "toarna" periodic niste fisiere in format .csv intr-o anumita locatie, unde avem acces. Am atasat un exemplu de log: 20110307_023937AM_vms4pplog download.csv
 
Cerinte:
- O aplicatie care sa verifice periodic folderul unde se afla fisierele .csv, si sa le importe intr-o baza de date SQL Server, si sa permita interogarea acestora si afisarea pe ecran folosind o interfata Web.
 
Resurse disponibile:
- Visual Studio 2010
- Microsoft SQL Server 2008 R2 (sau Express)
 
Eu vad aplicatia asta in felul urmator:
- O parte care va rula in background, periodic, si va verifica/procesa datele. Poate fi o aplicatie consola pusa intr-un task scheduler, sau un serviciu Windows. Fisierele procesate vor fi marcate pentru a fi deosebite de fisierele noi (probabil mutate in alta parte). Vom avea un mecanism de validare a datelor pentru a nu permite duplicari in baza.
- O parte scrisa in ASP.NET care va citi datele si le va afisa intr-un grid, cu posibilitate de filtrare dupa un interval de date (Start Date - End Date, combo-uri de tip datetime), si IP (camp text).
- Zona de lucru cu baza de date va fi scoasa intr-un layer separat. Operatiile de insert si validarile le facem in proceduri stocate.
- Toate connection string-urile, caile catre foldere, numele bazei de date, si in general tot ce tine de configurari le vom tine intr-un fisier de configurare, pentru a evita harcodarile.
- Incercam abstractizarea layerelor prin interfete.
- Implementam si un mecanism de logging pentru a urmari status-ul importurilor noastre. Aici putem folosi log4net (sau, daca vreti, puteti implementa propriul mecanism de logging).

Implementarea noastra pe scurt
=======
Am folosit trei proiecte
- unul pentru procesarea fisierului csv si preluarea datelor intr-o clasa tabel(formata din randuri si header) cu care lucreaza celelalte doua proiecte
- unul pentru afisarea datelor pe web prin asp
- un proiect pentru lucru cu baza de date
