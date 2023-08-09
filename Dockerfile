FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80
COPY ["crowdlaunch-backend.csproj", "./"]
RUN dotnet restore "crowdlaunch-backend.csproj"
COPY . .
WORKDIR "/app/."
RUN dotnet build "crowdlaunch-backend.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "crowdlaunch-backend.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "crowdlaunch-backend.dll"]