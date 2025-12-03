DROP DATABASE IF EXISTS Vaktbok_2;
DROP USER IF EXISTS 'robot_user'@'localhost';
DROP USER IF EXISTS 'reader_user'@'localhost';

CREATE DATABASE IF NOT EXISTS Vaktbok_2
	DEFAULT CHARACTER SET utf8mb4
	DEFAULT COLLATE utf8mb4_unicode_ci;

-- Robot user --
CREATE USER IF NOT EXISTS 'robot_user'@'localhost' IDENTIFIED BY 'robot_user_password';
GRANT SELECT, INSERT, UPDATE, DELETE ON Vaktbok_2.* TO 'robot_user'@'localhost';

CREATE USER IF NOT EXISTS 'reader_user'@'localhost' IDENTIFIED BY 'reader_user_password';
GRANT SELECT ON Vaktbok_2.* TO 'reader_user'@'localhost';
