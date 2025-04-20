# Use the official .NET SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory
WORKDIR /app

# Expose the port the application runs on
EXPOSE 8080
EXPOSE 8081

# Copy the solution and project files
COPY TaskManagement.sln ./
COPY API/API.csproj ./API/
COPY Application/Application.csproj ./Application/
COPY Domain/Domain.csproj ./Domain/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY IOC/IOC.csproj ./IOC/
COPY Test/Test.csproj ./Test/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the application files
COPY . .

# Build the application
RUN dotnet publish API/API.csproj -c Release -o /out

# Use the official .NET runtime image as the runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the build output from the build stage
COPY --from=build /out ./



# Set the entry point for the application
ENTRYPOINT ["dotnet", "API.dll"]