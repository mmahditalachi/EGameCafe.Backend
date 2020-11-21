#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["EGameCafe.Server/EGameCafe.Server.csproj", "EGameCafe.Server/"]
COPY ["EGameCafe.Infrastructure/EGameCafe.Infrastructure.csproj", "EGameCafe.Infrastructure/"]
COPY ["EGameCafe.Application/EGameCafe.Application.csproj", "EGameCafe.Application/"]
COPY ["EGameCafe.Domain/EGameCafe.Domain.csproj", "EGameCafe.Domain/"]
RUN dotnet restore "EGameCafe.Server/EGameCafe.Server.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "EGameCafe.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EGameCafe.sln" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EGameCafe.Server.dll"]
