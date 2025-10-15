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
-- Örnek reposu
```dash
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

## Saga Pattern
Orchestration → Merkezî bir servis var. O, isteği sırayla diğer servislere gönderir ve her cevaba göre bir sonraki adımı belirler.
Choreography → Her servis ortak bir kuyruktan aynı mesajı dinler, kendi işini yapar ve sonucu ayrı bir event olarak yayar. Başka servisler de o sonucu dinleyip kendi adımlarını yürütür.

- RabbitMQ Docker Kodu
```bash
docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

- Örnek Reposu
```dash
https://github.com/TanerSaydam/SagaPattern
```
