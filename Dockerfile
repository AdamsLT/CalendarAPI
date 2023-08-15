FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base

WORKDIR /app

EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build

COPY . .

RUN dotnet restore "/src/Calendarize.API/Calendarize.API.csproj"
RUN dotnet build "/src/Calendarize.API/Calendarize.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/src/Calendarize.API/Calendarize.API.csproj" -c Release -o /app/publish

FROM base AS final
	
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Calendarize.API.dll"]
