FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["blog-rest-api.csproj", "./"]
COPY ["Logic/Logic.csproj", "Logic/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Data/Data.csproj","Data/"]
RUN dotnet restore "blog-rest-api.csproj"
RUN dotnet restore "Logic/Logic.csproj"
RUN dotnet restore "Models/Models.csproj"
RUN dotnet restore "Data/Data.csproj"

COPY . .
RUN dotnet build "blog-rest-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "blog-rest-api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "blog-rest-api.dll"]
