# ddocs-backend
The backend service for [DDocs](https://ddocs.io)


## Working with EF Core
This project uses EF Core to communicate between the application and postgres. To use this system propery you will need a few prerequisites:

#### Add EntityFramework dotnet tool
```
$> dotnet tool install --global dotnet-ef
```

#### Update the tool
```
$> dotnet tool update --global dotnet-ef
```

#### Verify the installation
```
$> dotnet ef
```
The output should look somthing like this
```
                     _/\__
               ---==/    \\
         ___  ___   |.    \|\
        | __|| __|  |  )   \\\
        | _| | _|   \_/ |  //|\\
        |___||_|       /   \\\/\\

Entity Framework Core .NET Command-line Tools 2.1.3-rtm-32065

<Usage documentation follows, not shown.>
```

### Creating a new migration
A migration is basically a commit of the model structure -- you're required to do this to make changes to the database scheme, you can also roll back to previous migrations if needed.

```
$> dotnet ef migrations add <migration_name>
```

#### Applying your migration
Once your migration is built and you're ready to update the database you can run the following command to apply the new scheme:
```
$> dotnet ef database update
```

### References
https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli

https://docs.microsoft.com/en-us/ef/core/cli/powershell

https://docs.microsoft.com/en-us/ef/core/cli/dotnet

