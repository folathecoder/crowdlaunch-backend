#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /root
COPY ["src/crowdlaunch-backend.csproj", "src/"]
RUN dotnet restore "src/crowdlaunch-backend.csproj"
COPY . .
WORKDIR "/root/src"
RUN dotnet build "crowdlaunch-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "crowdlaunch-backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "crowdlaunch-backend.dll", "--environment=Development"]
