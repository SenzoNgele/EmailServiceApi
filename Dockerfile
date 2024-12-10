#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

# Use Linux-based .NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use Linux-based .NET SDK image for the build and publish stages
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the .csproj and restore any dependencies (via NuGet)
COPY ["EmailServiceApi/EmailServiceApi.csproj", "EmailServiceApi/"]
RUN dotnet restore "EmailServiceApi/EmailServiceApi.csproj"

# Copy the rest of the application code
COPY . ./

# Set the working directory to the project folder
WORKDIR "/src/EmailServiceApi"

# Build the application
RUN dotnet build "EmailServiceApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "EmailServiceApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image with the runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "EmailServiceApi.dll"]
