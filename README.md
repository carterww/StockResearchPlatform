# StockResearchPlatform
The StockResearchPlatform is a Blazor project that provides users with access to various forms of stock research. The project uses ASP.NET, Blazor, MySQL, and Microsoft Azure. This document will provide general information about setting up the project, the project's structure, and other information.

---

## IMPORTANT
This project contains API keys in appsettings.json. If you plan on sharing the project outside of the users on this repo, please delete that file, so our API keys do not get leaked.

---

## Setup
This project can be setup very easily with very little adjustments. The only requirements are setting up your own MySQL server to run the project locally. This setup will also assume you are using Visual Studio 2022, but it is not necessary. If you are using something other than Visual Studio 2022, these directions may be different in regards to migrating the database's schema to your local one.
1. Delete the Migrations directory in the root of your project.
    - These files are the history of migrations to the production database and are only on the Github repo to keep a backup.
2. In appsettings.json, add a connection string to "ConnectionStrings" and name is whatever you'd like.
3. In Program.cs, change the variable "CURRENT_CON_STRING_NAME" to the name of the connection string you just added.
4. In Visual Studio 2022, open the Package Manager Console and type the following: "Add-Migration \[name\]" Add your own name like "initial" or "setup."
5. Then type "Update-Database" in the Package Manager Console.
    - Your local database should then have the schema.
6. Finally, change the launch profile in Visual Studio 2022 to "Load Data" in order to load all the stock and mutual fund data into the database.

---

## Common Setup Problems
Here is a list of common setup problems that the development team discovered along with solutions. This list will be added to once they are discovered.
- If the "Add-Migration \[name\]" command fails, please ensure the Migrations directory is deleted and the connection string is correct.

---

## General Project Structure
The project has five important components all in different directories.
- There are the repository classes in the Repositories directory. These classes are used to perform CRUD operations on tables in the database.
- There are models in Models directory that correspond to either database records, HTTP responses, or general data transfer objects.
- There are services inthe Serives directory that, generally, are classes responsible for performing some server like getting stock financial information, updating dividend info in the database, etc.
- The Commands directory holds classes that represent HTTP request URLs. They provide an easy way to configure query parameters and HTTP headers.
- The Pages direcoctory holds Blazor pages. These are the views and controllers responsible for interfacing with the user and communicating with services.

The Program.cs file contains all the information necesary to start the server. It initializes all the services and repositories so they can be injected into other objects. Other things like authentication, authorization, the db connection, etc. are configured here. If there are problems with launching the web server, it is likely a problem in Program.cs.

---

## Miscellaneous
A live version of the website can be found [here](https://stockresearchplatform20230328164526.azurewebsites.net/). The MySQL database and Blazor server code are hosted on Microsoft Azure.

---

## Development Team
This project was developed by Ali Al-Raisi, David Qiu, and Carter Williams as a project for CS 496 at Western Kentucky University. 