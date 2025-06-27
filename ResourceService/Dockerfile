FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["ConstructionProject.sln", "."]
COPY ["AuthService/AuthService.csproj", "AuthService/"]
COPY ["BidService/BidService.csproj", "BidService/"]
COPY ["ResourceService/ResourceService.csproj", "ResourceService/"]
COPY ["ConstructionProject.Contracts/ConstructionProject.Contracts.csproj", "ConstructionProject.Contracts/"]

RUN dotnet restore "ConstructionProject.sln"

COPY . .
WORKDIR "/src/ResourceService"
RUN dotnet build "ResourceService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ResourceService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ResourceService.dll"]
