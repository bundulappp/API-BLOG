FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY ["blog-rest-api/blog-rest-api.csproj", "blog-rest-api/"]
RUN dotnet restore "blog-rest-api/blog-rest-api.csproj"

COPY . ./
WORKDIR "/app/blog-rest-api"
RUN dotnet build "blog-rest-api.csproj" -c Release -o /app/build

FROM build-env AS publish
RUN dotnet publish "blog-rest-api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog-Rest-Api.dll"]
