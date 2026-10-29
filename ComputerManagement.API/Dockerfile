# =========================
# Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./ComputerManagement.API/ComputerManagement.API.csproj ./ComputerManagement.API/
COPY ./ComputerManagement.Shared/ComputerManagement.Shared.csproj ./ComputerManagement.Shared/
RUN dotnet restore ./ComputerManagement.API
RUN dotnet restore ./ComputerManagement.Shared

# Copy the remaining source code
COPY ./ComputerManagement.API/ ./ComputerManagement.API/
COPY ./ComputerManagement.Shared/ ./ComputerManagement.Shared/
RUN dotnet publish ./ComputerManagement.API -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5176

# Expose port 5176
EXPOSE 5176

# Define the entry point
ENTRYPOINT ["dotnet", "ComputerManagement.API.dll"]
