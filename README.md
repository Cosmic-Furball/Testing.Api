## Setting up VSCode 
* Follow this guide: https://code.visualstudio.com/docs/languages/dotnet
    * Install extensions 
    * Install .Net SDK

## Building & Running
`cd API && dotnet build`
`cd API && dotnet run --project Testing.Api`

## Testing
`curl -X GET http://localhost:5188/api/test -H "Accept: application/json"`
