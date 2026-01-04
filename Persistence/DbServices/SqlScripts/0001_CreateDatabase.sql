-- Create database
CREATE DATABASE IF NOT EXISTS Vaktbok_2
	DEFAULT CHARACTER SET utf8mb4
	DEFAULT COLLATE utf8mb4_unicode_ci;
   
-- Robot user --
CREATE USER IF NOT EXISTS 'robot_user'@'localhost' 
IDENTIFIED BY 'robot_user_password';
GRANT SELECT, INSERT, UPDATE, DELETE ON Vaktbok_2.* 
TO 'robot_user'@'localhost';

-- Readonly user --
CREATE USER IF NOT EXISTS 'reader_user'@'localhost' 
IDENTIFIED BY 'reader_user_password';
GRANT SELECT ON Vaktbok_2.* 
TO 'reader_user'@'localhost';

FLUSH PRIVILEGES;
