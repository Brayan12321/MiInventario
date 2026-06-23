FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos del proyecto
COPY *.sln .
COPY LOGIN/*.csproj ./LOGIN/
RUN dotnet restore ./LOGIN/LOGIN.csproj

# Copiar el resto del código
COPY LOGIN/ ./LOGIN/

# Publicar la aplicación
RUN dotnet publish ./LOGIN/LOGIN.csproj -c Release -o out

# Segunda etapa: Imagen de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados
COPY --from=build /app/out .

# Configurar el puerto
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

# Punto de entrada
ENTRYPOINT ["dotnet", "LOGIN.dll"]