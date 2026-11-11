FROM mcr.microsoft.com/dotnet/sdk:10.0-noble-amd64@sha256:69ff714a42f7931475acfcd2792d69cd4a656e4f3653d520e25c0fbe3c6d0cba AS build
WORKDIR /source

COPY . ./
RUN dotnet tool restore
RUN dotnet restore
RUN dotnet publish --no-restore --configuration Release
RUN dotnet ef migrations bundle -p src/SDApp.Web/ --configuration Release --output efbundle

FROM mcr.microsoft.com/dotnet/aspnet:10.0.12-noble-chiseled-extra-amd64@sha256:69c87e30c013eac2024da453545dfc13f1476347726da4e1a534d66789a7564f
ARG GIT_SHA
WORKDIR /app

COPY --from=build /source/src/SDApp.Web/bin/Release/net10.0/publish/ .
COPY --from=build /source/efbundle ./efbundle

ENV SENTRY_RELEASE=${GIT_SHA}
ENV GIT_SHA=${GIT_SHA}
ENV ASPNETCORE_HTTP_PORTS=3000

USER $APP_UID

ENTRYPOINT ["dotnet", "SDApp.Web.dll"]