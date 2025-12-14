# Самостоятельная работа 1: Фреймворк автоматизации для DNS-Shop

## Описание фреймворка

Фреймворк автоматизации тестирования для интернет-магазина DNS (dns-shop.ru), реализованный с использованием паттерна **Page Object Model (POM)**.

### Архитектура фреймворка

```
Ind1.cs
├── BasePage (базовый класс)
│   └── Общие методы ожидания и взаимодействия с элементами
├── Page Objects
│   ├── DnsMainPage (главная страница)
│   ├── SearchResultsPage (страница результатов поиска)
│   └── ProductPage (страница товара)
└── Ind1_DnsShopTests (тестовый класс)
    └── 5 модульных тестов
```

## Компоненты фреймворка

### 1. BasePage (Базовый класс)
Содержит общие методы для всех страниц:
- `WaitAndFindElement()` - ожидание и поиск элемента
- `WaitAndClick()` - ожидание и клик по элементу

### 2. Page Objects

#### DnsMainPage
- `NavigateTo()` - открытие главной страницы
- `SearchProduct()` - поиск товара
- `GetCurrentCity()` - получение текущего города
- `OpenCatalog()` - открытие каталога
- `IsCatalogVisible()` - проверка видимости каталога
- `IsCartIconDisplayed()` - проверка отображения корзины

#### SearchResultsPage
- `GetProductCount()` - подсчет количества товаров
- `AreProductsDisplayed()` - проверка отображения товаров
- `GetFirstProductTitle()` - получение названия первого товара
- `DoesSearchContainKeyword()` - проверка релевантности результатов

#### ProductPage
- `GetProductTitle()` - получение названия товара
- `IsPriceDisplayed()` - проверка отображения цены
- `IsBuyButtonDisplayed()` - проверка кнопки покупки

## Реализованные тесты

### Test1_MainPageElementsAreDisplayed
Проверяет отображение основных элементов главной страницы:
- Заголовок содержит "DNS"
- Иконка корзины видна
- Корректный URL

### Test2_SearchFunctionalityWorks
Проверяет работу функционала поиска:
- Поиск товара по ключевому слову "ноутбук"
- Отображение результатов поиска
- Количество результатов больше 0

### Test3_SearchResultsAreRelevant
Проверяет релевантность результатов поиска:
- Поиск по запросу "смартфон"
- Наличие результатов
- Корректность названия товара

### Test4_CatalogCanBeOpened
Проверяет открытие каталога товаров:
- Клик по кнопке каталога
- Видимость меню каталога

### Test5_CityIsDisplayed
Проверяет отображение города пользователя:
- Город отображается на странице
- Название города не пустое

## Запуск тестов

### Запустить все тесты самостоятельной работы:
```powershell
cd "c:\Users\kulikovMA\Desktop\selenium\Selenium\Selenium"
dotnet test --filter "FullyQualifiedName~Ind1"
```

### Запустить конкретный тест:
```powershell
dotnet test --filter "FullyQualifiedName~Test1_MainPageElementsAreDisplayed"
```

### Запустить с подробным логом:
```powershell
dotnet test --filter "FullyQualifiedName~Ind1" --logger "console;verbosity=detailed"
```

## Преимущества реализованного фреймворка

1. **Page Object Model** - инкапсуляция логики страниц
2. **Переиспользуемость** - базовые методы в BasePage
3. **Читаемость** - понятные названия методов и тестов
4. **Масштабируемость** - легко добавлять новые страницы и тесты
5. **Maintainability** - изменения локаторов в одном месте
6. **Явные ожидания** - стабильность тестов
7. **Атрибуты Description** - документирование тестов

## Технологии

- C# .NET 10
- Selenium WebDriver 4.4.0
- NUnit 4.x
- Page Object Model Pattern
- Explicit Waits (WebDriverWait)

## Примечания

- Тесты используют явные ожидания для стабильности
- Все локаторы инкапсулированы в Page Objects
- Каждый тест независим и может выполняться отдельно
- ChromeDriver запускается автоматически благодаря NuGet пакету
