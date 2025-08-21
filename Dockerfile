# Use Windows Server Core with .NET Framework
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8-windowsservercore-ltsc2022 AS base
WORKDIR /app

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2022 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY *.csproj ./
COPY *.sln ./
RUN nuget restore

# Copy source files
COPY . .

# Build the application
WORKDIR /src
RUN msbuild hastaTakipSistemi.sln /p:Configuration=Release /p:Platform="Any CPU"

# Create the runtime image
FROM base AS final
WORKDIR /app

# Copy the built application
COPY --from=build /src/bin/Release/ .

# Set environment variables for database connection
ENV DB_CONNECTION_STRING="Data Source=sqlserver;Initial Catalog=HastaTakip;User Id=sa;Password=YourPassword123;Encrypt=False;"

# Note: This is a Windows Forms GUI application
# To run with GUI, you would need:
# - Windows containers with GUI support
# - Remote Desktop or similar display mechanism
# For headless operation, consider converting to console app or web service

ENTRYPOINT ["hastaTakipSistemi.exe"]