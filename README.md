# Employees management application

## General info
This is an Employees Management Application designed and created as part of a college project.

## Technologies
Project is created with:
* C#
* .NET 8.0
* WPF
* SQL server

## Main features
Main things this application is designed to help with are primarly:
* **Schedule** - allowing employee to see his own shifts planned in the coming days, and employer to change and plan work days for his staff
* **Messages** - sending and receiving correspondence between workers of the same company
* **Tasks system** - assigning and monitoring progress in employees' work at specific things
* **Employee roster** - list of all staff employed together with information about them (only available to the boss)
* **Timer** - counting down the time until the end of the shift

## Secondary modules
Some detailes about minor modules in this application, not directly related to main functionality:
* All data is stored on SQL database server to which the application connects when launched
* Every user logs in by his own login and password. Passwords are encrypted at the both sides (client & database)
* Implemented permission system that allows certain functionalities only to users with the approprate level of permissions
* Almost every change in database is available from the application itself and is immediately updated

Loggin in
![image](https://github.com/P1wko/Employee_management_system/assets/102859404/090406d1-8127-459d-9118-acd5d7189cf6)

Sending a message
![image](https://github.com/P1wko/Employee_management_system/assets/102859404/8ab97db3-bb02-4c33-acc2-3cf977c506dd)

Schedule
![image](https://github.com/P1wko/Employee_management_system/assets/102859404/b186a34a-9401-4506-8dd0-188483e99b06)

Employee roster
![image](https://github.com/P1wko/Employee_management_system/assets/102859404/290f1d26-d50e-4eb8-aef5-f8f6bd899bff)

Tasks system
![image](https://github.com/P1wko/Employee_management_system/assets/102859404/1fe3be21-e577-4e9f-9eb4-4e063d124e18)
