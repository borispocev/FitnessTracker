FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only csproj first (better caching)
COPY src/FitnessTracker.Api/FitnessTracker.Api.csproj src/FitnessTracker.Api/
COPY src/FitnessTracker.Application/FitnessTracker.Application.csproj src/FitnessTracker.Application/
COPY src/FitnessTracker.Domain/FitnessTracker.Domain.csproj src/FitnessTracker.Domain/
COPY src/FitnessTracker.Infrastructure/FitnessTracker.Infrastructure.csproj src/FitnessTracker.Infrastructure/

RUN dotnet restore src/FitnessTracker.Api/FitnessTracker.Api.csproj

# Copy the rest of the source
COPY src/ src/

RUN dotnet publish src/FitnessTracker.Api/FitnessTracker.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FitnessTracker.Api.dll"]
