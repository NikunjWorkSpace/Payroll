IF DB_ID(N'PayBridgeDb') IS NULL
BEGIN
    CREATE DATABASE [PayBridgeDb];
END;
GO

USE [PayBridgeDb];
GO

:r /workspace/SQL/CreateTables.sql
:r /workspace/SQL/SampleData.sql
