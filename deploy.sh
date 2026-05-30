#!/bin/bash
set -e

MOD_NAME="PatternMining"
MODS_PATH="$HOME/.config/VintagestoryData/Mods"
STAGING_DIR="bin/staging"
BUILD_OUTPUT="bin/Debug/Mods/PatternMining"

echo "--- 1. CLEANING ---"
dotnet clean -v q
rm -rf "$STAGING_DIR"
rm -f "$MOD_NAME.zip"

echo "--- 2. BUILDING ---"
dotnet build -c Debug
if [ ! -f "$BUILD_OUTPUT/$MOD_NAME.dll" ]; then
    echo "ERROR: Build failed — $MOD_NAME.dll not found."
    exit 1
fi

echo "--- 3. PACKAGING ---"
mkdir -p "$STAGING_DIR"
cp modinfo.json "$STAGING_DIR/"
cp modicon.png "$STAGING_DIR/"
cp "$BUILD_OUTPUT/$MOD_NAME.dll" "$STAGING_DIR/"

(cd "$STAGING_DIR" && zip -r -q "../../$MOD_NAME.zip" *)

echo "--- 4. DEPLOYING ---"
rm -f "$MODS_PATH/$MOD_NAME.zip"
mv "$MOD_NAME.zip" "$MODS_PATH/"

echo "Deploy Complete: $MODS_PATH/$MOD_NAME.zip"
