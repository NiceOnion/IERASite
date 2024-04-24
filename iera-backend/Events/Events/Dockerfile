# Base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Events/Events/Events.csproj", "Events/"]
RUN dotnet restore "Events/Events.csproj"
COPY . .

# Create the /app/build directory with correct permissions
RUN mkdir -p /app/build && chmod -R 777 /app/build

WORKDIR "/src/Events"
RUN dotnet build "Events.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Events.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Events.dll"]
