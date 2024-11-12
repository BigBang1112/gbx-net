FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
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
COPY Src/GBX.NET.Hashing/*.csproj Src/GBX.NET.Hashing/
COPY Src/GBX.NET.ZLib/*.csproj Src/GBX.NET.ZLib/
COPY Src/GBX.NET.Lua/*.csproj Src/GBX.NET.Lua/
COPY Generators/GBX.NET.Generators/*.csproj Generators/GBX.NET.Generators/
RUN dotnet restore Tools/GbxExplorer/Server/GbxExplorer.Server.csproj -a $TARGETARCH

# copy and publish app and libraries
COPY Tools/GbxExplorer/Server/ Tools/GbxExplorer/Server/
COPY Tools/GbxExplorer/Client/ Tools/GbxExplorer/Client/
COPY Tools/GbxExplorer/Component/ Tools/GbxExplorer/Component/
COPY Tools/GbxExplorer/Shared/ Tools/GbxExplorer/Shared/
COPY Src/GBX.NET/ Src/GBX.NET/
COPY Src/GBX.NET.LZO/ Src/GBX.NET.LZO/
COPY Src/GBX.NET.Hashing/ Src/GBX.NET.Hashing/
COPY Src/GBX.NET.ZLib/ Src/GBX.NET.ZLib/
COPY Src/GBX.NET.Lua/ Src/GBX.NET.Lua/
COPY Generators/GBX.NET.Generators/ Generators/GBX.NET.Generators/
COPY Resources/ Resources/
WORKDIR /src/Tools/GbxExplorer/Server
RUN dotnet publish -c $BUILD_CONFIGURATION -a $TARGETARCH -o /app --no-restore --self-contained


# final stage/image
FROM mcr.microsoft.com/dotnet/runtime-deps:9.0-alpine
EXPOSE 8080
EXPOSE 8081
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["./GbxExplorer.Server"]