-- Таблица Пользователи
CREATE TABLE Users (
    id INT IDENTITY(1,1) PRIMARY KEY, -- Уникальный идентификатор пользователя
    last_name NVARCHAR(100) NOT NULL, -- Фамилия
    first_name NVARCHAR(100) NOT NULL, -- Имя
    middle_name NVARCHAR(100), -- Отчество
    login NVARCHAR(50) UNIQUE NOT NULL, -- Логин (уникальный)
    password_hash NVARCHAR(255) NOT NULL, -- Хеш пароля
    phone NVARCHAR(20), -- Телефон
    email NVARCHAR(100), -- Email
);

-- Таблица Категории
CREATE TABLE Categories (
    id INT IDENTITY(1,1) PRIMARY KEY, -- Уникальный идентификатор категории
    name NVARCHAR(100) NOT NULL -- Название категории
);

-- Таблица Продукция
CREATE TABLE Products (
    id INT IDENTITY(1,1) PRIMARY KEY, -- Уникальный идентификатор продукта
    category_id INT FOREIGN KEY REFERENCES Categories(id) ON DELETE CASCADE, -- Ссылка на категорию
    name NVARCHAR(255) NOT NULL, -- Название продукта
    description TEXT, -- Описание продукта
    image_url NVARCHAR(255), -- URL изображения продукта
    price DECIMAL(10, 2) NOT NULL -- Стоимость продукта
);

-- Таблица Платежи
CREATE TABLE Payments (
    id INT IDENTITY(1,1) PRIMARY KEY, -- Уникальный идентификатор платежа
    user_id INT FOREIGN KEY REFERENCES Users(id) ON DELETE CASCADE, -- Ссылка на пользователя
    product_id INT FOREIGN KEY REFERENCES Products(id) ON DELETE CASCADE, -- Ссылка на продукт
    quantity INT NOT NULL, -- Количество купленной продукции
    payment_date DATE NOT NULL, -- Дата платежа
    total_price DECIMAL(10, 2) NOT NULL -- Итоговая стоимость платежа
);