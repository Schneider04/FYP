FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
#exposing port
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FYPfinalWEBAPP.csproj", "."]
RUN dotnet restore "./FYPfinalWEBAPP.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "FYPfinalWEBAPP.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FYPfinalWEBAPP.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FYPfinalWEBAPP.dll"]
