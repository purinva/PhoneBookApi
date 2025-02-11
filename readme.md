# api-explorer-hub

**api-explorer-hub** — достаточно простое веб-приложение, включающее в себя реализацию управления телефонной книгой и выполнение операций по добавлению и удалению, на котором отработан брокер сообщений **Kafka** и база данных **Redis** для кэширования.

**Frontend** составляющая была реализована с помощью библиотеки **React** и **Vite**. Весь код был написан на **JavaScript**.

**Backend** составляющая была реализована на основе **ASP.NET Core Web API**, включая в себе чистую и расширяемую архитектуру с логически грамотным разделением слоёв и соблюдением принципов **REST**. В качестве базы данных была выбрана **Postgresql**. Взаимодействие с базой данных осуществляется через ORM **Entity Framework Core**, позволяя работать с базой данных с помощью объектов .NET. В качестве набора инструментов для автоматического описания **RESTful API** был выбран **Swagger**. Для работы с Dto-моделями использовалась библиотека **AutoMapper**.

**Инструкция по запуску**
1. Для запуска контейнера необходимо прописать команду "docker-compose up --build" в PhoneBook/PhoneBookApi/PhoneBookApi
2. Для запуска backend составляющй в том же месте прописать "dotnet run"
3. Для запуска frontend составляющей прописать команду "npm run dev" в PhoneBook/frontend
4. В консоли выводятся все действия пользователя по добавлению или удалению контактов.
