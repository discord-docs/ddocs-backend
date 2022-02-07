FROM mcr.microsoft.com/dotnet/sdk:6.0 AS builder

WORKDIR /build

# fetch dependencies first, for caching
COPY DDocsBackend.sln /build
COPY DDocsBackend/DDocsBackend.csproj /build/DDocsBackend/
COPY DDocsBackend.Database/DDocsBackend.Data.csproj /build/DDocsBackend.Database/
COPY DDocsBackend.Common/DDocsBackend.Common.csproj /build/DDocsBackend.Common/
RUN dotnet restore DDocsBackend.sln

COPY . /build

RUN dotnet build -c release -o bin

FROM mcr.microsoft.com/dotnet/runtime:6.0

WORKDIR /app
COPY --from=builder /build/bin/ /app/

ENTRYPOINT ["dotnet"]
CMD ["/app/DDocsBackend.dll"]