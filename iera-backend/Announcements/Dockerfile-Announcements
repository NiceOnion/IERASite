# Base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Announcements/Announcements/Announcements.csproj", "Announcements/"]
RUN dotnet restore "Announcements/Announcements.csproj"
COPY . .
WORKDIR "/src/Announcements/Announcements"
RUN dotnet build "Announcements.csproj" -c Release -o /app/build

# Change ownership of the /app/build directory to allow writing during the build process
USER root
RUN chown -R nobody:nogroup /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Announcements.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Announcements.dll"]
