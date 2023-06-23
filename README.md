# ZenDays API


The ZenDays API is a C# application designed to support a mobile app for vacation control. This API provides various endpoints to manage vacation-related information, including creating vacation requests, approving or rejecting requests, retrieving vacation history, and managing user accounts.

## Features

- User authentication and authorization
- Vacation request management
- Vacation request approval/rejection
- Vacation history retrieval
- User account management

## Technologies Used

- C# programming language
- ASP.NET Core framework
- Entity Framework Core
- MySQL database
- JSON Web Tokens (JWT) for authentication
- Swagger UI for API documentation

## Getting Started

To run the ZenDays API locally, follow these steps:

1. Clone the repository:

2. Navigate to the project directory:

3. Configure the database connection string in the `appsettings.json` file:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Your-MySQL-Database-Connection-String"
   }
   ```

4. Apply the database migrations:

   ```bash
   dotnet ef database update
   ```

5. Build and run the API:

   ```bash
   dotnet run
   ```

   The API will be accessible at `https://localhost:7021;http://localhost:5150`.

## API Documentation

The API is documented using Swagger UI. Once the API is running, you can access the Swagger documentation at `https://localhost:7021/swagger;http://localhost:5150/swagger`. This documentation provides detailed information about each endpoint, including request/response examples and available parameters.

## Authentication and Authorization

The ZenDays API uses JWT-based authentication. To access protected endpoints, you need to obtain an access token by providing valid user credentials through the `/api/auth/login` endpoint. The access token should be included in the `Authorization` header of subsequent requests as a Bearer token.

## Endpoints

The following are the main endpoints provided by the ZenDays API (section is work in progress):

- **POST /api/v1/Auth/Login**: Authenticate and obtain an access token.


Please refer to the Swagger documentation for detailed information about request/response payloads and authentication requirements for each endpoint.

## Contributors

- Matheus Franco https://github.com/matheusfranco256
- Lucas Vinicius https://github.com/LucaasVGP
- Matheus Madrid https://github.com/MatheusMadrid473
