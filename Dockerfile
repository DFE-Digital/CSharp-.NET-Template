FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine3.23@sha256:732cd42c6f659814c9804ad7b05c7f761e83ef8379c5b2fdc3af673353caff73 AS build
WORKDIR /source

COPY . ./
RUN dotnet tool restore
RUN dotnet restore
RUN dotnet publish --no-restore --configuration Release
RUN dotnet ef migrations bundle -p src/SDApp.Web/ --configuration Release --output efbundle

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine3.23@sha256:1201dde897ab436b7c6b386f6dbd4f9a3ca0245f9c5a8aac8f8bcdccb4c7d484
ARG GIT_SHA
WORKDIR /app

COPY --from=build /source/src/SDApp.Web/bin/Release/net10.0/publish/ .
COPY --from=build /source/efbundle ./efbundle

# Ensure culture data is available
RUN apk add --no-cache tzdata icu-data-full icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ENV SENTRY_RELEASE=${GIT_SHA}
ENV GIT_SHA=${GIT_SHA}
ENV ASPNETCORE_HTTP_PORTS=3000

USER app

ENTRYPOINT ["dotnet", "SDApp.Web.dll"]