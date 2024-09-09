FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

RUN dotnet tool install --global dotnet-ef --version 8.0
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY ./AnalysisData/ ./
RUN dotnet restore
COPY . .

RUN dotnet publish ./AnalysisData/AnalysisData/AnalysisData.csproj -c Release -o out
RUN dotnet ef migrations bundle --project AnalysisData/AnalysisData/AnalysisData.csproj -o migrateout

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

COPY --from=build /app/out /app/migrateout /app/AnalysisData/AnalysisData/Assets/email-template.html ./

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://*:80

EXPOSE 80

#COPY . ./app

CMD ["dotnet", "AnalysisData.dll"]

