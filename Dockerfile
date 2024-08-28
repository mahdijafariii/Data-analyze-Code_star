# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /app

# COPY ./AnalysisData/ ./
# RUN dotnet restore
# COPY . .

# RUN dotnet publish ./AnalysisData/AnalysisData/AnalysisData.csproj -c Release -o out

# #FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime
# #WORKDIR /app
# #COPY --from=build /app/out ./
# RUN dotnet tool install --global dotnet-ef --version 8.0
# ENV PATH="${PATH}:/root/.dotnet/tools"

# ENV ASPNETCORE_ENVIRONMENT=Development

# ENV ASPNETCORE_URLS=http://*:80
# EXPOSE 80

# #COPY . ./app

# ENTRYPOINT ["dotnet", "./out/AnalysisData.dll"]

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

COPY --from=build /app/out /app/migrateout ./

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://*:80

EXPOSE 80

#COPY . ./app

ENTRYPOINT ["dotnet", "AnalysisData.dll"]

