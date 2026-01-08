FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["InventoryService/InventoryService.csproj", "InventoryService/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Shared.EF/Shared.EF.csproj", "Shared.EF/"]
COPY ["Shared.MT/Shared.MT.csproj", "Shared.MT/"]

RUN dotnet restore "InventoryService/InventoryService.csproj"

COPY ["InventoryService/", "InventoryService/"]
COPY ["Shared/", "Shared/"]
COPY ["Shared.EF/", "Shared.EF/"]
COPY ["Shared.MT/", "Shared.MT/"]

WORKDIR "/src/InventoryService"
RUN dotnet publish "InventoryService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "InventoryService.dll"]
