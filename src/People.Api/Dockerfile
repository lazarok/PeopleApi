FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

ARG BUILD_CONFIGURATION=Release

COPY ["src/People.Api/People.Api.csproj", "src/People.Api/"]
COPY ["src/People.Application/People.Application.csproj", "src/People.Appplication/"]
COPY ["src/People.Domain/People.Domain.csproj", "src/People.Domain/"]
COPY ["src/People.Infrastructure.Persistence/People.Infrastructure.Persistence.csproj", "src/People.Infrastructure.Persistence/"]
COPY ["src/People.Infrastructure.Shared/People.Infrastructure.Shared.csproj", "src/People.Infrastructure.Shared/"]

RUN dotnet restore "src/People.Api/People.Api.csproj"


COPY src/ .
WORKDIR "/src/People.Api"
RUN dotnet build "People.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "People.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose the HTTP port
EXPOSE 5050

ENTRYPOINT ["dotnet", "People.Api.dll"]
