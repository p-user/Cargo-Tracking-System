# 📦 Cargo Tracking System — Microservices Architecture in .NET

The **Cargo Tracking System** is a fictional cargo  delivery, distributed microservices-based application developed using **.NET**, designed to manage and track cargo shipments efficiently.It leverages various modern architectural patterns and technologies, including **Microservices Architecture**, **Vertical Slice Architecture**, **CQRS**, **Domain-Driven Design (DDD)**, and **Event-Driven Architecture**. Communication between independent services primarily occurs through asynchronous messaging using **RabbitMQ** with the **MassTransit** library. For real-time scenarios, the system also supports synchronous communication via **REST** and **gRPC**.
The entire system is containerized with **Docker** and adopts modern architectural patterns to ensure scalability, maintainability, and separation of concerns.


> 💡 **Note**  
> This application is **not business-oriented**. The primary focus is on the **technical aspects** — implementing a sample project that showcases various **technologies**, **software architecture designs**, **principles**, and all the essentials required to build a modern **microservices application**.

> ⚠️ **Warning**  
> This project is **a work in progress**. New features and improvements are added over time.

---

## 🧩 Microservices Overview

### ✅ Order Service
- Manages cargo data such as **weight**, **dimensions**, and **shipment details**.
- Implements **Vertical Slice Architecture** with **CQRS** and **MediatR**.
- Handles creation, validation, and management of cargo orders.

### ✅ Notification Service
- Sends **email notifications** to users about shipment status updates and alerts.
- Built with **Vertical Slice Architecture**, **CQRS**, and **MediatR**.
- Integrates with email services (e.g., SMTP or third-party providers).

### ✅ Routing Service
- Calculates optimal routes using the **Google Maps API**.
- Developed using a traditional **N-layered architecture**.
- Exposes a **gRPC interface** for other services to query routes and waypoints.
- Includes logic for waypoint reduction and distance-based routing.

### ✅ Tracking Service
- Tracks the real-time **location and status** of cargo shipments.
- Uses **CQRS**, **MediatR**, and **Vertical Slice Architecture**.
- Supports integration with third-party tracking providers or GPS feeds.

---

## ⚙️ Architecture & Technologies

- 🧱 **Microservices-based** architecture with clear service boundaries.
- 🚢 **Docker** for containerization and isolated service deployment.
- ⚔️ **CQRS + MediatR** used across services for clean separation of read/write logic.
- 🗂️ **Vertical Slice Architecture** adopted in all services (except the Routing Service) for modular, feature-based design.
- 🌐 **gRPC** used in the Routing Service for high-performance, real-time communication.
- 📍 **Google Maps API** integrated for routing and distance estimation.
- 📬 **RabbitMQ + MassTransit** for asynchronous messaging between services.
- ✉️ **Email integration** in the Notification Service for sending user updates.
- 📊 **Structured logging** using **Serilog**, with logs exported to **Elasticsearch** and visualized in **Kibana** via the `serilog-sinks-elasticsearch` sink.
- 📦 **Outbox Pattern** implemented across all microservices for **guaranteed** or **at-least-once delivery**.
- 🔀 **YARP** reverse proxy used as the **API Gateway** for routing external traffic to internal services.




## Postman Collections and Environment

Postman collections and environment for testing APIs are located in the `/Postman` folder.

### Steps to Import

1. Open Postman.
2. Go to `File -> Import`.
3. Import the collection(s) from `/Postman/Collections/`.
4. Import the environment from `/Postman/Environments/`.
5. Select the imported environment.

### Notes
- Make sure your local services (via Docker Compose) are running before sending requests.
- Postman does not support exporting gRPC collections yet. You will need to import the proto file for yourself! 