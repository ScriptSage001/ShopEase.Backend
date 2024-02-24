#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/AuthService/ShopEase.Backend.AuthService.API/ShopEase.Backend.AuthService.API.csproj", "Services/AuthService/ShopEase.Backend.AuthService.API/"]
COPY ["Services/AuthService/ShopEase.Backend.AuthService.Application/ShopEase.Backend.AuthService.Application.csproj", "Services/AuthService/ShopEase.Backend.AuthService.Application/"]
COPY ["Services/AuthService/ShopEase.Backend.AuthService.Core/ShopEase.Backend.AuthService.Core.csproj", "Services/AuthService/ShopEase.Backend.AuthService.Core/"]
COPY ["Services/AuthService/ShopEase.Backend.AuthService.Infrastructure/ShopEase.Backend.AuthService.Infrastructure.csproj", "Services/AuthService/ShopEase.Backend.AuthService.Infrastructure/"]
RUN dotnet restore "./Services/AuthService/ShopEase.Backend.AuthService.API/ShopEase.Backend.AuthService.API.csproj"
COPY . .
WORKDIR "/src/Services/AuthService/ShopEase.Backend.AuthService.API"
RUN dotnet build "./ShopEase.Backend.AuthService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ShopEase.Backend.AuthService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopEase.Backend.AuthService.API.dll"]