# ddocs-backend
The backend service for [DDocs](https://ddocs.io)


## Environment Variables
|        Name       | Description                                          |
|:-----------------:|------------------------------------------------------|
| CONNECTION_STRING | The postgres connection string                       |
|     JWT_SECRET    | The secret key for generating JWT tokens             |
|     CLIENT_ID     | The discord application client id used for OAuth     |
|   CLIENT_SECRET   | The discord application client secret used for OAuth |
| REDIRECT_URI      | The discord application redirect uri used for OAuth  |

## Defining Routes

The backend service uses a custom wrapper for the default `HttpListener` to implement route modules.

### Creating new routes

```cs
public class YourClass : RestModuleBase
{
  [Route("/your/uri/path", "GET")
  public async Task<RestResult> ExecuteAsync()
  {
    // code
  }
}
```
#### Requirements
- Your module must inherit `RestModuleBase`.
- Your method must contain a `Route` attribute specifing the route path and the HTTP method.

#### Optional
- Your method can have a `RequireAuthentication` attribute, this locks execution to client with valid jwt authentication.
- Your method can be a regular expression by specifying `isRegex: true` in the `Route` attribute
- Your method can contain route parameters by defining them like so:
  ```cs
  [Route("/your/{parameter}", "GET")
  public async Task<RestResult> ExecuteAsync(string parameter)
  ```
  One note on this is that your parameter names must match up, the type can be any default type (like int, long, string, etc).
 
Each route must return a `RestResult` which holds the status code and optional json payload to return to the request.

### The Module Base
The `RestModuleBase` contains useful properties that use can use during your request:

|          Name         | Description                                                                                    |
|:---------------------:|------------------------------------------------------------------------------------------------|
|        Context        | The raw HTTP Context                                                                           |
|       RestServer      | The HTTP server instance                                                                       |
|        Request        | The raw request                                                                                |
|        Response       | The raw response object                                                                        |
| AuthenticationService | The authentication service                                                                     |
| DiscordOAuthHelper    | The OAuth helper                                                                               |
| Authentication        | The current authentication state for the request, can be null if no authentication is provided |
| ModuleInfo            | The raw module info                                                                            |


## Working with EF Core
This project uses EF Core to communicate between the application and postgres. To use this system propery you will need a few prerequisites:

In order to run any ef commands on the project you must have the env variable `CONNECTION_STRING` set to the postgres connection string.

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

