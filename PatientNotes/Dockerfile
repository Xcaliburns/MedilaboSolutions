# Étape de base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8090


# Étape de construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PatientNotes/PatientNotes.csproj", "PatientNotes/"]
RUN dotnet restore "./PatientNotes/PatientNotes.csproj"
COPY . .
WORKDIR "/src/PatientNotes"
RUN dotnet build "./PatientNotes.csproj" -c Release -o /app/build

# Étape de publication
FROM build AS publish
RUN dotnet publish "./PatientNotes.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Étape finale avec HTTPS
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./PatientNotes/certs/cert.pfx /https/cert.pfx  
# Certificat HTTPS partagé

ENV ASPNETCORE_URLS="http://+:8090"


ENTRYPOINT ["dotnet", "PatientNotes.dll"]
