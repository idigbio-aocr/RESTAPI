CREATE TABLE [dbo].[SourceImage] (
    [sourceImageId]           UNIQUEIDENTIFIER CONSTRAINT [DF_SourceImage_sourceImageId] DEFAULT (newsequentialid()) ROWGUIDCOL NOT NULL,
    [sourceImageRepositoryId] UNIQUEIDENTIFIER NOT NULL,
    [fileName]                NVARCHAR (500)   NULL,
    [url]                     NVARCHAR (1000)  NULL,
    [alternateId]             NVARCHAR (100)   NULL,
    [createdUTCDateTime]      DATETIME         CONSTRAINT [DF_SourceImage_createdUTCDateTime] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_SourceImage] PRIMARY KEY CLUSTERED ([sourceImageId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_SourceImageRepositoryUrl_SourceImageRepository] FOREIGN KEY ([sourceImageRepositoryId]) REFERENCES [dbo].[SourceImageRepository] ([sourceImageRepositoryId])
);

