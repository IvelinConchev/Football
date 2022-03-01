#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Football/Football.csproj", "Football/"]
RUN dotnet restore "Football/Football.csproj"
COPY . .
WORKDIR "/src/Football"
RUN dotnet build "Football.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Football.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Football.dll"]