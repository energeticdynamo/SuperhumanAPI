FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY SuperhumanAPI/SuperhumanAPI.csproj SuperhumanAPI/
RUN dotnet restore SuperhumanAPI/SuperhumanAPI.csproj

# Copy the rest of the app's source code
COPY SuperhumanAPI/ SuperhumanAPI/

# Publish the application
WORKDIR /src/SuperhumanAPI
RUN dotnet publish -c Release -o /app/publish

# Build the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SuperhumanAPI.dll"]