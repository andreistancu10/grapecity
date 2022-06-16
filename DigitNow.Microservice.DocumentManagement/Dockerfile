#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /dist
EXPOSE 80

FROM base AS final
WORKDIR /dist
COPY . .
ENTRYPOINT ["dotnet", "DigitNow.Microservice.DocumentManagement.dll", "host=0.0.0.0", "port=80"]