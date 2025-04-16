# Étape de base avec HTTPS
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5020


# Étape de construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DiabeteRiskReportService/DiabeteRiskReportService.csproj", "DiabeteRiskReportService/"]
RUN dotnet restore "./DiabeteRiskReportService/DiabeteRiskReportService.csproj"
COPY . .
WORKDIR "/src/DiabeteRiskReportService"
RUN dotnet build "./DiabeteRiskReportService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Étape de publication
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DiabeteRiskReportService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Étape finale avec HTTP
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


ENV ASPNETCORE_URLS="http://+:5020"

ENTRYPOINT ["dotnet", "DiabeteRiskReportService.dll"]
