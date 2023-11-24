FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["Tools/GbxExplorer/Server/GbxExplorer.Server.csproj", "Tools/GbxExplorer/Server/"]
COPY ["Tools/GbxExplorer/Client/GbxExplorer.Client.csproj", "Tools/GbxExplorer/Client/"]
COPY ["Src/GBX.NET/GBX.NET.csproj", "Src/GBX.NET/"]
COPY ["Generators/GBX.NET.Generators/GBX.NET.Generators.csproj", "Generators/GBX.NET.Generators/"]
COPY ["Tools/GbxExplorer/Shared/GbxExplorer.Shared.csproj", "Tools/GbxExplorer/Shared/"]
RUN dotnet restore "Tools/GbxExplorer/Server/GbxExplorer.Server.csproj"
COPY . .
WORKDIR "/src/Tools/GbxExplorer/Server"
RUN dotnet build "GbxExplorer.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GbxExplorer.Server.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-alpine
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./GbxExplorer.Server"]
