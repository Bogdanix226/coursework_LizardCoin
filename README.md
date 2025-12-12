# coursework_LizardCoin
Розширена інструкція по особливостях, збірці та запусках проєкту:

Попередні вимоги:
1.	.NET8 
2.	Microsoft SQL Server (або LocalDB, що йде разом з Visual Studio)
3.	Visual Studio 2022 

Інструкція зі встановлення та запуску:

1.	Клонування репозитрію:
Виконайте команду :
 git clone https://github.com/your-username/LizardCoin.git
Після цього відкрийте проєкт в IDE.
2.	Налаштування БД:
Відкрийте файл appsettings.json та перевірте рядок підключення (ConnectionStrings). За замовчуванням він налаштований на LocalDB:
  "ConnectionStrings": {
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LizardCoinDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
3.	Застосування міграцій
Для створення бази даних та необхідних таблиць виконайте команду міграції.
Visual Studio - Package Manager Console: Update-Database
4.	Налаштування доступу по локальній мережі (для тестування з іншого пристрою)
Щоб відкрити веб-застосунок з мобільного телефону або іншого комп'ютера у тій самій Wi-Fi мережі, необхідно налаштувати прив'язку до IP-адреси.
1.	Дізнайтеся свою локальну IP-адресу:
o	Відкрийте термінал (cmd) і введіть команду ipconfig.
o	Знайдіть значення IPv4 Address (наприклад 192.168.0.6).
2.	Відредагуйте конфігурацію запуску:
o	Відкрийте файл Properties/launchSettings.json.
o	У профілі "http" знайдіть параметр "applicationUrl".
o	Додайте вашу IP-адресу через крапку з комою, вказавши порт (наприклад, 5123):
"profiles": {
  "http": {
    "commandName": "Project",
    "dotnetRunMessages": true,
    "launchBrowser": true,
    "applicationUrl": "http://localhost:5123;http://192.168.0.6:5123", <- СЮДИ
    "environmentVariables": {
      "ASPNETCORE_ENVIRONMENT": "Development"
    }
  }
5.	Запуск проєкту. Натисність на кнопку ▶️ https або f5.

©Розроблено в рамках курсової роботи. Рік: 2025
