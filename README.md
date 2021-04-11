# Masiv Casino Api

This is a .Net Core v3.1 Web Api. Use Redis Cache for distribuided data and optimized for horizontal scaling. This API implements docker.

<br>

# Architecture

This project consists it's build in ONION architecture with the following layers:

##### **1. Domain**: Contains three projects:

- **Entities**: Business models
- **Interfaces**: Business contracts
- **Services**: Business logic implementation

##### **2. Infrastructure**: External Agents

- **Data**: Implements Cache Redis Repository
- **IoC**: Implements Dependency Injection

#### **3. Application**: Layer for UI access

- **Contracts**: Contains the necessary DTO's
- **Interfaces**: UI Contracts
- **Services**: UI Services (Implements UI Contracts)

#### **4. WebApi**: API for the client access

- **Controllers**: Endpoints for the application access
- **Middleware**: Implements Global Event Handler and Logger.

#### **5. Test**: Layer test

- **UnitTest**: Test for bussines logic (services).
- **IntegrationTest**: Test for verify correct flow between application layers (controllers).

<br>

# Instructions

#### **1.** Clone the Repo

#### **2.** If required change port bindings in docker-compose.yml

#### **3.** Execute from the root folder `docker-compose build`

#### **4.** Start the everything up `docker-compose up`

#### **5.** Open browser in `http://localhost:{DefaultPort}/swagger`. **DefaultPort: 6001**

#### **6.** Populate the Redis cache data executing method create `/casino/roulette/create`

#### **7.** Access the roulette data using Http Get to access `/casino/roulette/all`

#### **8.** Finish by cleaning up... `docker-compose down`

<br>

# Authors

- #### **Wilmar Duque** - wilmarduque71@gmail.com
