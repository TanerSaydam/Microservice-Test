## Microservice Eğitim Programı
- Architectural patterns
- Microservice nedir?
- Modular Monolith nedir?
- Microservice vs Modular Monolith
- WebAPI ile Microservice inşa edelim
- OpenApi and Scalar
- Health Check
- Service Discovery Pattern (HashiCorp Consul)
- Resilience Pattern (Polly)
- Gateway nedir?
- Ocelot ile Gateway
- Docker
- CORS politikası
- Authentication
- Authorization
- QoS / Retry / Circuit Breaker
- LoadBalance
- RateLimit
- YARP ile Gateway
- Ocelot vs YARP
- Transaction (Saga Pattern)
- Observability
- Aspire

# Teorik bilgiler
- Shared Database Anti pattern
- Database-per Service Pattern
- API Composition Pattern
- Event Sourcing Pattern
```powershell
//Örnek reposu
https://github.com/TanerSaydam/EventSourcingDesignPattern
```

```powershell
//docker cli ile tek dockerfile image dönüştür ve container olarak çalıştır
docker build -t productwebapi .
docker build -f ETicaret.ProductWebAPI/Dockerfile -t productwebapi .
docker run -d -p 6001:8080 --name ProductWebAPI productwebapi

//Network oluştur ve birbirine bağla
docker network create mynetwork
docker run -d -network mynetwork -p 6001:8080 --name ProductWebAPI productwebapi

//Tüm network’leri listele
docker network ls

//Belirli bir network’ü sil
docker network rm network_adi

//Kullanılmayan tüm network’leri sil
docker network prune
```

```powershell
docker run -d --name consul -p 8500:8500 hashicorp/consul:latest

docker compose up -d
docker compose down
docker compose build
docker compose up -d --build
```
