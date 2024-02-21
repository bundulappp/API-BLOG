	FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
	WORKDIR /src
	COPY ["blog-rest-api.csproj", "./"]
	COPY ["../Logic/Logic.csproj", "/Logic/"]