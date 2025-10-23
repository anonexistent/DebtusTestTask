# Базовый образ для runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Установка системных зависимостей для Playwright
RUN apt-get update && apt-get install -y \
    libnss3 \
    libatk-bridge2.0-0 \
    libxkbcommon0 \
    libxcomposite1 \
    libxdamage1 \
    libxrandr2 \
    libgbm1 \
    libpango-1.0-0 \
    libcairo2 \
    libasound2 \
    libatk1.0-0 \
    libgtk-3-0 \
    fonts-liberation \
    && rm -rf /var/lib/apt/lists/*

# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DebtusTestTask.Api/DebtusTestTask.Api.csproj", "DebtusTestTask.Api/"]
COPY ["DebtusTestTask.Infrastructure.Data/DebtusTestTask.Infrastructure.Data.csproj", "DebtusTestTask.Infrastructure.Data/"]
COPY ["DebtusTestTask.Integrations.OrangeHRM/DebtusTestTask.Integrations.OrangeHRM.csproj", "DebtusTestTask.Integrations.OrangeHRM/"]
COPY ["DebtusTestTask.Integrations.OrangeHRM.Contracts/DebtusTestTask.Integrations.OrangeHRM.Contracts.csproj", "DebtusTestTask.Integrations.OrangeHRM.Contracts/"]
COPY ["DebtusTestTask.Infrastructure/DebtusTestTask.Infrastructure.csproj", "DebtusTestTask.Infrastructure/"]
COPY ["DebtusTestTask.Application/DebtusTestTask.Application.csproj", "DebtusTestTask.Application/"]
COPY ["DebtusTestTask.Contracts/DebtusTestTask.Contracts.csproj", "DebtusTestTask.Contracts/"]
COPY ["DebtusTestTask.Models/DebtusTestTask.Models.csproj", "DebtusTestTask.Models/"]
COPY ["DebtusTestTask.Utils/DebtusTestTask.Utils.csproj", "DebtusTestTask.Utils/"]
COPY ["DebtusTestTask.Integrations.OrangeHRM.Services/DebtusTestTask.Integrations.OrangeHRM.Services.csproj", "DebtusTestTask.Integrations.OrangeHRM.Services/"]
RUN dotnet restore "./DebtusTestTask.Api/DebtusTestTask.Api.csproj"
COPY . .
WORKDIR "/src/DebtusTestTask.Api"
RUN dotnet build "./DebtusTestTask.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Установка Playwright и его браузеров
RUN dotnet add package Microsoft.Playwright --version 1.48.0
RUN dotnet build # Пересобираем, чтобы включить Playwright
WORKDIR /app/build
RUN pwsh -Command "./playwright.ps1 install --with-deps"

# Этап публикации
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "/src/DebtusTestTask.Api/DebtusTestTask.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Копируем установленные браузеры Playwright
COPY --from=build /root/.cache/ms-playwright /root/.cache/ms-playwright
ENTRYPOINT ["dotnet", "DebtusTestTask.Api.dll"]