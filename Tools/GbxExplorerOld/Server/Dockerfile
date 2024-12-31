FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH=x64
RUN apt-get update && apt-get install -y git python3
WORKDIR /src

COPY .git/ ./.git/

RUN dotnet workload install wasm-tools

# copy csproj and restore as distinct layers
COPY Tools/GbxExplorerOld/Server/*.csproj Tools/GbxExplorerOld/Server/
COPY Tools/GbxExplorerOld/Client/*.csproj Tools/GbxExplorerOld/Client/
COPY Src/GBX.NET/*.csproj Src/GBX.NET/
COPY Src/GBX.NET.LZO/*.csproj Src/GBX.NET.LZO/
COPY Src/GBX.NET.Hashing/*.csproj Src/GBX.NET.Hashing/
COPY Src/GBX.NET.ZLib/*.csproj Src/GBX.NET.ZLib/
COPY Generators/GBX.NET.Generators/*.csproj Generators/GBX.NET.Generators/
RUN dotnet restore Tools/GbxExplorerOld/Server/GbxExplorerOld.Server.csproj -a $TARGETARCH

# copy and publish app and libraries
COPY Tools/GbxExplorerOld/Server/ Tools/GbxExplorerOld/Server/
COPY Tools/GbxExplorerOld/Client/ Tools/GbxExplorerOld/Client/
COPY Src/GBX.NET/ Src/GBX.NET/
COPY Src/GBX.NET.LZO/ Src/GBX.NET.LZO/
COPY Src/GBX.NET.Hashing/ Src/GBX.NET.Hashing/
COPY Src/GBX.NET.ZLib/ Src/GBX.NET.ZLib/
COPY Generators/GBX.NET.Generators/ Generators/GBX.NET.Generators/
COPY Resources/ Resources/
RUN dotnet publish Tools/GbxExplorerOld/Server -c $BUILD_CONFIGURATION -a $TARGETARCH -o /app --no-restore


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
EXPOSE 8080
EXPOSE 8081
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["./GbxExplorerOld.Server"]