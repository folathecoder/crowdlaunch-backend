# dotnet 6 web api dockerfile

# Build stage for MarketPlaceApi
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the entire solution
COPY . .

# Build the application
WORKDIR "/app/MarketPlaceApi"
RUN dotnet build "BookStoreApi.csproj" -c Release -o /app/build

# Test stage
FROM build AS test
WORKDIR /app/MarketPlaceApi.Tests
RUN dotnet test "MarketPlaceApi.Tests.csproj" --logger:trx

# Publish stage for MarketPlaceApi
FROM build AS publish
WORKDIR /app/MarketPlaceApi
RUN dotnet publish "BookStoreApi.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

# Copy published files from MarketPlaceApi
COPY --from=publish /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "BookStoreApi.dll"]
