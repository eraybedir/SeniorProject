FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy the project file
COPY backend/*.csproj ./
RUN dotnet restore

# Copy the rest of the code
COPY backend/ ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "MarketAPI.dll"]
