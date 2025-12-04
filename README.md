**Product Management System**

This is a simple Product Management System built for CRUD operations using:

ASP.NET Core Web API (Backend)

SQL Server with Stored Procedures (Database)

AngularJS + Bootstrap (Frontend UI)

Features supported:

Add Item

Edit Item

Delete Item

List Items

Image upload in Base64 format

Form validations

Success and error messages

Responsive Bootstrap-based UI

**Why Web API and AngularJS Together**

Both the Web API and the AngularJS frontend are kept inside the same ASP.NET Core project so they run together with a single host.
This removes the need to run two separate servers and avoids CORS issues.
This setup is simple, lightweight, and ideal for quick demonstration and evaluation.

**Solution Architecture**
**1. SQL Server**

Database name: Product

Table name: Items

Stored procedures: AddItem, EditItem, DeleteItem, GetAllItems

**2. ASP.NET Core Web API**

Communicates with SQL Server using ADO.NET

Handles CRUD operations

Converts images to and from Base64

Returns data in JSON format

**3. AngularJS Frontend**

Built using index.cshtml and app.js

Calls API endpoints using HTTP

Loads items automatically on page load

Provides forms for Add and Edit operations

Displays uploaded photo preview

Uses Bootstrap for responsive design

**How to Run**

Create the database and stored procedures using the provided SQL script.

Update the connection string in appsettings.json.

Open the solution in Visual Studio 2022.

Press F5 to run the Web API.

Open the UI at:

https://localhost:xxxx/index.cshtml


Items will load automatically.
