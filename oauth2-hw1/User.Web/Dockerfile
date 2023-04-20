FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY ./ ./
RUN dotnet restore "User.Web.csproj" -s https://www.nuget.org/api/v2/
RUN dotnet publish "User.Web.csproj" -c Release -o published

FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app
COPY --from=build /app/published/ ./

# ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "User.Web.dll"]
