# Introduction
TrackingSystem is an online solution for tracking user's orders who need to be confirmed by several admins before start working on it (tracking admins acceptation).
example: 
If a user bought the item1 (he makes a request) 
This request must be accepted by 3 admins roles (admin1, admin2, admin3), admin1 accept the request and allow admin2 to accept it and allow admin3 to accept it or it will be refused (when admin accept, the system save the date for acceptation)

Language:
C# 
Telerik Component

Database:
SQL server
 
# Setup
  
1- Install SQL Server
  - Add this DB https://github.com/touficSl/TrackingSystem/blob/master/DBTrack-Diagram.png
  
2- Install Visual Studio 2015
  - Open VS and click File -> Open -> Web Site... -> select the project
  - Change the connection string in web.config
  - Click run
