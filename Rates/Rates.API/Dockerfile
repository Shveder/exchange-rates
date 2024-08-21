# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET SDK 8.0 as a parent image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies
COPY ["Rates.sln", "./"]
COPY ["Rates.API/Rates.API.csproj", "Rates.API/"]
COPY ["Rates.Core/Rates.Core.csproj", "Rates.Core/"]
COPY ["Rates.Infrastructure/Rates.Infrastructure.csproj", "Rates.Infrastructure/"]
RUN dotnet restore "Rates.API/Rates.API.csproj"

# Copy the remaining files and build the project
COPY . .
WORKDIR "/src/Rates.API"
RUN dotnet build "Rates.API.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "Rates.API.csproj" -c Release -o /app/publish

# Build the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rates.API.dll"]
