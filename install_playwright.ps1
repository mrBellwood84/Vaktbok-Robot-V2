# ===============================================
# Playwright Setup Script for Windows (PowerShell)
# ===============================================
# This script will:
# 1. Check if .NET is installed
# 2. Check if Playwright CLI is installed, and install it if needed
# 3. Install the required Playwright browsers (Chromium, Firefox, WebKit)
# -----------------------------------------------

# Stop the script if any command fails
$ErrorActionPreference = "Stop"

Write-Host "=== Playwright Setup ===" -ForegroundColor Cyan

# 1️⃣ Check for .NET installation
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Host "ERROR: .NET SDK not found. Please install .NET 6+ before running this script." -ForegroundColor Red
    exit 1
} else {
    Write-Host ".NET SDK detected." -ForegroundColor Green
}

# 2️⃣ Install Playwright CLI if not installed
if (-not (Get-Command playwright -ErrorAction SilentlyContinue)) {
    Write-Host "Playwright CLI not found. Installing..." -ForegroundColor Yellow
    dotnet tool install --global Microsoft.Playwright.CLI
    Write-Host "Playwright CLI installed successfully!" -ForegroundColor Green
} else {
    Write-Host "Playwright CLI is already installed." -ForegroundColor Green
}

# 3️⃣ Install Playwright browsers
Write-Host "Installing Playwright browsers (Chromium, Firefox, WebKit)..." -ForegroundColor Cyan
playwright install

Write-Host "=== Playwright setup completed successfully! ===" -ForegroundColor Cyan
