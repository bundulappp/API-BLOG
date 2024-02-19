FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["blog-rest-api/blog-rest-api.csproj", "blog-rest-api/"]
RUN dotnet restore "./blog-rest-api/./blog-rest-api.csproj"
COPY . .
WORKDIR "/src/blog-rest-api"
RUN dotnet build "blog-rest-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "blog-rest-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "blog-rest-api.dll"]
