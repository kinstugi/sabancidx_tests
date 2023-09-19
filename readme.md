## Testing for sabancidx case
- ensure that the you have cloned the api
- add a reference to the rest api project to this project
```sh
dotnet add reference <path-to-webapi-project/webapi.csproj>
```
- ensure that you have install Moq
- build project using 
```sh
dotnet build
```
- run the test using 
```sh
dotnet test
```