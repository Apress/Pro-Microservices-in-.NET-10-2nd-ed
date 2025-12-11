# build "server" image
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

WORKDIR /src

COPY MyProject.csproj .
RUN dotnet restore
COPY . .
RUN dotnet build -c Release
RUN dotnet test -c Release
RUN dotnet publish -c Release -o /dist MyProject.csproj

# production runtime "server" image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Production

WORKDIR /app
COPY --from=build /dist .
CMD ["dotnet", "MyProject.dll"]
