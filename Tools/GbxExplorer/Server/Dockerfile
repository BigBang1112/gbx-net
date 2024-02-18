#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tools/GbxExplorer/Server/GbxExplorer.Server.csproj", "Tools/GbxExplorer/Server/"]
COPY ["Tools/GbxExplorer/Client/GbxExplorer.Client.csproj", "Tools/GbxExplorer/Client/"]
COPY ["Tools/GbxExplorer/Component/GbxExplorer.Component.csproj", "Tools/GbxExplorer/Component/"]
COPY ["Src/GBX.NET.Lua/GBX.NET.Lua.csproj", "Src/GBX.NET.Lua/"]
COPY ["Src/GBX.NET/GBX.NET.csproj", "Src/GBX.NET/"]
COPY ["Generators/GBX.NET.Generators/GBX.NET.Generators.csproj", "Generators/GBX.NET.Generators/"]
COPY ["Tools/GbxExplorer/Shared/GbxExplorer.Shared.csproj", "Tools/GbxExplorer/Shared/"]
RUN dotnet restore "./Tools/GbxExplorer/Server/GbxExplorer.Server.csproj"
COPY . .
WORKDIR "/src/Tools/GbxExplorer/Server"
RUN dotnet build "./GbxExplorer.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GbxExplorer.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GbxExplorer.Server.dll"]


FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH=x64
WORKDIR /src

# copy csproj and restore as distinct layers
COPY Tools/GbxExplorer/Server/*.csproj Tools/GbxExplorer/Server/
COPY Tools/GbxExplorer/Client/*.csproj Tools/GbxExplorer/Client/
COPY Tools/GbxExplorer/Component/*.csproj Tools/GbxExplorer/Component/
COPY Tools/GbxExplorer/Shared/*.csproj Tools/GbxExplorer/Shared/
COPY Src/GBX.NET/*.csproj Src/GBX.NET/
COPY Src/GBX.NET.LZO/*.csproj Src/GBX.NET.LZO/
COPY Src/GBX.NET.Lua/*.csproj Src/GBX.NET.Lua/
COPY Generators/GBX.NET.Generators/*.csproj Generators/GBX.NET.Generators/
RUN dotnet restore Tools/$APPNAME/$APPNAME.csproj -a $TARGETARCH

# copy and publish app and libraries
COPY Tools/GbxExplorer/Server/ Tools/GbxExplorer/Server/
COPY Tools/GbxExplorer/Client/ Tools/GbxExplorer/Client/
COPY Tools/GbxExplorer/Component/ Tools/GbxExplorer/Component/
COPY Tools/GbxExplorer/Shared/ Tools/GbxExplorer/Shared/
COPY Src/GBX.NET/ Src/GBX.NET/
COPY Src/GBX.NET.LZO/ Src/GBX.NET.LZO/
COPY Src/GBX.NET.Lua/ Src/GBX.NET.Lua/
COPY Generators/GBX.NET.Generators/ Generators/GBX.NET.Generators/
COPY Resources/ Resources/
WORKDIR /src/Tools/GbxExplorer/Server
RUN dotnet publish -c $BUILD_CONFIGURATION -a $TARGETARCH -o /app --no-restore --self-contained


# final stage/image
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-alpine
EXPOSE 8080
EXPOSE 8081
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["./GbxExplorer.Server"]