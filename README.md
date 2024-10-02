# Intermediate+ mentoring program

### 1. Set-up guide
----

1. Установить настроить docker по инструкции с официального сайта https://docs.docker.com/desktop/install/windows-install/ 
2. Скопировать репозиторий к себе на ПК используя git
3. Используя консоль вашей ОС отрыть папку `src` в руте репозитория
4. Из папки `src` ввести команду `docker compose up` которая поднимет базу данных на `localhost:5400` и pgAdmin4 на `localhost:5555`
5. Откройте в браузере `localhost:5555` и используйте `postgres@postgres.com` и `postgres` как логин и пароль соответственно
6. Нажмите 'Add new server' в интерфейсе приложения. <br>
 В окне General укажите любое имя в поле Name. <br>
 В окне Connection укажите Host name/address `mentoring_database`, Username `postgres`, Password `postgres`. <br>
 Нажмите Save.
7. (Опционально) Заполните базу данных тестовыми записями: <br>
   1. В Object Explorer разверните добавленый сервер, разверните вкладку Databases и выберете postgres
   2. На верхней части Object Explorer нажмите кнопку Query Tool
   3. Справа должна открыться вкладка Query
   4. Скопируйте текст файла `seed.sql` из папки `src` и вставьте его во вкладку Query
   5. Нажмите Execute Query кнопку выше вкладки Query.

### 2. Connection string
----

> Server=localhost; Port=5400; User Id=postgres; Password=postgres; Database=postgres;