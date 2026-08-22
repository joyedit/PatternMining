#!/bin/bash
set -e

MOD_NAME="PatternMining"
MODS_PATH="$HOME/.config/VintagestoryData/Mods"
STAGING_DIR="bin/staging"
BUILD_OUTPUT="bin/Debug/Mods/PatternMining"

# ModDB requires the version in the filename, so derive it from modinfo.json
# rather than hardcoding it. Old versioned zips are removed on deploy so the
# game never loads two copies of the mod side by side.
VERSION=$(grep -oP '"version":\s*"\K[^"]+' modinfo.json)
ZIP_NAME="${MOD_NAME}-${VERSION}.zip"

echo "--- 1. CLEANING ---"
dotnet clean -v q
rm -rf "$STAGING_DIR"
rm -f "$MOD_NAME"*.zip

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

(cd "$STAGING_DIR" && zip -r -q "../../$ZIP_NAME" *)

echo "--- 4. DEPLOYING ---"
rm -f "$MODS_PATH/$MOD_NAME"*.zip
mv "$ZIP_NAME" "$MODS_PATH/"

echo "Deploy Complete: $MODS_PATH/$ZIP_NAME"
