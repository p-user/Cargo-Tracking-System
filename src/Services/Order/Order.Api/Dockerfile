# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Order/Order.Api/Order.Api.csproj", "Services/Order/Order.Api/"]
COPY ["SharedKernel/SharedKernel.Logging/SharedKernel.Logging.csproj", "SharedKernel/SharedKernel.Logging/"]
COPY ["SharedKernel/SharedKernel.Messaging/SharedKernel.Messaging.csproj", "SharedKernel/SharedKernel.Messaging/"]
COPY ["SharedKernel/SharedKernel.OpenApi/SharedKernel.OpenApi.csproj", "SharedKernel/SharedKernel.OpenApi/"]
COPY ["SharedKernel/SharedKernel.Core/SharedKernel.Core.csproj", "SharedKernel/SharedKernel.Core/"]
RUN dotnet restore "./Services/Order/Order.Api/Order.Api.csproj"
COPY . .
WORKDIR "/src/Services/Order/Order.Api"
RUN dotnet build "./Order.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Order.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.Api.dll"]