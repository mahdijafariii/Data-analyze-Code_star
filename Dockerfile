FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ./AnalysisData/ ./
RUN dotnet restore
COPY . .

RUN dotnet publish ./AnalysisData/AnalysisData/AnalysisData.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENV ASPNETCORE_ENVIRONMENT=Development
#ENV ConnectionStrings__DefaultConnection=Host=postgres;Database=mohaymen;Username=postgres;Password=1234;
ENV ASPNETCORE_URLS=http://*:80
EXPOSE 80

COPY . ./app

#RUN dotnet tool install --global dotnet-ef
#RUN export PATH="${PATH}:/root/.dotnet/tools"
#RUN dotnet ef database update --project AnalysisData/AnalysisData/AnalysisData.csproj

# Start the application
ENTRYPOINT ["dotnet", "AnalysisData.dll"]
