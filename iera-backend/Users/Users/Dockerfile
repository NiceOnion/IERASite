FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 32776

ENV ASPNETCORE_URLS=http://+:32776

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["iera-backend/Users/Users/Users.csproj", "iera-backend/Users/Users/"]
RUN dotnet restore "iera-backend/Users/Users/Users.csproj"
COPY . .
WORKDIR "/src/iera-backend/Users/Users"
RUN dotnet build "Users.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Users.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.dll"]
