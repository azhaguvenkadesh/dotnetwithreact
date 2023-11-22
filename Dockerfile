# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# Set the working directory
WORKDIR /app

# Copy the backend (ASP.NET Core) project files
COPY ./*.csproj ./
RUN dotnet restore

# Copy the full backend source code
COPY . .

# Build the backend
RUN dotnet publish -c Release -o out

# Use the official Node.js image for building React app
FROM node:14 AS build-client

# Set the working directory for the frontend
WORKDIR /app/client

# Copy the frontend (React) project files
COPY ./ClientApp/package*.json ./
RUN npm install

# Copy the full frontend source code
COPY ./ClientApp .

# Build the frontend
RUN npm run build

# Use the official ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0

# Set the working directory
WORKDIR /app

# Copy the backend and frontend build artifacts
COPY --from=build /app/out .
COPY --from=build-client /app/client/build ./ClientApp/build

# Expose the port that your application will run on
EXPOSE 80

# Command to run the application
ENTRYPOINT ["dotnet", "YourWebApplication.dll"]
