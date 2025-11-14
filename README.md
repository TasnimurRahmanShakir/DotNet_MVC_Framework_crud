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

## ⚙️ Full Tailwind CSS Setup Details

This project is fully integrated with the Tailwind CSS v4 CLI, automated to run on build. Here’s a breakdown of the setup:

### 1. NPM Initialization
* A `package.json` file is located in the solution root, created using `npm init -y`.
* The Tailwind CLI was installed as a dev dependency:
    ```bash
    npm install -D @tailwindcss/cli
    ```

### 2. Configuration Files
* **`tailwind.config.js`**: Located in the solution root. Its `content` property is crucial and configured to scan all `.cshtml` files for classes:
    ```javascript
    module.exports = {
      content: [
        './TodoApplicationMVC/Views/**/*.cshtml', // Scans for classes here
      ],
      theme: {
        extend: {},
      },
      plugins: [],
    }
    ```
* **`Styles/index.css`** (or `input.css`): The source CSS file (in the `Styles` folder) that contains the main Tailwind directive:
    ```css
    @import "tailwindcss";
    ```

### 3. Build Scripts (in `package.json`)
Two scripts are configured in `package.json` to run the Tailwind CLI:
* **`"build:css"`**: `tailwindcss -i ./Styles/index.css -o ./TodoApplicationMVC/Content/tailwind.css --minify`
    * This is the main build script. It takes the source CSS, scans the `.cshtml` files, and generates a minified `tailwind.css` output file in the project's `Content` folder.
* **`"watch:css"`**: `tailwindcss -i ./Styles/index.css -o ./TodoApplicationMVC/Content/tailwind.css --watch`
    * This is a development script. It watches for changes in your `.cshtml` or `index.css` files and instantly rebuilds the CSS, enabling a live-reload workflow.

### 4. Automatic Build Integration (in `.csproj`)
To automate the build, the `TodoApplicationMVC.csproj` file was modified. A custom `<Target>` was added to run the `npm run build:css` script *before* the main Visual Studio build starts.

This is the code added to the `.csproj` file (right before the closing `</Project>` tag):
```xml
  <Target Name="Tailwind" BeforeTargets="Build">
    <Exec WorkingDirectory=".." Command="npm run build:css" />
  </Target>