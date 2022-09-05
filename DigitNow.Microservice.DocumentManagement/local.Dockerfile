FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY DigitNow.Microservice.DocumentManagement/nuget.config ./
COPY DigitNow.Microservice.DocumentManagement.sln ./ 
COPY DigitNow.Domain.DocumentManagement/*.csproj ./DigitNow.Domain.DocumentManagement/
COPY DigitNow.Adapters.MS.Catalog/*.csproj ./DigitNow.Adapters.MS.Catalog/
COPY DigitNow.Domain.DocumentManagement.Client/*.csproj ./DigitNow.Domain.DocumentManagement.Client/
COPY DigitNow.Domain.DocumentManagement.Client.IntegrationTests/*.csproj ./DigitNow.Domain.DocumentManagement.Client.IntegrationTests/
COPY DigitNow.Domain.DocumentManagement.Contracts/*.csproj ./DigitNow.Domain.DocumentManagement.Contracts/
COPY DigitNow.Domain.DocumentManagement.Tests/*.csproj ./DigitNow.Domain.DocumentManagement.Tests/
COPY DigitNow.Microservice.DocumentManagement/*.csproj ./DigitNow.Microservice.DocumentManagement/

 
RUN dotnet restore

COPY . .
WORKDIR /src/DigitNow.Domain.DocumentManagement
RUN dotnet build -c Release -o /app

WORKDIR /src/DigitNow.Microservice.DocumentManagement
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app . 
ENTRYPOINT ["dotnet", "DigitNow.Microservice.DocumentManagement.dll"]