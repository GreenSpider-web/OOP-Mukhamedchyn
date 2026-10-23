# Звіт до самостійної роботи №3
**Тема:** Аналіз інкапсуляції в open-source проєктах. Практики валідації полів

## 1. Досліджуваний проєкт
* **Назва:** .NET (`dotnet/runtime`).
* **Репозиторій:** [github.com/dotnet/runtime](https://github.com/dotnet/runtime).
* **Проаналізовані класи:** [`UriBuilder`](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.Uri/src/System/UriBuilder.cs), [`HttpRequestMessage`](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Net.Http/src/System/Net/Http/HttpRequestMessage.cs) та [`HttpResponseMessage`](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Net.Http/src/System/Net/Http/HttpResponseMessage.cs).

## 2. Дослідження інкапсуляції та валідації
* **`UriBuilder`:** Зберігає частини URI у приватних полях, а властивості забезпечують контрольований доступ. Сеттер `Path` нормалізує роздільники та екранує рядок; `Query` і `Fragment` додають необхідні початкові символи. Сеттер `Scheme` перевіряє назву схеми, а `Port` відхиляє значення поза діапазоном від `-1` до `65535`.
* **`HttpRequestMessage`:** Інкапсулює метод запиту, URI, версію, заголовки та вміст. Властивості `Method` і `Version` перевіряють значення на `null`, `VersionPolicy` — допустимість значення переліку. Зміна властивостей також перевіряє, чи повідомлення не було утилізоване.
* **`HttpResponseMessage`:** Інкапсулює код відповіді, версію, заголовки, вміст і фразу стану. `StatusCode` перевіряє діапазон від `0` до `999`, `ReasonPhrase` відхиляє рядки з неприпустимими символами нового рядка, а `Version` — значення `null`. `IsSuccessStatusCode` доступна лише для читання й визначає, чи належить код до діапазону 200–299.
* Перевірка виконується у властивостях і конструкторах; некоректні значення обробляються винятками `ArgumentException`, `ArgumentOutOfRangeException`, `ArgumentNullException` і `FormatException`.
* У розглянутих класах властивості переважно працюють із приватними полями, а не є автоматичними. Індексатори та перевантажені оператори для описаних сценаріїв не використовуються.

## 3. Висновки
* Інкапсуляція приховує внутрішній стан і дає змогу контролювати його через властивості та методи.
* Валідація безпосередньо під час зміни значення допомагає рано виявити некоректні дані. Конкретні перевірки залежать від призначення властивості.
* Посилання на вихідний код: [репозиторій dotnet/runtime](https://github.com/dotnet/runtime) та файли [`UriBuilder.cs`](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.Uri/src/System/UriBuilder.cs), [`HttpRequestMessage.cs`](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Net.Http/src/System/Net/Http/HttpRequestMessage.cs), [`HttpResponseMessage.cs`](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Net.Http/src/System/Net/Http/HttpResponseMessage.cs).

## 4. Відповіді на контрольні запитання
1. **За якими ознаками можна зробити висновок, що в класі дотримано інкапсуляції?**
   Про це свідчить зберігання даних у приватних полях і контроль доступу та змін через публічні властивості й методи.
2. **Які підходи до валідації властивостей зустрічаються в коді?**
   У коді перевіряються `null`, числові межі, значення переліків, формат рядків і поточний стан об'єкта; помилки повідомляються винятками.
3. **Чому важливо наводити посилання на конкретні класи та фрагменти коду?**
   Так читач може перевірити зроблені висновки безпосередньо у вихідному коді.
4. **Які висновки щодо якості проєктування дає порівняння класів?**
   Розглянуті класи приховують внутрішній стан і перевіряють критичні дані, але конкретні правила залежать від призначення властивості.
