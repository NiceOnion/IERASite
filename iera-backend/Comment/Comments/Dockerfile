FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 32778

ENV ASPNETCORE_URLS=http://+:32778

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["iera-backend/Comment/Comments/Comments.csproj", "iera-backend/Comment/Comments/"]
RUN dotnet restore "iera-backend/Comment/Comments/Comments.csproj"
COPY . .
WORKDIR "/src/iera-backend/Comment/Comments"
RUN dotnet build "Comments.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Comments.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Comments.dll"]
