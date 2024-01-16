![image](https://github.com/Educom-Trainees/educom-verrukkeluk/assets/41477180/ec8c8195-ace1-493b-be89-3038e2cf3ab2)

Verrukkulluk is een recepten website die door een grote supermarktketen als extra service- en verkoopkanaal wordt ingezet. Alle artikelen die in de recepten gebruikt worden, zijn in de filialen van de supermarkt te verkrijgen.

## Doel van de site
De bedoeling is dat bezoekers van de site zelf recepten kunnen toevoegen en van de gekozen recepten een boodschappenlijstje kunnen samenstellen. De site berekent automatisch het aantal calorieën per recept op basis van de artikel informatie en de benodigde hoeveelheden. De bezoekers van de site kunnen recepten (anoniem) waarderen (1 t/m 5 sterren), en er kan door de bezoekers op recepten gereageerd worden (daarvoor dient men ingelogd te zijn).

## Orignele opdracht
https://e-learning.educom.nu/cases/verrukkulluk/intro


## Install
* Create a new User called `verrukkulluk_user` in MySQL that has the following permissions: `Data.*`, `Structure.*`, `Administration.SHOW_DATABASES`, `Administration.LOCK_TABLES`.
  ![image](https://github.com/Educom-Trainees/educom-verrukkulluk/assets/41477180/52070f17-c21e-4060-855e-898fb35e1297)

* Create an environment variable with the name `VERRUKKULLUK_CONNECTION_STRING`  
  set it to the value `Server=localhost;Database=verrukkulluk;Uid=verrukkulluk_user;Pwd={the_given_password}`
  > Here is how to create it on [Windows](https://phoenixnap.com/kb/windows-set-environment-variable)  
  > Here is how to create it on [a Mac](https://phoenixnap.com/kb/set-environment-variable-mac)  
  > Here is how to create it on [Linux](https://phoenixnap.com/kb/linux-set-environment-variable)  
* Clone the repository
* Run the following commands (when not using visual studio):
  ```bash
  dotnet build
  ```
* Update the database from the terminal/Developer Powershell:
  ```bash
  dotnet ef database update
  ```

## Run de applicatie
* Run de applicatie vanuit Visual Studio of met het commandline commando `dotnet run`
