# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Environment settings to avoid fallback problems
ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
ENV DOTNET_ROLL_FORWARD=Major
ENV NUGET_PACKAGES=/root/.nuget/packages

# Copy csproj and restore dependencies (inside container)
COPY ContactManagment/ContactManagment.csproj ./ContactManagment/
WORKDIR /src/ContactManagment
RUN dotnet restore --ignore-failed-sources

# Copy the rest of the source and build
COPY ContactManagment/. .
RUN dotnet publish -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ContactManagment.dll"]
