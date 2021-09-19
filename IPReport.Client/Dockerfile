FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["IPReport.Client.csproj", "./"]
RUN dotnet restore "./IPReport.Client.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "IPReport.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IPReport.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY "appsettings.json" .
ENTRYPOINT ["dotnet", "IPReport.Client.dll"]