#!/usr/bin/env node

const { execSync, spawn } = require('child_process');
const http = require('http');
const fs = require('fs');
const path = require('path');

// Read environment configuration
function getEnvironmentConfig() {
  const envPath = path.join(__dirname, '../src/environments/environment.ts');
  const envContent = fs.readFileSync(envPath, 'utf8');
  
  // Extract apiBaseUrl from environment file
  const apiBaseUrlMatch = envContent.match(/apiBaseUrl:\s*['"`]([^'"`]+)['"`]/);
  const apiUrlMatch = envContent.match(/apiUrl:\s*['"`]([^'"`]+)['"`]/);
  
  return {
    apiBaseUrl: apiBaseUrlMatch ? apiBaseUrlMatch[1] : 'http://localhost:5001',
    apiUrl: apiUrlMatch ? apiUrlMatch[1] : 'http://localhost:5001/api'
  };
}

const config = getEnvironmentConfig();
const API_URL = config.apiBaseUrl;
const API_HEALTH_ENDPOINT = '/api/health';
const MAX_RETRIES = 10;
const RETRY_DELAY = 2000; // 2 seconds

// Colors for console output
const colors = {
  green: '\x1b[32m',
  yellow: '\x1b[33m',
  red: '\x1b[31m',
  blue: '\x1b[34m',
  reset: '\x1b[0m'
};

function log(message, color = 'reset') {
  console.log(`${colors[color]}${message}${colors.reset}`);
}

function checkApiHealth() {
  return new Promise((resolve) => {
    const req = http.get(`${API_URL}${API_HEALTH_ENDPOINT}`, (res) => {
      if (res.statusCode === 200) {
        resolve(true);
      } else {
        resolve(false);
      }
    });

    req.on('error', () => {
      resolve(false);
    });

    req.setTimeout(5000, () => {
      req.destroy();
      resolve(false);
    });
  });
}

async function waitForApi() {
  log('�� Checking if API is running...', 'blue');
  
  for (let i = 0; i < MAX_RETRIES; i++) {
    const isHealthy = await checkApiHealth();
    
    if (isHealthy) {
      log('✅ API is running and healthy!', 'green');
      return true;
    }
    
    if (i < MAX_RETRIES - 1) {
      log(`⏳ API not ready (attempt ${i + 1}/${MAX_RETRIES}), retrying in ${RETRY_DELAY/1000}s...`, 'yellow');
      await new Promise(resolve => setTimeout(resolve, RETRY_DELAY));
    }
  }
  
  log('❌ API is not running or not responding', 'red');
  log(`Please start your .NET API server at ${API_URL}`, 'yellow');
  return false;
}

function generateApiClient() {
  log('🔄 Generating API client...', 'blue');
  
  try {
    // TODO: Remove this with script generation from package.json
    execSync('yarn generate-api', { 
      stdio: 'inherit',
      cwd: process.cwd()
    });
    log('✅ API client generated successfully!', 'green');
    return true;
  } catch (error) {
    log('❌ Failed to generate API client', 'red');
    return false;
  }
}

function startDevServer() {
  log('🚀 Starting Angular development server...', 'blue');
  
  const child = spawn('yarn', ['start'], {
    stdio: 'inherit',
    cwd: process.cwd(),
    shell: true
  });

  child.on('error', (error) => {
    log(`❌ Failed to start development server: ${error.message}`, 'red');
    process.exit(1);
  });

  child.on('close', (code) => {
    if (code !== 0) {
      log(`❌ Development server exited with code ${code}`, 'red');
      process.exit(code);
    }
  });

  // Handle process termination
  process.on('SIGINT', () => {
    log('\n🛑 Shutting down development server...', 'yellow');
    child.kill('SIGINT');
    process.exit(0);
  });

  process.on('SIGTERM', () => {
    log('\n🛑 Shutting down development server...', 'yellow');
    child.kill('SIGTERM');
    process.exit(0);
  });
}

async function main() {
  log('🎯 Starting development environment...', 'blue');
  
  // Check if API is running
  const apiRunning = await waitForApi();
  
  if (!apiRunning) {
    log('💡 You can still start the dev server without API by running: yarn start', 'yellow');
    process.exit(1);
  }
  
  // Generate API client
  const clientGenerated = generateApiClient();
  
  if (!clientGenerated) {
    log('⚠️  Continuing without API client generation...', 'yellow');
  }
  
  // Start development server
  startDevServer();
}

// Run the script
main().catch((error) => {
  log(`❌ Unexpected error: ${error.message}`, 'red');
  process.exit(1);
});