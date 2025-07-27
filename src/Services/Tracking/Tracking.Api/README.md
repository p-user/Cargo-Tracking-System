# 📚 Event Sourcing Notes

> 📝 *In this service, I have implemented the Event Sourcing pattern using **Marten** and **PostgreSQL** as the Event Store.*

## 🧠 What is Event Sourcing?

**Event Sourcing** is a design pattern in which the results of business operations are stored as a series of events.

- It's an **alternative way to persist data**.
- In contrast with state-oriented persistence (which only keeps the latest version of the entity data), event sourcing **stores each state change as a separate event**.
- Each business operation result is stored as an event.

---

## 🔁 Events

Events:
- Represent **facts in the past** – they carry information about something that was accomplished.
- Are **named in past tense**, like `"UserAdded"`, `"OrderUpdated"`, etc.
- Are **not directed to a specific recipient** – they are undisclosed information.
- Are **immutable** – they cannot be changed once stored.

---

## 🌊 Stream

A **stream** is a representation of entities. Events are logically grouped into streams.

- A stream should have a **unique identifier** representing the specific object.
- Each event has its own **unique position within a stream**.
  - This position is usually a **numeric incremented value**.
  - It defines the **order of events** while retrieving state.
  - It also helps to **detect concurrency issues**.

---

## 🧾 Event Representation

Each event includes:
- `id`: Unique event identifier
- `type`: Name of the event (e.g., `"UserUpdated"`)
- `streamId`: Object ID of the entity the event is related to
- `streamPosition`: Version or ordering number within the stream
- `timestamp`: When the event happened
- `data`: Payload of the event

---

## 🧵 Entity & Stream

In **Event Sourcing**, each entity is represented by its **stream**:

> The sequence of events correlated by `streamId`, ordered by `streamPosition`.
