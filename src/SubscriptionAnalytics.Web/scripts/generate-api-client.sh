@echo off

REM Read API URL from environment file
for /f "tokens=2 delims='" %%i in ('findstr "apiBaseUrl:" src\environments\environment.ts') do set API_BASE_URL=%%i

if "%API_BASE_URL%"=="" (
    echo ❌ Could not find apiBaseUrl in environment file
    exit /b 1
)

echo 🔗 Using API URL: %API_BASE_URL%

REM Remove existing API client
echo 🧹 Removing existing API client...
if exist src\app\api-client rmdir /s /q src\app\api-client

REM Generate new API client
echo 🔄 Generating API client from %API_BASE_URL%/swagger/v1/swagger.json...

docker run --rm ^
  -v "%cd%:/local" ^
  openapitools/openapi-generator-cli generate ^
  -i "%API_BASE_URL%/swagger/v1/swagger.json" ^
  -g typescript-angular ^
  -o /local/src/app/api-client ^
  --additional-properties=ngVersion=18.2.11,stringEnums=true,enumPropertyNaming=original,modelPropertyNaming=original,serviceSuffix=Service,serviceFileSuffix=.service,fileNaming=kebab-case

echo ✅ API client generated successfully!