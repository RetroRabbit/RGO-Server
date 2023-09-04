# Introduction 
This system is an employee management system for Retro Rabbit

# Getting Started
### Cloning the [repository](https://retro-rabbit@dev.azure.com/retro-rabbit/RetroGradOnboard/_git/RGO-Server)
```powershell
git clone 'https://retro-rabbit@dev.azure.com/retro-rabbit/RetroGradOnboard/_git/RGO-Server'
```
Runs on(.NET Web API):
- https://localhost:7026
- http://localhost:5193
### Install Docker
- If you don't use **WSL/Ubuntu** subsystem install Docker using the **Hyper-V** installation
### Setting up docker container
```powershell
docker run --name RGO -e POSTGRES_PASSWORD=postgrespw -p 5432:5432 -d postgres
```
### Run migration
Pull up the nuget package manager console:
**_Tools_** -> **_NuGet Package Manager_** -> **_Package Manager Console_**
Make sure the **Default project** is **_RGO.Repository_**.
___
![Image of Package Manager Console](./Screenshot%202023-08-02%20173156.png)

```powershell
Update-Database
```
### Adding user to the DB you made
Install **PgAdmin** beforehand. If you locally installed **_PostgreSQL_** be warned that it may interfear with your attempts to connect to the database(Docker).
___
![Register service](./Screenshot%202023-08-02%20173735.png)
___
![Register service - connection](./Screenshot%202023-08-02%20173613.png)
___
![PgAdmin query tool](./Screenshot%202023-08-02%20173343.png)
```sql
insert into social(discord, codewars, github, linkedin)
values ('discord', 'codewars','github', 'linkedin');
insert into users(firstName, lastName, email, [type], joinDate, [status])
values ('Firstname', 'Lastname', 'email@retrorabbit.co.za', 0, now(), 1, 1);
```

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
    
Navigate to the %temp% / report folder and open index.html using your prefered browser
```


### FAQ
```typescript
// user type
enum UserType {
    GRAD = 0,
    PRESENTER,
    MENTOR,
    ADMIN
}

// user status
enum UserStatus {
    NEW = 1,
    ACTIVE,
    INACTIVE
}

// form status
enum FormStatus {
    NEW = 0,
    ACTIVE,
    INACTIVE
}

// field type
enum FieldType {
    SIGNATURE = 0,
    FILEUPLOAD,
    DROPDOWN,
    TEXTAREA,
    TEXTBOX
}

// stack type
enum StackType {
    DATABASE = 0,
    FRONTEND,
    BACKEND,
}

// event type
enum EventType {
    EVENTS = 0,
    WORKSHOPS
}

// form type
enum FormType {
    
}

// userStack typestatus
enum UserStackStatus {
    Saved = 1,
    Pending
}
```