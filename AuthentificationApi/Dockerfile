# =========================
# Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./AuthentificationApi/AuthentificationApi.csproj ./AuthentificationApi/
COPY ./ComputerManagement.Shared/ComputerManagement.Shared.csproj ./ComputerManagement.Shared/
RUN dotnet restore ./AuthentificationApi
RUN dotnet restore ./ComputerManagement.Shared

# Copy the remaining source code
COPY ./AuthentificationApi/ ./AuthentificationApi/
COPY ./ComputerManagement.Shared/ ./ComputerManagement.Shared/
RUN dotnet publish ./AuthentificationApi -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5150

# Expose port 5150
EXPOSE 5150

# Define the entry point
ENTRYPOINT ["dotnet", "AuthentificationApi.dll"]
