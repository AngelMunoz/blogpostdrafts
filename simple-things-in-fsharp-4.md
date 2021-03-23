[Npgsql.FSharp]: https://github.com/Zaid-Ajaj/Npgsql.FSharp
[RepoDB]: https://repodb.net/
[MongoDB.NET Driver]: https://mongodb.github.io/mongo-csharp-driver/
[Mondocks]: https://github.com/AngelMunoz/Mondocks
[Migrondi]: https://dev.to/tunaxor/migrondi-simple-sql-migrations-tool-30lm

# Simple things in F#

Hello everyone it's time for other simple things in F# today we'll talk about... Databases so let's get started!

# Databases
Everytime you create a new .NET application the chances of needing a database are high specially if you're working on a server kind of application. There are various kinds of databases out there but I'll focus in the following

- PostgreSQL
- SQLite
- MongoDB

The reason for this is that the solutions I'll show (except for MongoDB) are likely to be compatible with the other main databases out there (MySQL, SQL Server, etc.)

### Schema
in the spirit of keeping it simple we'll only work with a single table/collection with the following create table statement

```sql

```


## [RepoDB]
This is an awesome library that calls itself a `A hybrid ORM library for .NET.` the reason for that is basically because it sits between a full fledged ORM like EntityFramework and SQL libraries this can help you improve your productivity and also gain full controll when needed without having to fight your DB abstractions




```fsharp
#r "nuget: RepoDb.PostgreSql"

RepoDb.PostgreSqlBootstrap.Initialize()


```


### Shameless Plug