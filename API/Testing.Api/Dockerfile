FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /build
#COPY API/*.sln .
#COPY API/Testing.Api/Testing.Api.csproj API/Testing.Api/Testing.Api.csproj
COPY . .

RUN dotnet restore API/Testing.Api/Testing.Api.csproj
RUN dotnet publish API/Testing.Api/Testing.Api.csproj -c Debug -o /app/publish --no-restore

FROM base AS web
WORKDIR /app

COPY --from=build-env /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Testing.Api.dll"]