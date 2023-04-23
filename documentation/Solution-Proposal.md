# Solution proposal for To do list

## Intro. 
This document descrips the solution proposal for the demo 'To do list'. this demo is meant as a way to show of my skills as a software developer as well as a way to gain new knolagde.

## Requirements. 
the requirements for this application has been set by a external company wishes. that i then have build a set of funtional and non functionla requirements for. 

### Functional requirements. 
* The user must be able to create a user for the app.
* the user must be able to login to the app.
* when logged in the user must be able to create a todo list.
* when logged in the user must be able to add items to a todo list.
* when logged in the user must be able to log out.
* when logged in the user must be able to marke a item as done.

### Non-functional requirements.
* the system must store passwords in a non readable form for security.
* The apps language must be english

# Solution.
The solution will be build using ASP.net for the front-end and Microsoft Azure functions for the backend with the common language C# .Net core 3.1. 
Persistan layer will be handled through a MSSQL server. 
The application will hosted through Azure.

# Git. 
Github will serve as the primary reposetory for the sourcecode, Git hub actions should be implimented for CI/CD 

## Branching strategy. 
To optimice the development the folling branching strategy should be implimented.

* Main branch, locked for direct commites. changes will have to be merged via pull requests. Git action for building and deploying changes to Production eviroment in Azure.  

* Test branch, Locked for direct commites changes will have to be merged via pull requests. Git action for building and deploying changes to Production eviroment in Azure.  if this makes sence with azure payment models.

* Development branch, can have direct commites from contributers. 

# Front-end ASP.Net. 

# Back-end Functions.

# Database MSSQL.



