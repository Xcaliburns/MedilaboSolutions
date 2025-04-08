# Étape de base pour l'exécution de l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080


# Étape de construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Installer dotnet-ef pour gérer les migrations
RUN dotnet tool install -g dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY ["MedilaboSolutions/PatientService.csproj", "MedilaboSolutions/"]
RUN dotnet restore "./MedilaboSolutions/PatientService.csproj"
COPY . .
WORKDIR "/src/MedilaboSolutions"
RUN dotnet build "./PatientService.csproj" -c Release -o /app/build

# Étape de publication
FROM build AS publish
RUN dotnet publish "./PatientService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Étape finale
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app

# Installer `dotnet-ef` une seule fois dans `final`
RUN dotnet tool install -g dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY --from=publish /app/publish .
COPY ./MedilaboSolutions/certs/cert.pfx /https/cert.pfx

ENV ASPNETCORE_URLS="http://+:8080"


# Exécuter la migration **à l'intérieur du conteneur**
CMD ["dotnet", "ef", "database", "update", "--project", "/app/PatientService.csproj"]

ENTRYPOINT ["dotnet", "PatientService.dll"]

