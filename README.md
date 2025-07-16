## Setting up VSCode 
* Follow this guide: https://code.visualstudio.com/docs/languages/dotnet
    * Install extensions 
    * Install .Net SDK

## Building & Running
`cd API && dotnet build`
`cd API && dotnet run --project Testing.Api`

## Build & Run in the Whale
* Build & start your containers: `docker compose up --build`
    * You'll want to do this every time you make code changes. `--build` tells docker to rebuild them. 
* Remove all containers completely: `docker compose down`
    * Useful for ensuring resources which persist beyond a build get destroyed, such as databases.

## Testing Locally
`curl -X GET http://localhost:5188/api/test -H "Accept: application/json"`

## Testing on the Whale
`curl -X GET http://localhost:8080/api/test -H "Accept: application/json"`
