FROM mcr.microsoft.com/dotnet/sdk:latest AS build-env
WORKDIR /app
EXPOSE 80
EXPOSE 443
# Copy the csproj and restore all of the nugets
COPY . ./
RUN dotnet restore ./AnalysisData/AnalysisData.sln
# Copy everything else and build
#COPY . ./
RUN dotnet publish -c Release -o out ./AnalysisData/AnalysisData.sln
# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:latest
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "AnalysisData.dll"]
