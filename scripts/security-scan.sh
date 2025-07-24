#!/bin/bash

# Security and Compliance Scanning Script for SubscriptionAnalytics
# This script runs comprehensive security checks for the entire codebase

set -e

echo "🔒 Starting Security & Compliance Scan for SubscriptionAnalytics"
echo "================================================================"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Create reports directory
REPORTS_DIR="security-reports"
mkdir -p $REPORTS_DIR

print_status "Created reports directory: $REPORTS_DIR"

# 1. Snyk Security Scan (Dependencies)
print_status "Running Snyk security scan for dependencies..."
if command -v snyk &> /dev/null; then
    cd src/SubscriptionAnalytics.Web
    snyk test --json > ../../$REPORTS_DIR/snyk-dependencies.json 2>/dev/null || true
    snyk test --sarif > ../../$REPORTS_DIR/snyk-dependencies.sarif 2>/dev/null || true
    cd ../..
    print_success "Snyk dependency scan completed"
else
    print_warning "Snyk not found, skipping dependency scan"
fi

# 2. Semgrep Static Analysis
print_status "Running Semgrep static analysis..."
if command -v semgrep &> /dev/null; then
    semgrep scan --config .semgrep.yml --json > $REPORTS_DIR/semgrep-results.json 2>/dev/null || true
    semgrep scan --config .semgrep.yml --sarif > $REPORTS_DIR/semgrep-results.sarif 2>/dev/null || true
    print_success "Semgrep static analysis completed"
else
    print_warning "Semgrep not found, skipping static analysis"
fi

# 3. .NET Security Scan
print_status "Running .NET security scan..."
if command -v security-scan &> /dev/null; then
    cd src/SubscriptionAnalytics.Api
    security-scan --output json > ../../$REPORTS_DIR/dotnet-security.json 2>/dev/null || true
    cd ../..
    print_success ".NET security scan completed"
else
    print_warning "security-scan tool not found, skipping .NET security scan"
fi

# 4. NPM Audit
print_status "Running NPM audit..."
cd src/SubscriptionAnalytics.Web
if [ -f "package.json" ]; then
    npm audit --json > ../../$REPORTS_DIR/npm-audit.json 2>/dev/null || true
    print_success "NPM audit completed"
else
    print_warning "No package.json found, skipping NPM audit"
fi
cd ../..

# 5. GDPR Compliance Check
print_status "Running GDPR compliance check..."
grep -r -i "email\|phone\|ssn\|credit.*card\|password" src/ --include="*.cs" --include="*.ts" --include="*.js" > $REPORTS_DIR/gdpr-pii-detection.txt 2>/dev/null || true
print_success "GDPR compliance check completed"

# 6. SOC2 Compliance Check
print_status "Running SOC2 compliance check..."
grep -r -i "console\.log\|console\.write\|debug.*log" src/ --include="*.cs" --include="*.ts" --include="*.js" > $REPORTS_DIR/soc2-logging-detection.txt 2>/dev/null || true
print_success "SOC2 compliance check completed"

# 7. Multi-tenant Security Check
print_status "Running multi-tenant security check..."
grep -r -i "tenant.*id\|tenant_id" src/ --include="*.cs" --include="*.ts" --include="*.js" > $REPORTS_DIR/tenant-isolation-check.txt 2>/dev/null || true
print_success "Multi-tenant security check completed"

# 8. Generate Summary Report
print_status "Generating summary report..."
cat > $REPORTS_DIR/security-summary.md << EOF
# Security & Compliance Scan Summary
Generated: $(date)

## Scan Results

### 1. Dependency Vulnerabilities (Snyk)
- Report: snyk-dependencies.json
- SARIF: snyk-dependencies.sarif

### 2. Static Analysis (Semgrep)
- Report: semgrep-results.json
- SARIF: semgrep-results.sarif

### 3. .NET Security
- Report: dotnet-security.json

### 4. NPM Audit
- Report: npm-audit.json

### 5. GDPR Compliance
- PII Detection: gdpr-pii-detection.txt

### 6. SOC2 Compliance
- Logging Detection: soc2-logging-detection.txt

### 7. Multi-tenant Security
- Tenant Isolation: tenant-isolation-check.txt

## Next Steps
1. Review all generated reports
2. Address high and critical vulnerabilities
3. Implement security fixes
4. Re-run scans after fixes
5. Consider integrating scans into CI/CD pipeline

## Compliance Frameworks
- GDPR: Data privacy and protection
- SOC2: Security, availability, processing integrity
- OWASP Top 10: Web application security
EOF

print_success "Summary report generated: $REPORTS_DIR/security-summary.md"

# 9. Display quick summary
echo ""
echo "📊 Scan Summary:"
echo "================"
echo "Reports generated in: $REPORTS_DIR/"
echo ""
echo "Files created:"
ls -la $REPORTS_DIR/
echo ""
print_success "Security scan completed successfully!"
echo ""
echo "🔍 To view detailed results:"
echo "   - Check $REPORTS_DIR/security-summary.md for overview"
echo "   - Review individual JSON/SARIF files for detailed findings"
echo "   - Address any high/critical vulnerabilities found"
echo ""
echo "🔄 To re-run scans:"
echo "   ./scripts/security-scan.sh" 