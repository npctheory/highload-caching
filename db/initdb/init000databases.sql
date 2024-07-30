-- Create user
CREATE USER docker;

-- Create database
CREATE DATABASE highload_social;

-- Grant privileges
GRANT ALL PRIVILEGES ON DATABASE highload_social TO docker;

-- Connect to the database
\c highload_social;

CREATE TABLE roles (
    id BIGSERIAL CONSTRAINT roles_pk PRIMARY KEY,
    name VARCHAR(20) NOT NULL
);

INSERT INTO roles (name) VALUES 
('ROLE_ADMIN'),
('ROLE_USER');

CREATE TABLE IF NOT EXISTS users (
    id VARCHAR(255) PRIMARY KEY,
    password_hash VARCHAR(255) NOT NULL,
    role_id BIGSERIAL NOT NULL CONSTRAINT users_roles_id_fk REFERENCES roles ON UPDATE CASCADE ON DELETE CASCADE,
    first_name VARCHAR(255) NOT NULL,
    second_name VARCHAR(255) NOT NULL,
    birthdate DATE NOT NULL,
    biography TEXT,
    city VARCHAR(255)
);

INSERT INTO users VALUES
('admin','$2a$10$.mcwiWcTMn2u6ToxnzCmc.EN0RqcZ2HOxMjwPg1LTD3ac8lUj3M8G',1,'Admin','Admin','1970-01-01','Administration','Moscow'),
('user','$2a$10$yHWO5o4p2aQ.vmxkxBIxWezk5mB8V2soVO3xRG9fBjFweNI67qZk2',2,'User','User','1970-01-01','User','Moscow');


CREATE TABLE IF NOT EXISTS friends (
    user_id VARCHAR(255) NOT NULL,
    friend_id VARCHAR(255) NOT NULL,
    PRIMARY KEY (user_id, friend_id),
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (friend_id) REFERENCES users(id)
);

-- Create the posts table with UUID as id and a creation timestamp
CREATE TABLE IF NOT EXISTS posts (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    text VARCHAR(1000) NOT NULL,
    user_id VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,  -- New column for creation timestamp
    FOREIGN KEY (user_id) REFERENCES users(id)
);