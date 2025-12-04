Product Management System – README
🔹 Overview

This is a simple Product Management System built for CRUD operations using:

ASP.NET Core Web API (Backend)

SQL Server + Stored Procedures (Database)

AngularJS + Bootstrap (Frontend UI)

The project supports:

Add Item

Edit Item

Delete Item

List Items

Image upload (Base64)

Validations + Success/Error messages

🔹 Why Web API + AngularJS Together?

I placed the API and frontend in the same ASP.NET Core project so they run together with one click (F5).
This avoids:

Running API & UI separately

CORS issues

Complex setup

This makes the demo simple, clean, and easy for evaluation.

🔹 Solution Architecture

SQL Server

Database: Product

Table: Items

4 stored procedures for CRUD

ASP.NET Core Web API

Reads/writes data using ADO.NET

Converts images to/from Base64

AngularJS Frontend

index.cshtml + app.js

Calls API using HTTP

Auto-load items on page load

Responsive UI with Bootstrap

🔹 How to Run

Create database and stored procedures (SQL script included)

Update connection string in appsettings.json

Open solution in Visual Studio

Press F5

Go to:

https://localhost:xxxx/index.cshtml


Items will load automatically.