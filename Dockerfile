FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
RUN apt update && apt install -y git wget
RUN wget https://github.com/trufflesecurity/trufflehog/releases/download/v3.88.11/trufflehog_3.88.11_linux_amd64.tar.gz && tar -xvf trufflehog_3.88.11_linux_amd64.tar.gz && mv trufflehog /usr/local/bin
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RepoLinter/RepoLinter.csproj", "RepoLinter/"]
RUN dotnet restore "RepoLinter/RepoLinter.csproj"
#COPY ConfigFile.toml .
COPY . .
WORKDIR "/src/RepoLinter"
RUN dotnet build "RepoLinter.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RepoLinter.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RepoLinter.dll"]
