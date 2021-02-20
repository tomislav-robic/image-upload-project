CREATE TABLE Image
(
    Id     int PRIMARY KEY IDENTITY,
    UserId nvarchar(450) FOREIGN KEY REFERENCES dbo.AspNetUsers (Id),
    FileName nvarchar(256) NOT NULL,
    LocalFilePath nvarchar(512) NOT NULL,
    Tags nvarchar(max) NULL,
    Description nvarchar(max) NULL,
    Timestamp datetime2(0) NOT NULL,
    ImageSize bigint NOT NULL
)