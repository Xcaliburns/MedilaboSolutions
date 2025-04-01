# Étape de base avec HTTPS
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# Étape de construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Gateway/Gateway.csproj", "Gateway/"]
RUN dotnet restore "./Gateway/Gateway.csproj"
COPY . .
WORKDIR "/src/Gateway"
RUN dotnet build "./Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Étape de publication
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Étape finale avec HTTPS
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./Gateway/certs/cert.pfx /https/cert.pfx  
# Ajout du certificat HTTPS

ENV ASPNETCORE_URLS="https://+:5001;http://+:5000"
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="Davjess0806+-"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/https/cert.pfx"

ENTRYPOINT ["dotnet", "Gateway.dll"]
