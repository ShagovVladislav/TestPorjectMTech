# TestProjectMTech

REST API на .NET 10 для учета товаров на складе.

## Что реализовано

- Категории: получение списка, получение по `id` и создание.
- Товары: получение списка, фильтрация по `status` и `categoryId`, получение по `id`, создание, смена статуса.
- Бизнес-правила статусов: `Active -> Defective`, `Defective -> WriteOff`; обратные переходы и `Active -> WriteOff` запрещены.
- Уникальность `sku`.
- Валидация обязательных полей `name`, `sku`, `categoryId`.
- JSON-ответы для ошибок приложения.
- EF Core migrations и seed-данные: 3 категории и 5 товаров разных статусов.
- Swagger UI для ручного тестирования.
- Docker Compose для приложения и PostgreSQL.

## Переменные окружения

| Переменная | Назначение | Значение по умолчанию |
| --- | --- | --- |
| `ConnectionStrings__DefaultConnection` | строка подключения API к PostgreSQL | `Host=localhost;Port=5000;Database=warehouse_db;Username=postgres;Password=postgres` |
| `ASPNETCORE_ENVIRONMENT` | окружение ASP.NET Core | `Development` для локального запуска и compose |
| `TEST_DB_CONNECTION_STRING` | fallback-строка подключения для ручного запуска тестовой инфраструктуры без Testcontainers | `Host=localhost;Port=5000;Database=warehouse_db;Username=postgres;Password=postgres` |

## Локальный запуск

Поднять PostgreSQL:

```powershell
docker compose up -d postgres
```

Запустить API:

```powershell
dotnet run --project TestProjectMTech\TestProjectMTech.csproj --launch-profile http
```

При старте приложение автоматически применяет миграции и создает seed-данные.

Swagger UI:

```text
http://localhost:5118/swagger
```

## Запуск через Docker Compose

```powershell
docker compose up --build
```

Swagger UI в compose:

```text
http://localhost:8082/swagger
```

Остановить контейнеры:

```powershell
docker compose down
```

Полностью удалить контейнеры и данные PostgreSQL:

```powershell
docker compose down -v
```

## Миграции

Ручной запуск миграций обычно не нужен, потому что приложение делает это при старте. Если нужно применить миграции вручную:

```powershell
dotnet ef database update --project TestProjectMTech\TestProjectMTech.csproj --startup-project TestProjectMTech\TestProjectMTech.csproj --context TestProjectMTech.api.Data.WarehouseDbContext
```

Проверить, что модель совпадает с миграциями:

```powershell
dotnet ef migrations has-pending-model-changes --project TestProjectMTech\TestProjectMTech.csproj --startup-project TestProjectMTech\TestProjectMTech.csproj --context TestProjectMTech.api.Data.WarehouseDbContext
```

## Тесты

Интеграционные и функциональные тесты сами поднимают PostgreSQL-контейнер через Testcontainers. Для запуска тестов нужен запущенный Docker.

```powershell
dotnet test
```

Запустить только функциональные тесты:

```powershell
dotnet test --filter "FullyQualifiedName~Functional"
```

Запустить только интеграционные тесты:

```powershell
dotnet test --filter "FullyQualifiedName~Integration"
```

## API

Примеры пагинации:

```text
GET /api/products?page=2&pageSize=2
GET /api/products?status=Active&categoryId=1&page=1&pageSize=10
```

`page` начинается с `1`, `pageSize` должен быть от `1` до `100`.

| Метод | Путь | Описание |
| --- | --- | --- |
| `GET` | `/api/categories` | список категорий |
| `GET` | `/api/categories/{id}` | получить категорию по id |
| `POST` | `/api/categories` | создать категорию |
| `GET` | `/api/products` | список товаров, фильтры `status`, `categoryId`, пагинация `page`, `pageSize` |
| `GET` | `/api/products/{id}` | получить товар по id |
| `POST` | `/api/products` | создать товар |
| `PATCH` | `/api/products/{id}/status?status=Defective` | изменить статус товара |
