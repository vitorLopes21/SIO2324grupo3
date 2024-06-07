# SIO2324grupo3

Grupo de Sistemas de Informação Organizacionais do ano letivo 2023/2024.

## Table of Contents

1. [Accessing the Database](#accessing-the-database)
2. [Starting the First ETL Process](#starting-the-first-etl-process)
3. [Using the REST API](#using-the-rest-api)
4. [Introduction to Power BI Reports](#introduction-to-power-bi-reports)

## Accessing the Database

To access the database, follow these steps:

1. **Servername:** `servername01.database.windows.net`
2. **Database:** `databasename01`
3. **Credentials:**
   - **Username:** `CloudSA8de65aa8`
   - **Password:** `Adminadmin333`

4. **IP Registration:**
   - You will be prompted to register your IP address the first time you attempt to connect to the database. Please follow the instructions provided during this prompt.

### Important Disclaimer

We are currently low on funds (only $7 remaining), which might cause the database to become unavailable once funds are depleted. Since we are not sure when the professor is evaluating this project, we thought we should let you know beforehand the database might not be available because we ran out of funds.

## Starting the First ETL Process

Before you can execute the ETL process, ensure your IP address has been registered (refer to the [Accessing the Database](#accessing-the-database) section).

Once your IP is registered:

1. **Run the ETL Process:**
   - You can execute the ETL process by running the program with the following command:

     ```sh
     dotnet run <path_to_csproj_file>
     ```

   - Alternatively, you can run it directly inside Visual Studio.

## Using the REST API

Before you can use the REST API, ensure your IP address has been registered (refer to the [Accessing the Database](#accessing-the-database) section).

Once your IP is registered:

1. **Run the REST API Server:**
   - You can run the server directly in Visual Studio.
   - Ensure that you are running it with HTTPS, as the REST API only works with HTTPS.

## Introduction to Power BI Reports

The Power BI report is designed to be user-friendly and informative.

1. **Navigation:**
   - The first page contains buttons that link to all other pages in the report.
   - There are also buttons to filter the data for each company involved in the project.

2. **Filtering Data:**
   - To filter the data displayed for a specific company, simply control-click on the respective company button. This action will update the data displayed to reflect the selected company.
