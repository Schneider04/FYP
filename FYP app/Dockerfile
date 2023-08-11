#Dockerfile used to create Docker image for our FYPfinalWEBAPP application

#defines base image for subsequent stages of building the image. this stage is name "base"
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#sets working directory inside Docker image to /app
WORKDIR /app
#exposing port
EXPOSE 80
EXPOSE 443

#using another image for building application. stage is named "build"
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#set working directory to /src inside Docker container where all application source code will be copied and built
WORKDIR /src
#copies project file FYPfinalWEBAPP.csproj from host's current directory into container's /src directory
COPY ["FYPfinalWEBAPP.csproj", "."]
#executes dotnet restore command to restore NuGet packages specified in project file FYPfinalWEBAPP.csproj
RUN dotnet restore "./FYPfinalWEBAPP.csproj"
#copies all files and directories from host's current directory into container's /src directory
COPY . .
#change working directory to /src
WORKDIR "/src/."
#executes dotnet build command to build application in release configuration and outputs build artifacts to /app/build directory
RUN dotnet build "FYPfinalWEBAPP.csproj" -c Release -o /app/build

#new stage named "publish" started based on "build" stage
FROM build AS publish
#executes dotnet publish command to publish application in release configuration and outputs published files to /app/publish
#/p:UseAppHost=false is used to indicate not to use app host when publishing
RUN dotnet publish "FYPfinalWEBAPP.csproj" -c Release -o /app/publish /p:UseAppHost=false

#new stage named "final" started based on "base" stage
FROM base AS final
#set working directory to /app 
WORKDIR /app
#copies published files from "publish" stages into current directory 
COPY --from=publish /app/publish .
"specifies command that will be executed when container started from this image, runs FYPfinalWEBAPP.dll assembly using dotnet command
ENTRYPOINT ["dotnet", "FYPfinalWEBAPP.dll"]
