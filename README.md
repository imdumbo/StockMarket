# EventService Stock Market

A .NET 10 console application that demonstrates a simple stock market workflow for managing customers, stocks, and customer holdings.

## Application Design

The application is organized into three layers:

```text
Program.cs
	↓
Process layer
	↓
DAL layer
	↓
JSON files in wwwRoot
```

### Model layer

The model layer contains the application's data objects:

- `Customer` represents a customer.
- `Stock` represents a stock and its current and previous price.
- `Inventory` represents the stocks owned by a customer.

### Process layer

The process layer contains the application operations:

- `CustomerProcess` creates, finds, and deletes customers.
- `StockProcess` creates stocks and handles stock selection from the console.
- `Transaction` applies BUY and SELL rules.

### Data access layer

`MyDataBaseProcess` loads and saves application data as JSON. The application uses one shared data process for all operations and stores data in three files:

```text
wwwRoot/
├── Customers.json
├── Stocks.json
└── Inventories.json
```

## How the Application Works

1. The application loads customers, stocks, and inventories from the JSON files.
2. Sample customers and stocks are added if they do not already exist.
3. The user enters a customer name.
4. The user can create a new customer if the name is not found.
5. The user selects one of the following operations:
   - `BUY` adds a stock to the customer's inventory.
   - `SELL` removes a stock from the customer's inventory.
   - `LOGIN` switches to another customer.
   - `EXIT` closes the application.
6. Each successful change is saved to the appropriate JSON file.

## Transaction Rules

- Customers and stocks are identified by unique `Guid` values.
- Customer and stock names are matched without regard to letter casing.
- A customer's inventory is created automatically during the first purchase.
- A customer cannot purchase the same stock more than once.
- A customer cannot sell a stock that they do not own.
- Deleting a customer also removes the customer's inventory.

## Project Structure

```text
DAL/
  DataBase.cs
  MyDataBaseProcess.cs
Model/
  Customer.cs
  Inventory.cs
  Stock.cs
Process/
  CustomerProcess.cs
  StockProcess.cs
  Transaction.cs
Program.cs
wwwRoot/
  Customers.json
  Inventories.json
  Stocks.json
```