# �tape de base avec HTTPS
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000


# �tape de construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Gateway/Gateway.csproj", "Gateway/"]
RUN dotnet restore "./Gateway/Gateway.csproj"
COPY . .
WORKDIR "/src/Gateway"
RUN dotnet build "./Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# �tape de publication
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# �tape finale 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS="http://+:5000"

USER root
RUN apt-get update && apt-get install -y curl
USER app


ENTRYPOINT ["dotnet", "Gateway.dll"]
