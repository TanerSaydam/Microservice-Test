#çalýþacaðý ortamý belirliyor
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#container da çalýþma klasörü
WORKDIR /app
#container dýþ dünyaya açýlacaðý HTTP port
EXPOSE 8080

#derleme ijamý
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#configuration ortamý için argüman kaydediliyor
ARG BUILD_CONFIGURATION=Release
#derleme iþlemlerinin yapýlacaðý çalýþma klasörü
WORKDIR /src
#prroje dosyasý src klasörüne kopyalanýyor
COPY ["./Microservice.CategoryWebAPI/Microservice.CategoryWebAPI.csproj", "Microservice.CategoryWebAPI/"]
#kopylanmýþ dosya src altýnda çalýþtýrýlýp paketler indiriliyor
RUN dotnet restore "./Microservice.CategoryWebAPI/Microservice.CategoryWebAPI.csproj"
#tüm proje dosyalarý container içine kopyalanýyor
COPY . .
#derleme için klasör seçiliyor
WORKDIR /src/Microservice.CategoryWebAPI
#argüman c ile alýnýp derlenen proje -o daki klasöre yönlendiriliyor
RUN dotnet build "./Microservice.CategoryWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

#publish
FROM build AS publish
#configuration ortamý için argüman kaydediliyor
ARG BUILD_CONFIGURATION=Release
#argüman c ile alýnýp publish edilen proje -o daki klasöre yönlendiriliyor
RUN dotnet publish "./Microservice.CategoryWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish

#final aþamasý
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservice.CategoryWebAPI.dll"]

#docker build -t categoryapi -f Microservice.CategoryWebAPI/Dockerfile .
#docker run -d -p 6002:8080 --name categoryapi categoryapi