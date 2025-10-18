<!-- Use this file to provide workspace-specific custom instructions to Copilot. For more details, visit https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file -->

## .NET Core Web API Project - UserApi

This workspace contains a complete .NET Core Web API for user management with CRUD operations, validation, and middleware.

### Project Status: ✅ COMPLETED

- [x] Verify that the copilot-instructions.md file in the .github directory is created.
- [x] Clarify Project Requirements
- [x] Scaffold the Project
- [x] Customize the Project
- [x] Install Required Extensions
- [x] Compile the Project
- [x] Create and Run Task
- [x] Launch the Project
- [x] Ensure Documentation is Complete

### Project Features

- **Full CRUD Operations**: Complete user management API
- **Data Validation**: Comprehensive input validation using Data Annotations
- **Authentication Middleware**: API key-based authentication
- **Request Logging**: Detailed request/response logging middleware
- **Health Checks**: Built-in health monitoring
- **CORS Support**: Cross-origin resource sharing enabled
- **OpenAPI Integration**: Swagger/OpenAPI documentation

### Quick Start

1. **Run the API**: Use the "Build and Run UserApi" task in VS Code
2. **Access the API**: Navigate to `https://localhost:7170`
3. **API Key**: Use `your-secret-api-key-here` in the `X-API-Key` header
4. **Documentation**: See README.md for complete API documentation

### API Endpoints

- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update existing user
- `DELETE /api/users/{id}` - Delete user
- `GET /health` - Health check
