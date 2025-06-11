/* Create the database */
CREATE DATABASE  IF NOT EXISTS Malshinon;

/* Switch to the classicmodels database */
USE Malshinon;



/* Create the tables */
CREATE TABLE People 
(
    id INT AUTO_INCREMENT PRIMARY KEY ,
    first_name varchar(50) UNIQUE,
    last_name varchar(50),
    secret_code varchar(10) UNIQUE,
    type_people ENUM('reporter', 'target', 'both', 'potential_agent'),
    num_reports INT DEFAULT 0,
    num_mentions INT DEFAULT 0
);

CREATE TABLE IntelReports
(
    id INT PRIMARY KEY AUTO_INCREMENT,
    reporter_ID INT,
    target_ID INT,
    text_report TEXT,
    timestamp DATETIME DEFAULT NOW(),

    FOREIGN KEY (reporter_ID) REFERENCES People(id),
    FOREIGN KEY (target_ID) REFERENCES People(id)
);

-- CREATE TABLE Alerts
-- (
--     id INT AUTO_INCREMENT PRIMARY KEY ,
-- 	target_ID INT, 
--     created_at DATETIME DEFAULT NOW(),
--     reason  VARCHAR(250)
-- );