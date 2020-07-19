#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base
COPY NetcoreLoggerDemo/fordocker /App
WORKDIR /App
ENV ASPNETCORE_URLS="http://+:5001; https://+:5002"
ENTRYPOINT ["dotnet", "netcoreloggerdemo.dll"]