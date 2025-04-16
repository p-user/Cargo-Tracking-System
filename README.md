# 📦 Cargo Tracking System — Microservices Architecture in .NET

The **Cargo Tracking System** is a distributed microservices-based application developed using **.NET**, designed to manage and track cargo shipments efficiently. The system is containerized with **Docker** and adopts modern architectural patterns to ensure scalability, maintainability, and separation of concerns.

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

- 🧱 **Microservices-based** with clear service boundaries.
- 🚢 **Docker** for containerization and isolated service deployment.
- ⚔️ **CQRS + MediatR** used across services for clean separation of read/write logic.
- 🗂️ **Vertical Slice Architecture** adopted in all services except Routing for modular feature-based design.
- 🌐 **gRPC** used for high-performance communication in the Routing Service.
- 📍 **Google Maps API** integrated for routing and distance estimation.
- 📬 **Email Integration** in Notification Service for user updates.
