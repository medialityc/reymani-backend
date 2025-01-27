# Etapa de construcción (build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar y restaurar dependencias
COPY *.csproj .
RUN dotnet restore

# Copiar todo el código y compilar
COPY . .
RUN dotnet publish -c Release -o /app

# Etapa final (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_ENVIRONMENT=Development

# Puerto expuesto y comando de inicio
EXPOSE 80
ENTRYPOINT ["dotnet", "reymani-web-api.dll"]