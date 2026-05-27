FROM mcr.microsoft.com/dotnet/sdk:10.0-noble-amd64@sha256:69ff714a42f7931475acfcd2792d69cd4a656e4f3653d520e25c0fbe3c6d0cba AS build
WORKDIR /source

COPY . ./
RUN dotnet restore
RUN dotnet publish --no-restore --configuration Release

FROM mcr.microsoft.com/dotnet/aspnet:10.0.8-noble-chiseled-extra-amd64@sha256:d3552fc1bd9b5195f6a397a547975fa1dbfb21870b4710f929eaa9adc5ceee42
ARG GIT_SHA
WORKDIR /app

COPY --from=build /source/src/SDApp.Web/bin/Release/net10.0/publish/ .

ENV SENTRY_RELEASE=${GIT_SHA}
ENV GIT_SHA=${GIT_SHA}
ENV ASPNETCORE_HTTP_PORTS=3000

USER $APP_UID

ENTRYPOINT ["dotnet", "SDApp.Web.dll"]