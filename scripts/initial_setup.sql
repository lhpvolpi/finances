CREATE DATABASE [finances];

USE [finances];

CREATE TABLE [users]
(
    [id] UNIQUEIDENTIFIER PRIMARY KEY,
    [email] NVARCHAR(255) NOT NULL UNIQUE,
    [password_hash] NVARCHAR(255) NOT NULL,
    [created_at] DATETIME NOT NULL,
    [updated_at] DATETIME NULL
);

CREATE TABLE [transactions]
(
    [id] UNIQUEIDENTIFIER PRIMARY KEY,
    [user_id] UNIQUEIDENTIFIER NOT NULL,
    [type] INT NOT NULL CHECK ([type] IN (1, 2)),
    [amount] DECIMAL(18,2) NOT NULL CHECK ([amount] > 0),
    [occurred_at] DATETIME NOT NULL,
    [created_at] DATETIME NOT NULL,
    [updated_at] DATETIME NULL,
    CONSTRAINT FK_transactions_users FOREIGN KEY ([user_id]) REFERENCES [users]([id])
);
GO
CREATE INDEX IDX_transactions_user ON [transactions] ([user_id]);

CREATE TABLE [messages]
(
    [id] UNIQUEIDENTIFIER PRIMARY KEY,
    [type] INT NOT NULL,
    [payload] NVARCHAR(MAX) NOT NULL,
    [processed] BIT NOT NULL DEFAULT 0,
    [processing_at] DATETIME NULL,
    [created_at] DATETIME NOT NULL,
    [updated_at] DATETIME NULL
);
GO
CREATE INDEX IDX_messages_unprocessed ON [messages] ([processed], [processing_at]);

CREATE TABLE [consolidations]
(
    [id] UNIQUEIDENTIFIER PRIMARY KEY,
    [user_id] UNIQUEIDENTIFIER NOT NULL,
    [date] DATE NOT NULL,
    [total_credits] DECIMAL(18,2) NOT NULL DEFAULT 0,
    [total_debits] DECIMAL(18,2) NOT NULL DEFAULT 0,
    [balance] DECIMAL(18,2) NOT NULL DEFAULT 0,
    [created_at] DATETIME NOT NULL,
    [updated_at] DATETIME NULL,
    CONSTRAINT FK_consolidations_users FOREIGN KEY ([user_id]) REFERENCES [users]([id]),
    CONSTRAINT UQ_consolidations_date UNIQUE ([user_id], [date])
);
GO
CREATE INDEX IDX_consolidations_user_date ON [consolidations] ([user_id], [date]);
