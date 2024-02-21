FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["blog-rest-api/blog-rest-api.csproj", "blog-rest-api/"]
RUN dotnet restore "blog-rest-api/blog-rest-api.csproj"

COPY . .
WORKDIR "/src/blog-rest-api"
RUN dotnet build "/src/blog-rest-apiblog-rest-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/src/blog-rest-apiblog-rest-api.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "blog-rest-api.dll"]
