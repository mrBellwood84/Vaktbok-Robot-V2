#!/usr/bin/env bash

set -e

echo "=========================================="
echo "   Vaktbok Robot V2 – Playwright Setup"
echo "=========================================="
echo ""

# --- 1) Check for .NET SDK ---
echo "[1/3] Checking for .NET SDK..."

if ! command -v dotnet &> /dev/null
then
    echo "❌ .NET SDK is not installed."
    echo "   Please install from: https://dotnet.microsoft.com/en-us/download"
    exit 1
fi

echo "✔ .NET SDK found: $(dotnet --version)"
echo ""

# --- 2) Check Playwright CLI (global tool) ---
echo "[2/3] Checking for Playwright CLI..."

if ! dotnet tool list -g | grep -q "playwright-cli"
then
    echo "Installing Playwright CLI..."
    dotnet tool install -g Microsoft.Playwright.CLI
else
    echo "✔ Playwright CLI already installed"
fi

echo ""

# --- 3) Install browsers ---
echo "[3/3] Installing Playwright browsers..."

# Navigate to project folder (script location)
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

# Run Playwright install
playwright install

echo ""
echo "=========================================="
echo "   Playwright successfully installed!"
echo "=========================================="
