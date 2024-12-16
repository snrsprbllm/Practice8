-- ������� ������������
CREATE TABLE Users (
    id INT IDENTITY(1,1) PRIMARY KEY, -- ���������� ������������� ������������
    last_name NVARCHAR(100) NOT NULL, -- �������
    first_name NVARCHAR(100) NOT NULL, -- ���
    middle_name NVARCHAR(100), -- ��������
    login NVARCHAR(50) UNIQUE NOT NULL, -- ����� (����������)
    password_hash NVARCHAR(255) NOT NULL, -- ��� ������
    phone NVARCHAR(20), -- �������
    email NVARCHAR(100), -- Email
);

-- ������� ���������
CREATE TABLE Categories (
    id INT IDENTITY(1,1) PRIMARY KEY, -- ���������� ������������� ���������
    name NVARCHAR(100) NOT NULL -- �������� ���������
);

-- ������� ���������
CREATE TABLE Products (
    id INT IDENTITY(1,1) PRIMARY KEY, -- ���������� ������������� ��������
    category_id INT FOREIGN KEY REFERENCES Categories(id) ON DELETE CASCADE, -- ������ �� ���������
    name NVARCHAR(255) NOT NULL, -- �������� ��������
    description TEXT, -- �������� ��������
    image_url NVARCHAR(255), -- URL ����������� ��������
    price DECIMAL(10, 2) NOT NULL -- ��������� ��������
);

-- ������� �������
CREATE TABLE Payments (
    id INT IDENTITY(1,1) PRIMARY KEY, -- ���������� ������������� �������
    user_id INT FOREIGN KEY REFERENCES Users(id) ON DELETE CASCADE, -- ������ �� ������������
    product_id INT FOREIGN KEY REFERENCES Products(id) ON DELETE CASCADE, -- ������ �� �������
    quantity INT NOT NULL, -- ���������� ��������� ���������
    payment_date DATE NOT NULL, -- ���� �������
    total_price DECIMAL(10, 2) NOT NULL -- �������� ��������� �������
);