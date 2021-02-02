FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Docker.Console/Docker.Console.csproj", "Docker.Console/"]
RUN dotnet restore "Docker.Console\Docker.Console.csproj"
COPY . .
WORKDIR "/src/Docker.Console"
RUN dotnet build "Docker.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Docker.Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Docker.Console.dll"]
