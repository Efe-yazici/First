-- Initialize HastaTakip Database
-- This script creates the basic database structure

USE master;
GO

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'HastaTakip')
BEGIN
    CREATE DATABASE HastaTakip;
END
GO

USE HastaTakip;
GO

-- Create basic tables (based on the application code analysis)
-- Hasta Bilgi (Patient Information) table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HastaBilgi' AND xtype='U')
BEGIN
    CREATE TABLE HastaBilgi (
        ID int IDENTITY(1,1) PRIMARY KEY,
        Ad nvarchar(50),
        Soyad nvarchar(50),
        TC nvarchar(11),
        Telefon nvarchar(15),
        Yas int,
        Cinsiyet nvarchar(10),
        Sikayet nvarchar(500),
        Tarih datetime,
        DurumID int,
        BolumID int,
        HastaYasiyorMi bit DEFAULT 1
    );
END
GO

-- Durum (Status) table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Durum' AND xtype='U')
BEGIN
    CREATE TABLE Durum (
        durumID int IDENTITY(1,1) PRIMARY KEY,
        durumAd nvarchar(50)
    );
    
    -- Insert default status values
    INSERT INTO Durum (durumAd) VALUES 
    ('Beklemede'),
    ('Muayene'),
    ('Tedavi'),
    ('Taburcu');
END
GO

-- Bolum (Department) table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Bolum' AND xtype='U')
BEGIN
    CREATE TABLE Bolum (
        BolumID int IDENTITY(1,1) PRIMARY KEY,
        BolumAd nvarchar(50)
    );
    
    -- Insert default department values
    INSERT INTO Bolum (BolumAd) VALUES 
    ('Genel Pratisyen'),
    ('İç Hastalıkları'),
    ('Çocuk Doktoru'),
    ('Göz Doktoru'),
    ('Kulak Burun Boğaz');
END
GO

-- User table for login
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Kullanici' AND xtype='U')
BEGIN
    CREATE TABLE Kullanici (
        ID int IDENTITY(1,1) PRIMARY KEY,
        KulAd nvarchar(50) UNIQUE,
        Sifre nvarchar(100)
    );
    
    -- Insert default admin user (password: Admin123!)
    INSERT INTO Kullanici (KulAd, Sifre) VALUES ('admin', 'Admin123!');
END
GO

-- Create stored procedures (if they don't exist)

-- Procedure for user registration
IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'kayitOl')
BEGIN
    EXEC('
    CREATE PROCEDURE kayitOl
        @KulAd nvarchar(50),
        @Sifre nvarchar(100)
    AS
    BEGIN
        INSERT INTO Kullanici (KulAd, Sifre) VALUES (@KulAd, @Sifre)
    END')
END
GO

-- Procedure to fill status dropdown
IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'durumDoldur')
BEGIN
    EXEC('
    CREATE PROCEDURE durumDoldur
    AS
    BEGIN
        SELECT durumID, durumAd FROM Durum
    END')
END
GO

-- Procedure to fill department dropdown
IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'bolumDoldur')
BEGIN
    EXEC('
    CREATE PROCEDURE bolumDoldur
    AS
    BEGIN
        SELECT BolumID, BolumAd FROM Bolum
    END')
END
GO

-- Procedure to update patient information
IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'guncelle')
BEGIN
    EXEC('
    CREATE PROCEDURE guncelle
        @id int,
        @ad nvarchar(50),
        @soyad nvarchar(50),
        @tc nvarchar(11),
        @tel nvarchar(15),
        @yas int,
        @cinsiyet nvarchar(10),
        @sikayet nvarchar(500),
        @tarih datetime,
        @durum int,
        @bolum int,
        @yasiyorMu bit
    AS
    BEGIN
        UPDATE HastaBilgi 
        SET Ad=@ad, Soyad=@soyad, TC=@tc, Telefon=@tel, Yas=@yas, 
            Cinsiyet=@cinsiyet, Sikayet=@sikayet, Tarih=@tarih, 
            DurumID=@durum, BolumID=@bolum, HastaYasiyorMi=@yasiyorMu
        WHERE ID=@id
    END')
END
GO

-- Procedure to delete patient
IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'sil')
BEGIN
    EXEC('
    CREATE PROCEDURE sil
        @id int
    AS
    BEGIN
        DELETE FROM HastaBilgi WHERE ID=@id
    END')
END
GO

PRINT 'Database HastaTakip initialized successfully!';