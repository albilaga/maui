#!/bin/bash

# Function to display help message
function show_help() {
    echo "Usage: ./build.sh --packageVersion=<version>"
}

# Check if arguments are passed
if [ $# -eq 0 ]; then
    show_help
    exit 1
fi

# Parse arguments
for arg in "$@"
do
    case $arg in
        --packageVersion=*)
        packageVersion="${arg#*=}"
        shift # Remove --packageVersion=<version> from processing
        ;;
        *)
        # Unknown option
        show_help
        exit 1
        ;;
    esac
done

# Check if packageVersion is set
if [ -z "$packageVersion" ]; then
    echo "Error: --packageVersion argument is required."
    show_help
    exit 1
fi

# Directory to store incremental build number
BUILD_INFO_DIR=".build_info"
BUILD_NUMBER_FILE="$BUILD_INFO_DIR/build_number"

# Create directory if it doesn't exist
mkdir -p "$BUILD_INFO_DIR"

# Get current date in UTC
current_date=$(date -u +"%Y%m%d")

# Read last build date and increment number
if [ -f "$BUILD_NUMBER_FILE" ]; then
    last_build_date=$(cut -d '.' -f1 "$BUILD_NUMBER_FILE")
    incremental_number=$(cut -d '.' -f2 "$BUILD_NUMBER_FILE")

    # If the date has changed, reset the incremental number
    if [ "$current_date" != "$last_build_date" ]; then
        incremental_number=1
    else
        incremental_number=$((incremental_number + 1))
    fi
else
    # First build of the day
    incremental_number=1
fi

# Save the new build number
echo "$current_date.$incremental_number" > "$BUILD_NUMBER_FILE"

# Compute the officialBuildId
officialBuildId="$current_date.$incremental_number"

# Run the dotnet cake command
dotnet cake --target=dotnet-pack --android --ios --configuration=Release --usenuget=false --workloads=system --packageVersion=$packageVersion --officialBuildId=$officialBuildId
