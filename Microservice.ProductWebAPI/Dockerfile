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
COPY ["./Microservice.ProductWebAPI/Microservice.ProductWebAPI.csproj", "Microservice.ProductWebAPI/"]
#kopylanmýþ dosya src altýnda çalýþtýrýlýp paketler indiriliyor
RUN dotnet restore "./Microservice.ProductWebAPI/Microservice.ProductWebAPI.csproj"
#tüm proje dosyalarý container içine kopyalanýyor
COPY . .
#derleme için klasör seçiliyor
WORKDIR /src/Microservice.ProductWebAPI
#argüman c ile alýnýp derlenen proje -o daki klasöre yönlendiriliyor
RUN dotnet build "./Microservice.ProductWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

#publish
FROM build AS publish
#configuration ortamý için argüman kaydediliyor
ARG BUILD_CONFIGURATION=Release
#argüman c ile alýnýp publish edilen proje -o daki klasöre yönlendiriliyor
RUN dotnet publish "./Microservice.ProductWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish

#final aþamasý
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservice.ProductWebAPI.dll"]

#docker build -t productapi -f Microservice.ProductWebAPI/Dockerfile .
#docker run -d -p 6001:8080 --name productapi productapi