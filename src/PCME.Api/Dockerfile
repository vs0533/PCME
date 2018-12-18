FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/PCME.Api/PCME.Api.csproj src/PCME.Api/
COPY PCME.KSDB/PCME.KSDB.csproj PCME.KSDB/
COPY PCME.MOPDB/PCME.MOPDB.csproj PCME.MOPDB/
COPY src/PCME.Infrastructure/PCME.Infrastructure.csproj src/PCME.Infrastructure/
COPY src/PCME.Domain/PCME.Domain.csproj src/PCME.Domain/
COPY src/PCME.WebHost/PCME.CustomWebHost.csproj src/PCME.WebHost/
RUN dotnet restore src/PCME.Api/PCME.Api.csproj
COPY . .
WORKDIR /src/src/PCME.Api
RUN dotnet build PCME.Api.csproj -c Release -o /app
RUN rm -rf wwwroot
FROM build AS publish
RUN dotnet publish PCME.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
CMD rm -rf wwwroot
ENTRYPOINT ["dotnet", "PCME.Api.dll"]
