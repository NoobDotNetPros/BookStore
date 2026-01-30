# 🐳 Docker Instructions for BookStore

This guide explains how to run the BookStore application using Docker.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop) installed and running.

## Quick Start

1.  Open your terminal in the root `BookStore` directory (where `docker-compose.yml` is located).
2.  Run the following command to build and start the services:

    ```bash
    docker-compose up --build
    ```

3.  Wait for the build to complete and the services to start. You will see logs from both `backend` and `frontend`.

## Accessing the Application

- **Frontend**: [http://localhost:4200](http://localhost:4200)
- **Backend API**: [http://localhost:8080](http://localhost:8080) (and [Swagger UI](http://localhost:8080/swagger) if enabled)

## Database Configuration

The application is configured to run in a container, but it likely connects to a database running on your **host machine** (your computer).

To allow the container to access your local database (e.g., SQL Server), the `docker-compose.yml` file uses `host.docker.internal`.

**Important**: Ensure your connection string in `backend/Bookstore.Web/appsettings.json` points to the correct server.
- If using LocalDB: `Server=(localdb)\\mssqllocaldb;...` (might require TCP/IP enabled)
- If using SQL Server Developer/Express: `Server=host.docker.internal,1433;...` or `Server=YOUR_COMPUTER_NAME;...`

## Stopping the Application

To stop the containers, press `Ctrl+C` in the terminal, or run:

```bash
docker-compose down
```
