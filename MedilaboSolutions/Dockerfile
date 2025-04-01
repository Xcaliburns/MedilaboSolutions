# Étape de base pour l'exécution de l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 443

# Étape de construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MedilaboSolutions/PatientService.csproj", "MedilaboSolutions/"]
RUN dotnet restore "./MedilaboSolutions/PatientService.csproj"
COPY . .
WORKDIR "/src/MedilaboSolutions"
RUN dotnet build "./PatientService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Étape de publication
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PatientService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Étape finale avec HTTPS
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./MedilaboSolutions/certs/cert.pfx /https/cert.pfx 
 # Copie du certificat HTTPS dans le conteneur

ENV ASPNETCORE_URLS="https://+:8081;http://+:8080"
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="Davjess0806+-"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/https/cert.pfx"

ENTRYPOINT ["dotnet", "PatientService.dll"]
