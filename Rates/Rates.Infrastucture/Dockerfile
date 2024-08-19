FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Rates.Infrastucture/Rates.Infrastucture.csproj", "Rates.Infrastucture/"]
RUN dotnet restore "Rates.Infrastucture/Rates.Infrastucture.csproj"
COPY . .
WORKDIR "/src/Rates.Infrastucture"
RUN dotnet build "Rates.Infrastucture.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rates.Infrastucture.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rates.Infrastucture.dll"]
