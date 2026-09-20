# StockMarket

<div align="center">

![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-Console%20App-239120?logo=csharp&logoColor=white)
![JSON](https://img.shields.io/badge/Data-JSON-000000?logo=json&logoColor=white)
![Event-Driven](https://img.shields.io/badge/Design-Event%20Driven-2E7D32)

A feature-rich, event-driven stock market simulation built with C# and .NET 10. The application allows customers to buy and sell stocks, tracks portfolio data in JSON files, logs transactions, and raises notifications when stock prices change or customer events occur.

</div>

## Overview

StockMarket is a console-based investment simulation designed to model a simplified stock trading experience. It combines persistent data storage, inventory management, transaction processing, and event-driven alerting to provide a realistic and educational example of how market logic can be structured in a C# application.

This project is ideal for demonstrating:
- Domain-driven design in a small but practical business scenario
- Event-based communication between services and workflows
- File-backed persistence with JSON storage
- Console UX patterns for interactive business applications
- Clean separation between data access, business logic, and event handlers

## Features

- Customer management with create and lookup workflows
- Stock creation and price tracking
- Buy/sell stock transactions for individual customers
- Persistent JSON-based storage for customers, inventories, and stock data
- Event-driven price updates and transaction notifications
- Alerting when stock price movement crosses a configured threshold
- Customer activity logs stored in `wwwRoot/stockmarket.log`
- Interactive console application for live simulation

## Architecture

The solution follows a simple layered design:

- `DAL/` — JSON data access and persistence logic
- `Model/` — business entities such as `Customer`, `Stock`, and `Inventory`
- `Model/Event/` — custom event argument classes for transaction and alert events
- `Process/` — business workflows and event service handlers
- `Program.cs` — entry point and console workflow
- `wwwRoot/` — local JSON and log data files

## Core Domain Model

- `Customer`
  - Stores customer identity and display name
- `Stock`
  - Tracks stock name, current price, and previous price
- `Inventory`
  - Maps a customer to their owned stock IDs
- `Transaction`
  - Handles buy/sell logic and updates stock pricing

## How it Works

1. The application loads customer, stock, and inventory records from JSON files in `wwwRoot`.
2. A user logs in by customer name or creates a new customer profile.
3. The user can choose to buy or sell available stocks.
4. Each transaction updates the stock price, persists the change, and raises events.
5. Event subscribers log activity and trigger price alerts based on configured thresholds.

## Technology Stack

- C#
- .NET 10
- Console application model
- JSON serialization with `System.Text.Json`
- Event-driven architecture using `EventHandler<T>`

## Getting Started

### Prerequisites

- .NET 10 SDK or compatible .NET 10 runtime
- Visual Studio 2026 / VS Code / .NET CLI

### Clone the Repository

```bash
git clone https://github.com/imdumbo/StockMarket.git
cd StockMarket
```

### Run the Application

```bash
dotnet run
```

## Example Interaction

When the app starts, it seeds sample customer and stock data, then prompts the user to log in or create a customer.

Typical commands available in the console:
- `BUY`
- `SELL`
- `NEW LOGIN`
- `EXIT`

Example flow:

```text
Please enter your name, or type EXIT to close:
John Doe

Enter BUY, SELL, NEW LOGIN, or EXIT:
BUY
Available stocks:
AAPL | Current: $150.00 | Previous: $0.00
GOOGL | Current: $2500.00 | Previous: $0.00
MSFT | Current: $300.00 | Previous: $0.00
Please enter stock name (or EXIT):
AAPL
Stock purchased successfully.
```

## Event and Alert System

The application uses events to decouple business actions from notifications and logging:

- `StockPurchased`
- `StockSold`
- `PriceChanged`
- `CustomerCreated`
- `CustomerDeleted`

These events are consumed by:
- `StockMarketEventHandlers` for transaction logging
- `PriceAlertService` for threshold-based notifications
- `CustomerAlertService` for customer lifecycle tracking

## Project Structure

```text
StockMarket/
├── DAL/
│   ├── DataBase.cs
│   └── MyDataBaseProcess.cs
├── Model/
│   ├── Customer.cs
│   ├── Inventory.cs
│   ├── Stock.cs
│   └── Event/
│       ├── CustomerEventArgs.cs
│       ├── PriceChangedEventArgs.cs
│       └── StockMarketEventArgs.cs
├── Process/
│   ├── CustomerAlertService.cs
│   ├── CustomerProcess.cs
│   ├── PriceAlertService.cs
│   ├── StockMarketEventHandlers.cs
│   ├── StockProcess.cs
│   └── Transaction.cs
├── wwwRoot/
│   ├── Customers.json
│   ├── Inventories.json
│   ├── Stocks.json
│   └── stockmarket.log
├── Program.cs
├── README.md
├── StockMarket.csproj
└── .gitignore
```

## Persistence Model

The application persists all state locally in JSON format:

- `Customers.json` — customer records
- `Inventories.json` — customer-to-stock ownership mapping
- `Stocks.json` — stock metadata and pricing history
- `stockmarket.log` — operational event trail and user alerts

## Why This Project Matters

This project demonstrates how a simple business scenario can be modeled with a clean separation of concerns:
- domain objects capture business entities
- service/process classes handle the workflow logic
- event handlers centralize monitoring and logging
- JSON persistence keeps the app lightweight and easy to run locally