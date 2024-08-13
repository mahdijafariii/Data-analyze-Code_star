# Build
FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /app
COPY . .
RUN dotnet restore ./AnalysisData/AnalysisData.sln

RUN dotnet publish -c Release -o out ./AnalysisData/AnalysisData.sln

# Run
FROM mcr.microsoft.com/dotnet/aspnet:latest
WORKDIR /app
COPY --from=build /app/out .
#ENV ASPNETCORE_URLS=http://*:8080
CMD dotnet AnalysisData.dll
# CMD /bin/bash

