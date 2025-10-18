# UserApi - .NET Core Web API

A comprehensive .NET Core Web API for user management with CRUD operations, validation, and middleware.

## Features

- **Full CRUD Operations**: Create, Read, Update, Delete users
- **Data Validation**: Comprehensive input validation using Data Annotations
- **Authentication Middleware**: API key-based authentication
- **Request Logging**: Detailed request/response logging middleware
- **Health Checks**: Built-in health monitoring
- **CORS Support**: Cross-origin resource sharing enabled
- **OpenAPI Integration**: Swagger/OpenAPI documentation

## API Endpoints

### Base URL
- HTTPS: `https://localhost:7170`
- HTTP: `http://localhost:5144`

### Authentication
All endpoints (except `/health` and `/openapi`) require an API key in the `X-API-Key` header.

**Default API Key**: `your-secret-api-key-here`

### User Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/users` | Get all users |
| GET | `/api/users/{id}` | Get user by ID |
| POST | `/api/users` | Create new user |
| PUT | `/api/users/{id}` | Update existing user |
| DELETE | `/api/users/{id}` | Delete user |
| HEAD | `/api/users/{id}` | Check if user exists |

### Health Check
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/health` | Application health status |

## Data Models

### User
```json
{
  \"id\": 1,
  \"firstName\": \"John\",
  \"lastName\": \"Doe\",
  \"email\": \"john.doe@example.com\",
  \"phoneNumber\": \"+1-555-0123\",
  \"age\": 30,
  \"createdAt\": \"2025-10-18T10:00:00Z\",
  \"updatedAt\": \"2025-10-18T10:00:00Z\"
}
```

### UserCreateDto / UserUpdateDto
```json
{
  \"firstName\": \"John\",
  \"lastName\": \"Doe\",
  \"email\": \"john.doe@example.com\",
  \"phoneNumber\": \"+1-555-0123\",
  \"age\": 30
}
```

## Validation Rules

- **firstName**: Required, max 50 characters
- **lastName**: Required, max 50 characters
- **email**: Required, valid email format, max 100 characters, must be unique
- **phoneNumber**: Optional, valid phone format
- **age**: Required, between 18 and 120

## Example Requests

### Get All Users
```bash
curl -X GET \"https://localhost:7170/api/users\" \\
  -H \"X-API-Key: your-secret-api-key-here\"
```

### Get User by ID
```bash
curl -X GET \"https://localhost:7170/api/users/1\" \\
  -H \"X-API-Key: your-secret-api-key-here\"
```

### Create User
```bash
curl -X POST \"https://localhost:7170/api/users\" \\
  -H \"X-API-Key: your-secret-api-key-here\" \\
  -H \"Content-Type: application/json\" \\
  -d '{
    \"firstName\": \"Jane\",
    \"lastName\": \"Smith\",
    \"email\": \"jane.smith@example.com\",
    \"phoneNumber\": \"+1-555-0124\",
    \"age\": 28
  }'
```

### Update User
```bash
curl -X PUT \"https://localhost:7170/api/users/1\" \\
  -H \"X-API-Key: your-secret-api-key-here\" \\
  -H \"Content-Type: application/json\" \\
  -d '{
    \"firstName\": \"John\",
    \"lastName\": \"Updated\",
    \"email\": \"john.updated@example.com\",
    \"phoneNumber\": \"+1-555-9999\",
    \"age\": 31
  }'
```

### Delete User
```bash
curl -X DELETE \"https://localhost:7170/api/users/1\" \\
  -H \"X-API-Key: your-secret-api-key-here\"
```

## Getting Started

### Prerequisites
- .NET 9.0 SDK or later
- Visual Studio Code or Visual Studio

### Running the Application

1. **Clone and navigate to the project**:
   ```bash
   git clone <repository-url>
   cd projects
   ```

2. **Build the project**:
   ```bash
   dotnet build UserApi.csproj
   ```

3. **Run the application**:
   ```bash
   dotnet run --project UserApi.csproj
   ```

4. **Access the API**:
   - API Base URL: `https://localhost:7170`
   - Health Check: `https://localhost:7170/health`
   - OpenAPI: `https://localhost:7170/openapi`

### Using VS Code

1. Open the project in VS Code
2. Use `Ctrl+Shift+P` and run \"Tasks: Run Task\"
3. Select \"Build and Run UserApi\"

The application will start and be available at the configured URLs.

## Project Structure

```
├── Controllers/
│   └── UsersController.cs      # Main API controller
├── DTOs/
│   ├── UserCreateDto.cs        # User creation model
│   └── UserUpdateDto.cs        # User update model
├── Middleware/
│   ├── ApiKeyAuthenticationMiddleware.cs  # API key auth
│   └── RequestLoggingMiddleware.cs        # Request logging
├── Models/
│   └── User.cs                 # User entity model
├── Services/
│   ├── IUserService.cs         # Service interface
│   └── UserService.cs          # Service implementation
├── Properties/
│   └── launchSettings.json     # Launch configuration
├── .vscode/
│   └── tasks.json              # VS Code tasks
├── Program.cs                  # Application entry point
├── UserApi.csproj              # Project file
├── appsettings.json            # Configuration
└── README.md                   # This file
```

## Configuration

### API Key
Update the API key in `appsettings.json`:
```json
{
  \"ApiKey\": \"your-custom-api-key-here\"
}
```

### Logging
Logging levels can be configured in `appsettings.json`:
```json
{
  \"Logging\": {
    \"LogLevel\": {
      \"Default\": \"Information\",
      \"Microsoft.AspNetCore\": \"Warning\"
    }
  }
}
```

## Middleware

### Request Logging Middleware
- Logs all incoming requests and responses
- Adds unique request ID to response headers
- Measures response time
- Logs errors with details

### API Key Authentication Middleware
- Validates API key in `X-API-Key` header
- Skips authentication for health checks and OpenAPI endpoints
- Returns 401 for missing or invalid API keys

## Sample Data

The application includes two sample users:
1. John Doe (ID: 1) - john.doe@example.com
2. Jane Smith (ID: 2) - jane.smith@example.com

## Error Handling

The API returns appropriate HTTP status codes:
- `200 OK`: Successful operation
- `201 Created`: Resource created successfully
- `204 No Content`: Successful deletion
- `400 Bad Request`: Invalid input data
- `401 Unauthorized`: Missing or invalid API key
- `404 Not Found`: Resource not found
- `409 Conflict`: Duplicate email address

## Notes

- This implementation uses in-memory storage for demonstration purposes
- For production use, integrate with a database (Entity Framework, etc.)
- The API key authentication is simplified for demo purposes
- Consider implementing JWT tokens for production authentication
- Add rate limiting and additional security measures for production use