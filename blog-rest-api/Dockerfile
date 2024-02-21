FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["blog-rest-api.sln", "./"]
COPY ["blog-rest-api/blog-rest-api.csproj", "blog-rest-api/"]
COPY ["Logic/Logic.csproj", "Logic/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Data/Data.csproj", "Data/"]

RUN dotnet restore

COPY . .

WORKDIR "/src/blog-rest-api"
RUN dotnet build "blog-rest-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "blog-rest-api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "blog-rest-api.dll"]
