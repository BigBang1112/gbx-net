FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH=x64
ARG APPNAME=GbxDiscordBot
WORKDIR /src

# copy csproj and restore as distinct layers
COPY Tools/$APPNAME/*.csproj Tools/$APPNAME/
COPY Src/GBX.NET/*.csproj Src/GBX.NET/
COPY Src/GBX.NET.LZO/*.csproj Src/GBX.NET.LZO/
COPY Src/GBX.NET.Hashing/*.csproj Src/GBX.NET.Hashing/
COPY Generators/GBX.NET.Generators/*.csproj Generators/GBX.NET.Generators/
RUN dotnet restore Tools/$APPNAME/$APPNAME.csproj -a $TARGETARCH

# copy and publish app and libraries
COPY Tools/$APPNAME/ Tools/$APPNAME/
COPY Src/GBX.NET/ Src/GBX.NET/
COPY Src/GBX.NET.LZO/ Src/GBX.NET.LZO/
COPY Src/GBX.NET.Hashing/ Src/GBX.NET.Hashing/
COPY Generators/GBX.NET.Generators/ Generators/GBX.NET.Generators/
COPY Resources/ Resources/
WORKDIR /src/Tools/$APPNAME
RUN dotnet publish -c $BUILD_CONFIGURATION -a $TARGETARCH -o /app --no-restore --self-contained


# final stage/image
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-alpine
ENV \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8
RUN apk add --no-cache \
    icu-data-full \
    icu-libs \
    tzdata
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./GbxDiscordBot"]