FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Since the Dockerfile is inside blog-rest-api, no need to specify it in the COPY path
COPY ["blog-rest-api.csproj", "./"]
RUN dotnet restore "blog-rest-api.csproj"
# The context is the current directory, so just copy everything from the current directory
COPY . .
RUN dotnet build "blog-rest-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "blog-rest-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "blog-rest-api.dll"]
