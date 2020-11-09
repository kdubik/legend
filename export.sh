#!/bin/bash

# This is script for exporting completed game (MacOS X version)
dotnet publish -p:PublishDir=.\publish --self-contained true -r osx-x64