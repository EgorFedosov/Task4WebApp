# Bulk User Management System

A web application built with **ASP.NET Core 10 MVC** for managing user accounts. This project focuses on efficient database interactions, cloud compatibility, and security through centralized session management.

## Features

* **MVC Architecture**: Implements Model-View-Controller pattern with Razor Views.
* **Bulk Operations**: Utilizes `ExecuteUpdateAsync` and `ExecuteDeleteAsync` for mass actions (Block, Unblock, Delete) in single database round-trips.
* **Centralized Security**: Implements session invalidation using `SecurityStamp` and `LockoutEnd` to ensure immediate termination of blocked or deleted user sessions.
* **Database Integrity**: Enforces unique email constraints at the database level using PostgreSQL Indexes.
* **Automated Auditing**: Tracks registration dates and last login times.
* **Auto-Migrations**: System automatically applies database schema changes on application startup.

## Tech Stack

* **Backend**: .NET 10, ASP.NET Core Identity, Entity Framework Core (EF Core).
* **Database**: PostgreSQL 17.
* **Frontend**: Bootstrap 5, Bootstrap Icons, Vanilla JavaScript.
* **DevOps**: Docker, Docker Compose, Render.

## Local Launch

Run the following command from the root directory to start the application and the PostgreSQL database:

```bash
docker-compose up --build

```

The application is accessible at: `http://localhost:8080`

## Docker Hub Image

```bash
docker pull egorfedosov/task4webapp:latest

```