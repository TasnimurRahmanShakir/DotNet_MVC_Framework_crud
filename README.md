# TodoApplicationMVC

A simple To-Do list application built with ASP.NET MVC (.NET Framework) and styled with Tailwind CSS v4.

This project was built to demonstrate a classic C# CRUD (Create, Read, Update, Delete) application using raw ADO.NET for database access and a modern `npm`-based build process for the frontend.

## 🚀 Tech Stack

* **Backend:** ASP.NET MVC 5 (.NET Framework)
* **Database:** SQL Server (using ADO.NET)
* **Styling:** Tailwind CSS v4
* **Build:** `npm` scripts to run the Tailwind CLI

## 📋 Prerequisites

* Visual Studio 2022
* .NET Framework 4.7.2 (or newer)
* Node.js and npm (LTS version recommended)
* SQL Server (LocalDB, Express, or any other version)

## 🛠️ How to Run This Project

1.  **Clone the Repository**
    ```bash
    git clone [https://your-repository-url.git](https://your-repository-url.git)
    cd TodoApplicationMVC
    ```

2.  **Install NPM Dependencies**
    Open a terminal in the solution root (where `package.json` is) and run:
    ```bash
    npm install
    ```
    This will install the `@tailwindcss/cli`.

3.  **Set Up the Database**
    * Open SQL Server Management Studio (or your preferred DB tool).
    * Create a new database (e.g., `TodoDb`).
    * Run the following script to create the `todo` table:
    ```sql
    CREATE TABLE [todo] (
        [Id]          UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        [Title]       NVARCHAR(255)    NOT NULL,
        [Description] NVARCHAR(MAX)    NULL,
        CONSTRAINT [PK_todo] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    ```

4.  **Update Connection String**
    * Open the `Web.config` file (located inside the `TodoApplicationMVC` project folder).
    * Find the `<connectionStrings>` section.
    * Update the `TodoApplication` connection string to point to your new database.

    *For example, if using LocalDB:*
    ```xml
    <connectionStrings>
      <add name="TodoApplication" 
           providerName="System.Data.SqlClient" 
           connectionString="Data Source=(LocalDB)\MSSQLLocalDB;Database=TodoDb;Integrated Security=True;"/>
    </connectionStrings>
    ```

5.  **Build and Run**
    * Open the `TodoApplicationMVC.sln` file in Visual Studio 2022.
    * **Build the solution (F6 or Build > Build Solution).** This will trigger the custom build step in the `.csproj` file, which runs `npm run build:css` and generates your `tailwind.css` file.
    * **Run the project (F5).**

## ⚙️ How the Tailwind Build Works

This project uses `npm` to manage Tailwind:
* `npm run build:css`: Runs a one-time build of the CSS, minifies it, and saves it to `/TodoApplicationMVC/Content/tailwind.css`. This is tied to the Visual Studio build process.
* `npm run watch:css`: Runs the Tailwind CLI in "watch" mode. This is great for development, as it will automatically rebuild your CSS every time you save a `.cshtml` file. You can run this in a separate terminal or use the **Task Runner Explorer** in Visual Studio.