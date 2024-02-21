FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Adjust the path to where your csproj file is located
COPY ["blog-rest-api/blog-rest-api.csproj", "blog-rest-api/"]
RUN dotnet restore "blog-rest-api/blog-rest-api.csproj"

# Copy the rest of your source code
COPY . .

# Make sure to specify the correct path for the build context
RUN dotnet build "blog-rest-api/blog-rest-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "blog-rest-api/blog-rest-api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "blog-rest-api.dll"]
