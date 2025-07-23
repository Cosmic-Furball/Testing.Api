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

### POST Example
`curl -X POST http://localhost:8080/api/weatherforecast/add -H "Content-Type: application/json" -d '{"summary": "Blizzard"}'`

### PUT Example
`curl -X PUT "http://localhost:8080/api/WeatherForecast/replace" -H "Content-Type: application/json" -d '["Freezing", "Bracing",  "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching", "Blizzard", "Spicy"]'`


## WSL 
* WSL's Windows Host IP: `172.31.176.1`
    * Obtain with `ip route show | grep -i default`

* Must update `launchSettings.json` to bind to `0.0.0.0` so that the application binds to all network interfaces and is accessible from WSL. 
    * To run in WSL: `curl -X GET http://172.31.176.1:8080/api/test -H "Accept: application/json"`



## SQL 
Example SQL queries for creating a table, inserting data, selecting data, and deleting a table.

```
CREATE TABLE weather_forecasts (
    id SERIAL PRIMARY KEY,
    summary VARCHAR(100) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO weather_forecasts ("summary") VALUES ('glitter rain');

SELECT * FROM weather_forecasts;

DROP TABLE weather_forecasts;
```
