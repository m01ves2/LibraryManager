# LibraryManager

## Overview

**LibraryManager** is a CLI-based library management application designed to demonstrate a clean, multi-layered architecture with advanced software engineering practices. The project implements a fully functional library system with authors and books, supporting CRUD operations, pagination, filtering, and a flexible UI through switchable screens.

---

## Architecture

The project follows a **multi-layered architecture**:

- **Domain Layer**: Contains entities (`Author`, `Book`) and enforces business invariants.
- **Application Layer**: Implements **UseCases** (commands and queries) to orchestrate business logic, ensuring separation of responsibilities.
- **Infrastructure Layer**: Provides persistence through EF Core (SQLite) and in-memory implementations for testing. Also contains repositories (command and query side for CQRS) and the unit-of-work pattern.

**Key architectural patterns:**

- **CQRS (Command-Query Responsibility Segregation)**: Commands modify state via repositories through a Unit of Work; queries read data via dedicated repositories returning DTOs (`AuthorPreview`, `BookPreview`) with `AsNoTracking()`.
- **Repository Pattern**: Command and query repositories abstract data access.
- **Unit of Work**: Used for transactional operations in the command side.
- **UseCase-based Application Layer**: All operations go through application services for orchestration.
- **Screen-based UI (CLI)**: Supports multiple switchable screens with clear separation of concerns.
- **Multiple Application-Infrastructure Interaction Models**: Includes:
  1. Standard repository pattern
  2. Direct DbContext access within Application
  3. Query objects returning DTOs
  4. LINQ expressions for dynamic filtering (prepared for future extension)
  5. Minimal CQRS implementation

---

## Features Implemented

- Add, remove, list authors and books
- Pagination for lists
- Basic filtering (by author or book name)
- CLI interface with multiple switchable screens
- In-memory and EF Core SQLite storage options
- UseCases coordinating Application layer logic
- Command and Query separation (CQRS)
- DTO projections (`AuthorPreview`, `BookPreview`) for Query-side
- Unit of Work for transactional operations on write-side
- Validation and operation results (`OperationResult<T>`)

---

## Features Not Implemented / Limitations

- Advanced filtering with expressions is not implemented (currently uses simple parameters)
- Asynchronous operations (`async/await`) are not used
- No integration tests; testing is left for future projects
- No caching or read-replica optimization (Query-side directly hits DbContext)
- No web interface; CLI only
- Minimal CQRS only; no Dapper or separate read database

---

## Technology Stack

- **Language**: C# (.NET 8)
- **Persistence**: EF Core (SQLite), In-Memory
- **UI**: Console (CLI) with multiple screens
- **Patterns**: CQRS, Repository, Unit of Work, UseCase-based Application Layer, DTO projections

---

## How to Run

1. Clone the repository
2. Restore NuGet packages
3. Build the solution
4. Run `LibraryManager.UI.CLI` project
5. Navigate through the CLI screens

---

## Future Improvements

- Add async/await support for better scalability
- Extend filtering with expressions and dynamic queries
- Implement caching for Query-side for high-performance reads
- Add integration and unit tests
- Optionally extend UI to a web interface

---

## Notes

This project was designed as a **learning and portfolio project**. It demonstrates:

- Clean architecture principles
- Separation of concerns across layers
- Advanced Application layer orchestration
- Practical usage of EF Core and In-Memory databases
- Minimal CQRS implementation in a small-scale CLI project