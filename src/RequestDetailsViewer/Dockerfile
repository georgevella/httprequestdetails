FROM microsoft/dotnet:2.2.103-sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish --runtime alpine-x64 --configuration Release -o out

# Build runtime image
# FROM microsoft/dotnet:2.2.1-aspnetcore-runtime-alpine3.8
FROM microsoft/dotnet:2.2.1-runtime-deps-alpine3.8
WORKDIR /app
COPY --from=build-env /app/out .
COPY appsettings.json .
# ENTRYPOINT ["dotnet", "RequestDetailsViewer.dll"]
ENTRYPOINT ["/app/RequestDetailsViewer"]