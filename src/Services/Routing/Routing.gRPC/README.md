# 📦 Routing Service - Cargo Tracking System

The **Routing Service** is a dedicated microservice responsible for planning and managing delivery routes for cargo shipments. 
It integrates with Google Maps to determine optimal paths between pickup and drop-off locations and exposes gRPC endpoints for interaction with other services like Order, Tracking, and Notification.

---

## 🚀 Why Routing Service 


- Using this service, we aim to  receive a clear route without waypoints, so we get to know where to pick up and drop off cargo.
- We track the estimated delivery time based on the planned route.

---

## 🔄 Interactions with Other Modules

| Module        | Interaction                                                                 |
|---------------|-------------------------------------------------------------------------------|
| **Order**     | Sends a `RoutePlanRequest` to generate a new route when an order is created. |
| **Tracking**  | Queries for the planned route to match real-time GPS locations.              |
| **Notification** | Gets notified if there’s a route change or significant ETA deviation.     |
| **Google Maps API** | Used to calculate distance, travel time, and generate waypoints.       |

---

## 📡 gRPC APIs

The Routing Service exposes gRPC endpoints for internal communication.

