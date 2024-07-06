FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

WORKDIR /app

# Copy source code and compile
COPY ./ ./
RUN dotnet tool restore

RUN dotnet publish --configuration Release -o bin

EXPOSE 8080

CMD ["bin/backend"]
