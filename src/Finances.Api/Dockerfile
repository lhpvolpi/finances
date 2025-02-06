#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Finances.Api/Finances.Api.csproj", "Finances.Api/"]
COPY ["Finances.Core/Finances.Core.csproj", "Finances.Core/"]
COPY ["Finances.Infra/Finances.Infra.csproj", "Finances.Infra/"]
RUN dotnet restore "Finances.Api/Finances.Api.csproj"
COPY . .
WORKDIR "/src/Finances.Api"
RUN dotnet build "Finances.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Finances.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Finances.Api.dll"]
