# Usa la imagen oficial de Microsoft para .NET 6.0 Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

# Usa la imagen oficial de Microsoft para el SDK de .NET 6.0
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copia el archivo csproj y restaura las dependencias
COPY ["HOTELAPI1/HOTELAPI1.csproj", "HOTELAPI1/"]
RUN dotnet restore "HOTELAPI1/HOTELAPI1.csproj"

# Copia el resto de los archivos del proyecto
COPY . .
WORKDIR "/src/HOTELAPI1"

# Compila la aplicación
RUN dotnet build "HOTELAPI1.csproj" -c Release -o /app/build

# Publica la aplicación
FROM build AS publish
RUN dotnet publish "HOTELAPI1.csproj" -c Release -o /app/publish

# Copia los archivos publicados a la imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Variables de entorno (Establece estas variables en tu entorno de producción)
ENV DEFAULT_CONNECTION_STRING=your_default_connection_string_here
ENV MONGO_CONNECTION_STRING=your_mongo_connection_string_here

ENTRYPOINT ["dotnet", "HOTELAPI1.dll"]
