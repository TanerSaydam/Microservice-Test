## Microservice E�itim Program�
- Architectural patterns
- Microservice nedir?
- Modular Monolith nedir?
- Microservice vs Modular Monolith
- WebAPI ile Microservice in�a edelim
- OpenApi and Scalar
- Health Check
- Service Discovery (HashiCorp Consul)
- Resilience Pattern (Polly)
- Gateway nedir?
- Ocelot ile Gateway
- Docker
- CORS politikas�
- Authentication
- Authorization
- QoS / Retry / Circuit Breaker
- LoadBalance
- RateLimit
- Observability
- YARP ile Gateway
- Ocelot vs YARP
- Aspire
- Transaction (Saga Pattern)
- Event Sourcing Design Pattern
- API Composition Design Pattern


docker run -d --name consul -p 8500:8500 hashicorp/consul:latest

docker compose up -d
docker compose down
docker compose build
docker compose up -d --build
