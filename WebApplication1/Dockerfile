
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY WebApplication1/*.csproj ./
#COPY *.csproj ./
RUN dotnet restore

COPY WebApplication1/ ./
#COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=build app/out .
ENTRYPOINT ["dotnet", "WebApplication1.dll"]