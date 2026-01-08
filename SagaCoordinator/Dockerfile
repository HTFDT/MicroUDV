FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["SagaCoordinator/SagaCoordinator.csproj", "SagaCoordinator/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Shared.EF/Shared.EF.csproj", "Shared.EF/"]
COPY ["Shared.MT/Shared.MT.csproj", "Shared.MT/"]

RUN dotnet restore "SagaCoordinator/SagaCoordinator.csproj"

COPY ["SagaCoordinator/", "SagaCoordinator/"]
COPY ["Shared/", "Shared/"]
COPY ["Shared.EF/", "Shared.EF/"]
COPY ["Shared.MT/", "Shared.MT/"]

WORKDIR "/src/SagaCoordinator"
RUN dotnet publish "SagaCoordinator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SagaCoordinator.dll"]
