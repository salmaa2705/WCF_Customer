# Project Readme: WCF Service with CRUD Operations

## Overview

This project is a WCF (Windows Communication Foundation) service that provides CRUD (Create, Read, Update, Delete) operations on a SQL Server database. The service demonstrates three different methods for interacting with the database: using Entity Framework with DbContext, implementing the Repository Design Pattern, and utilizing SQL native methods. Additionally, the project incorporates logging features with log4net and Serilog and is designed to be hosted in IIS (Internet Information Services).

## Features

- **WCF Service**: Provides a set of operations for CRUD operations on a SQL Server database.
- **Entity Framework with DbContext Method**: Utilizes the Entity Framework with DbContext for database interaction.
- **Repository Design Pattern Method**: Implements the Repository Design Pattern to separate data access logic.
- **SQL Native Method**: Directly uses SQL queries for database operations.
- **Logging with log4net and Serilog**: Incorporates log4net and Serilog for comprehensive logging capabilities.
- **SQL Server Database Connection**: Establishes a connection with a SQL Server database for data storage.
- **Hosted in IIS**: Configured to be hosted in IIS for easy deployment and scalability.

## Prerequisites

- Visual Studio (2019 or later)
- .NET Framework (4.5 or later)
- IIS (Internet Information Services)
- SQL Server

## Getting Started

1. **Clone the Repository**:

    ```bash
    git clone https://github.com/yourusername/your-wcf-service.git
    ```

2. **Open in Visual Studio**:

    Open the solution file in Visual Studio.

3. **Database Setup**:

    - Create a new database in SQL Server.
    - Update the connection string in the `app.config` file with the appropriate credentials.

4. **Build and Run**:

    Build the solution and run the WCF service project.

5. **Configure IIS**:

    - Create a new site in IIS.
    - Set the physical path to the location of the WCF service binaries.
    - Configure the site to use an application pool with the appropriate .NET version.

6. **Testing**:

    Use a tool like SOAPUI or Postman to test the CRUD operations exposed by the WCF service.

## Logging Configuration

Logging is configured using log4net and Serilog. The configuration files (`log4net.config` and `serilog.json`) are included in the project. Customize these files based on your logging requirements.

## Database Connection

The database connection is established in the `app.config` file. Update the connection string with your SQL Server database details.

## Conclusion

This project serves as a template for building WCF services with different methods for CRUD operations, incorporating logging features, and being ready for deployment in IIS. Feel free to modify and extend it based on your specific requirements.

**Note**: Ensure that the necessary dependencies and packages are restored before building and running the project.
