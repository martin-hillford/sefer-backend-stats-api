#!/bin/bash
dotnet restore
dotnet build --no-restore 
dotnet publish Sefer.Backend.Stats.Api/Sefer.Backend.Stats.Api.csproj --output ./build