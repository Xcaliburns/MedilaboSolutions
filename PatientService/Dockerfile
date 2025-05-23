# �tape de base pour l'ex�cution de l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080


# �tape de construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Installer dotnet-ef pour g�rer les migrations
RUN dotnet tool install -g dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY ["PatientService/PatientService.csproj", "PatientService/"]
RUN dotnet restore "./PatientService/PatientService.csproj"
COPY . .
WORKDIR "/src/PatientService"
RUN dotnet build "./PatientService.csproj" -c Release -o /app/build

# �tape de publication
FROM build AS publish
RUN dotnet publish "./PatientService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# �tape finale
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app

# Installer `dotnet-ef` une seule fois dans `final`
RUN dotnet tool install -g dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS="http://+:8080"


# Ex�cuter la migration **� l'int�rieur du conteneur**
CMD ["dotnet", "ef", "database", "update", "--project", "/app/PatientService.csproj"]

ENTRYPOINT ["dotnet", "PatientService.dll"]

