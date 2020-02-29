FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Nuget.Config .
COPY Micro.DDD.APIGateway/*.csproj ./Micro.DDD.APIGateway/
COPY Micro.DDD.Common/*.csproj ./Micro.DDD.Common/
COPY Micro.DDD.Crawler/*.csproj ./Micro.DDD.Crawler/
COPY Micro.DDD.CrawlerManagerService/*.csproj ./Micro.DDD.CrawlerManagerService/
COPY Micro.DDD.ETLWorker/*.csproj ./Micro.DDD.ETLWorker/
COPY Micro.DDD.MQMessages/*.csproj ./Micro.DDD.MQMessages/
COPY Micro.DDD.ReportingService/*.csproj ./Micro.DDD.ReportingService/
COPY Micro.DDD.MessagingService/*.csproj ./Micro.DDD.MessagingService/
RUN dotnet restore --configfile Nuget.Config

# copy everything else and build app
COPY Micro.DDD.ReportingService/. ./Micro.DDD.ReportingService/
COPY Micro.DDD.Common/. ./Micro.DDD.Common/
WORKDIR /app/Micro.DDD.ReportingService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/Micro.DDD.ReportingService/out ./
ENTRYPOINT ["dotnet", "Micro.DDD.ReportingService.dll"]