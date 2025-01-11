# Используем официальный образ .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Копируем файлы проекта в контейнер
COPY *.csproj ./
RUN dotnet restore

# Копируем остальные файлы приложения и выполняем сборку
COPY . ./
RUN dotnet publish -c Release -o out

# Используем минимальный образ .NET Runtime для выполнения приложения
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Копируем собранное приложение из предыдущего шага
COPY --from=build /app/out .

# Открываем порт 80 для доступа к приложению
EXPOSE 80

# Устанавливаем команду запуска приложения
ENTRYPOINT ["dotnet", "WebApplication1.dll"]