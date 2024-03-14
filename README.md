# Introduction

### NB!!! Make sure you're checked out on the develop branch

This system is an employee management system for Retro Rabbit Enterprise Services. This is the back end for said system and works in conjuncture with the Front End repository

# Getting Started

### Cloning the [repository](https://retro-rabbit@dev.azure.com/retro-rabbit/RetroGradOnboard/_git/RGO-Server)

```powershell
git clone 'https://retro-rabbit@dev.azure.com/retro-rabbit/RetroGradOnboard/_git/RGO-Server'
```

Runs on(.NET Web API):

- https://localhost:7026
- http://localhost:5193

## Docker

### Install Docker

- https://www.docker.com/products/docker-desktop/
- If you don't use **WSL/Ubuntu** subsystem install Docker using the **Hyper-V** installation
- If new installation follow default settings for install

### Incorrect WSL version error

- If you get a WSL wrong version error run the following command

```powershell
wsl --install
```

# Environment Variables
Add system environments as follows 

![Image of System Environment Variables](./EnvironmentVariables.png)

With the respective values in the redacted spaces 
(please note that the dashes in between the variable names are double dashes)

# User Secrets
Right Click RGO.App and Click on **Mange User Secrets**

![Image of User Secret Location](./UserSecretsLocation.png)

This will open your secrets.json file

![Image of User secret example](./UserSecretsExample.png)

Paste the following in the file 

{
  "ConnectionStrings": {
    "Default": ""
  },
  "Auth": {
    "Key": "",
    "Issuer": "",
    "Audience": "",
    "Expires": 60
  }
}

Replace the Connection strings, Auth Key, Auth Issuer and Auth Audience

# pgAdmin

### Setup PgAdmin and Create Database

Install the latest version of PgAdmin. Then Register a new server on PgAdmin, name it RGO.
Password should be postgrespw. Set the host to localhost. You should be able to connect to the
RGO database after adding migrations and updating the DB in package manager console in
Visual Studios.


### Change to the Dev branch

Make sure to have Git installed to run any Git command lines.

```powershell
#cd RGO-Server\RGO Backend
git checkout develop
```

### Setting up docker container

```powershell
docker run --name RGO -e POSTGRES_PASSWORD=postgrespw -p 5432:5432 -d postgres

For RabbitMQ
docker pull rabbitmq:3-management
docker run --name r-mailing -it --hostname my-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management
```
### Creating Database Tables:

1. Open Visual Studio 2022 and open the RGO-Server project file. 
   Pull up the nuget package manager console:
    
   **_Tools_** -> **_NuGet Package Manager_** -> **_Package Manager Console_**
   Make sure the **Default project** is **_RGO.UnitOfWork_**.
    
   ----
    
   ![Image of Package Manager Console](./RGO-UnitOfWork-example.png)

2. Change the default project to RR.UnitOfWork.
3. Run the following commands:
   
```powershell
add-migration migrationName
update-database
```

Congratulations! You have now successfully created a database with tables.

### Populating Database with Dummy Data:

- Register new RGO server in PgAdmin


- Update Information and save

![Register service - connection](./Screenshot%202023-08-02%20173613.png)
![Register service](./Screenshot%202023-08-02%20173735.png)

1. Make a local copy of the DummyData.sql file In the RR.UnitOfWork Project.

   ![Screenshot 2024-03-12 130755](https://github.com/RetroRabbit/RGO-Server/assets/82169901/178d5ba8-160e-4b28-b280-2b6a08fb02da)

3. Copy one of the ``INSERT INTO 'Employee,`` statements in the script.
   
   ![Screenshot 2024-03-12 130900](https://github.com/RetroRabbit/RGO-Server/assets/82169901/73545f25-ab5f-4e60-b929-6cd6d0fa781a)
   
4. Paste a new INSERT statement and populate it with your information such as your email, 
   name, and surname. It is important to note that the first email field should be populated 
   with a personal or work email you're going to use to log into the RGO system, otherwise 
   you won't have access to the system. The second email field can just be a dummy or
   additional email you'll make use of.
   
5. Copy the SQL in the locally created script.
   
6. Open PgAdmin, right-click on the RGO database, and select ``Create Script``.

   ![Screenshot 2024-03-12 131219](https://github.com/RetroRabbit/RGO-Server/assets/82169901/5b0fa31a-9e90-4337-8896-fa3d355a5d77)

7. Paste the locally created script in the query screen that pops up.

   ![Screenshot 2024-03-12 131332](https://github.com/RetroRabbit/RGO-Server/assets/82169901/87eabaab-4856-4adc-ba54-feb1c6e47512)

8. Click on ``Execute Script``.
  
   ![Screenshot 2024-03-12 131356](https://github.com/RetroRabbit/RGO-Server/assets/82169901/69c52269-e074-487e-b489-53ac9c41a5ff)


Congratulations you have a fully populated database!

### Checking new user added to the DB you made

- Install **PgAdmin** beforehand. If you locally installed **_PostgreSQL_** be warned that it may interfear with your attempts to connect to the database(Docker).

Once the query is completed successfully, you can go to the employee table and view all rows to see if you have data in the database.
### Running Unit Tests

When running unit tests make sure that the database is running to accomodate for integration tests

### Unit Test Coverage

With every pull request, there is a requirement to prove coverage of your code. Attached a screen shot of your code coverage to your PR description

```
Install the dotnet coverage tool
    dotnet tool install -g dotnet-coverage

Install the dotnet report generator tool
    dotnet tool install -g dotnet-reportgenerator-globaltool

Run the command to check coverage on your project
    dotnet-coverage collect -f xml -o coverage.xml dotnet test <solution/project>
    (<solution/project> can be omitted to test the entire project)

Generate report
    reportgenerator -reports:coverage.xml -targetdir:coverage/report

Navigate to the %temp% / report folder and open index.html using your prefered browser found at
    /RGO-Server/coverage/report/index.html
```

# Naming Conventions
## Endpoints
Use forward slash
Use forward slashes for resource hierarchy and to separate URI resources.
Example: "/employee/{id}"


## Use nouns, not verbs
When naming the URIs, you should use nouns to describe what the resource is and not what it does. For example:
Wrong:   "getEmployees/"
Correct: "employees/"

## Use plural nouns
This makes it clear that there is more than one resource within a collection. Using singular nouns can be confusing. For example:
Wrong:  "chart/{id}"
Correct: "charts/{id}"

## Lowercase letters
As a standard, URLs are typed in lowercase. The same applies to API URIs.


## Use hyphens to separate words
When chaining words together, hyphens are the most user-friendly way and are a better choice than underscores.
For example: "employee-documents/10"


## Endpoint strings can be the same provided that the Request Mapping is different:
PUT "employee/{id}"
GET "employee/{id}"

## Variables
All variables in methods must be in camelCase

Anything referenced by a service should prefixed with an underscore, to indicate that it is a reference to a service 

All Method names must be PascalCase
 ie: SaveEmployeeDocument(SimpleEmployeeDocumentDto employeeDocDto)

PS: When naming and endpoint, variable or method make the name as descriptive as possible. The only exception is for small scopes like a lambda.
