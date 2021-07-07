#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/GenGenerate.Core/GenGenerate.Core.csproj", "src/GenGenerate.Core/"]
COPY ["src/GenGenerate.Core.Infrastructure/GenGenerate.Core.Infrastructure.csproj", "src/GenGenerate.Core.Infrastructure/"]

RUN dotnet restore "src/GenGenerate.Core/GenGenerate.Core.csproj"
COPY . .
WORKDIR "/src/src/GenGenerate.Core"
RUN dotnet build "GenGenerate.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GenGenerate.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:8080
ENTRYPOINT ["dotnet", "GenGenerate.Core.dll"]
