FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY JiraDemo/JiraDemo.csproj JiraDemo/
RUN dotnet restore JiraDemo/JiraDemo.csproj

COPY JiraDemo/ JiraDemo/

WORKDIR /src/JiraDemo
RUN dotnet build JiraDemo.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish JiraDemo.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080;
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "JiraDemo.dll"]
