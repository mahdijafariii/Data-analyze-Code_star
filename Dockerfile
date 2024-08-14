FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ./AnalysisData/ ./
RUN dotnet restore
COPY . .

RUN dotnet publish ./AnalysisData/AnalysisData/AnalysisData.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENV ASPNETCORE_ENVIRONMENT=Development
#ENV ConnectionStrings__DefaultConnection=Host=db;Database=YourDatabaseName;Username=yourusername;Password=yourpassword
ENV ASPNETCORE_URLS=http://*:80
EXPOSE 80


# Start the application
ENTRYPOINT ["dotnet", "AnalysisData.dll"]
