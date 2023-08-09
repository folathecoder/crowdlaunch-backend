# dotnet 6 web api dockerfile

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
EXPOSE 80
COPY ["BookStoreApi.csproj", "./"]
RUN dotnet restore "BookStoreApi.csproj"
COPY . .
WORKDIR "/app/."
RUN dotnet build "BookStoreApi.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "BookStoreApi.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookStoreApi.dll"]
