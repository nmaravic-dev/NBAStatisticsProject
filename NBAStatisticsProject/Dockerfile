# ---- Stage 1: BUILD ----
# Use the full SDK image — it has the tools needed to compile the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only the csproj first and restore — Docker caches this layer,
# so packages aren't re-downloaded when only source code changes
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the source and publish a release build
COPY . ./
RUN dotnet publish -c Release -o /app

# ---- Stage 2: RUNTIME ----
# Use the smaller runtime image — no build tools, just enough to run the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy only the published output from the build stage — keeps the final image small
COPY --from=build /app ./

# Fly.io expects the app to listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "NBAStatisticsProject.dll"]