# Introduction

TrackingSystem is a "Request Management System", is a portal website for the Radiation Safety Services that manages requests of clients by different users depending on their roles using an approval process. 

**It includes these details:** 

-	Front-end to allow users to use the website, register and order different types of requests.
-	Back-end for each admin, to allow them to confirm the new registered users so they can order the requests and they accepts requests for transfers them to the next admin.

# Work Flow Example
- let's say we have a customer, a director, and a technician. 
- The customer needs an Item or service, he registers with his account using the front-end, at the first he can't login to his account, the system saves the date and time for the registration. 
- The director must accept him from the director back-end, to let him access his account, When the director accepts the customer, he receives an email that he is allowed to use his account and the system save the date and time for the acceptation. 
- The customer login using his account, and create a request, the system saves the date and time for the request.  
- The director must accept him, to let his request appears for the technician, the system saves the date and time for the acceptation.
- When director accepts, the request is transferred to the technician on his back-end, and when he finishes setting up the request he must close the request and the system save the date and time. 

**Note:** After customer acceptation, All users can track the request to know where it has become and who saw it and where did it get.
 
# The Project Goals

-	Tracking the request, to know where it has become, who saw it and where did it get.
-	Facilitation of the user work.
-	Online registration.
-	Assign the corresponding step of approval for each user.
 
 
# Implementation

## A. Technical environment used

- Visual Studio 2015
- SQL Server 2012
- ASP.Net / Database : SQL Server 
-	Language : C#
- Telerik Component

## B. Setup

1- Install SQL Server
  - Add this DB https://github.com/touficSl/TrackingSystem/blob/master/DBTrack-Diagram.png
  
2- Install Visual Studio (VS) 2015
  - Open VS and click File -> Click Open -> Click Web Site... -> Select the project
  - Change the connection string in web.config
  - Click run
  - You can check my youtube channel for more details on how to open and run the project: https://www.youtube.com/watch?v=hVB9ufdNPTk

