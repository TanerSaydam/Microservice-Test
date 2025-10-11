# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Microservice.OcelotGateway/Microservice.OcelotGateway.csproj", "Microservice.OcelotGateway/"]
RUN dotnet restore "./Microservice.OcelotGateway/Microservice.OcelotGateway.csproj"
COPY . .
WORKDIR "/src/Microservice.OcelotGateway"
RUN dotnet build "./Microservice.OcelotGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Microservice.OcelotGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservice.OcelotGateway.dll"]

#docker build -t ocelotgateway -f Microservice.OcelotGateway/Dockerfile .
#docker run -d -p 5001:8080 --name ocelotgateway ocelotgateway