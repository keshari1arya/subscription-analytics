# API Client Generation Scripts

This directory contains scripts for generating the Angular API client from the .NET API's OpenAPI/Swagger specification.

## Scripts

### `generate-api-client.sh` (Unix/Linux/macOS)
- **Purpose**: Removes existing API client and generates a fresh one
- **Features**:
  - Checks if API server is running
  - Removes existing `api-client` directory
  - Generates fresh TypeScript Angular client
  - Provides detailed progress feedback
  - Error handling and validation

### `generate-api-client.bat` (Windows)
- **Purpose**: Windows equivalent of the shell script
- **Features**: Same functionality as the shell script but for Windows environments

## Usage

### Prerequisites
1. **API Server Running**: Make sure the .NET API is running on `http://localhost:5001`
2. **Docker**: Docker must be installed and running
3. **Node.js**: Required for the cross-platform script execution

### Commands

#### Using Yarn (Recommended)
```bash
# From the SubscriptionAnalytics.Web directory
yarn openapigen
```

#### Direct Script Execution
```bash
# Unix/Linux/macOS
./scripts/generate-api-client.sh

# Windows
scripts\generate-api-client.bat
```

#### Legacy Command (Still Available)
```bash
yarn generate-api
```

## What the Script Does

1. **Validation**: Checks if the API server is accessible
2. **Cleanup**: Removes the existing `src/app/api-client` directory
3. **Generation**: Uses OpenAPI Generator to create fresh TypeScript Angular client
4. **Configuration**: Applies proper Angular 18.2.11 settings
5. **Feedback**: Provides detailed progress and success/failure messages

## Generated Files

The script generates the following structure in `src/app/api-client/`:

```
api-client/
├── api/                    # API service classes
├── model/                  # TypeScript interfaces/models
├── configuration.ts        # API configuration
├── encoder.ts             # HTTP parameter encoding
├── variables.ts           # Constants and variables
└── api.base.service.ts    # Base service class
```

## Configuration

The generated client includes:
- **Angular Version**: 18.2.11
- **Service Injection**: `providedInRoot=true`
- **Interfaces**: `withInterfaces=true`
- **ES6 Support**: `supportsES6=true`
- **Authentication**: Bearer token support

## Troubleshooting

### API Not Running
```
❌ API is not running or not accessible at http://localhost:5001/swagger/v1/swagger.json
Please start the API server first:
  cd ../SubscriptionAnalytics.Api && dotnet run
```

**Solution**: Start the .NET API server first

### Docker Not Available
```
docker: command not found
```

**Solution**: Install Docker Desktop and ensure it's running

### Permission Denied (Unix/Linux/macOS)
```
Permission denied: ./scripts/generate-api-client.sh
```

**Solution**: Make the script executable:
```bash
chmod +x scripts/generate-api-client.sh
```

### Path Issues (Windows)
```
'scripts' is not recognized as an internal or external command
```

**Solution**: Use the yarn command instead:
```bash
yarn openapigen
```

## After Generation

After successful generation:
1. **Restart Angular Dev Server**: `yarn start`
2. **Check for Errors**: Look for any TypeScript compilation errors
3. **Update Imports**: Update any existing imports if the API structure changed

## Best Practices

1. **Run Before Development**: Generate fresh client when API changes
2. **Version Control**: Commit generated files to track API changes
3. **Test After Generation**: Verify that existing functionality still works
4. **API First**: Always ensure API is running before generation

## Integration with Development Workflow

### Typical Workflow
1. Make changes to .NET API
2. Start API server: `dotnet run`
3. Generate client: `yarn openapigen`
4. Restart Angular: `yarn start`
5. Test changes

### Automated Workflow (Future Enhancement)
Consider adding this to your CI/CD pipeline to automatically regenerate the client when the API changes. 