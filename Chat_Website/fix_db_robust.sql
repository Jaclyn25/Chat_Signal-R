BEGIN TRANSACTION;

-- Skip ProfileImageUrl if it already exists
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[AspNetUsers]') AND name = N'ProfileImageUrl')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [ProfileImageUrl] nvarchar(max) NULL;
END

-- Create the join table if it doesn't exist
IF OBJECT_ID(N'[ChatRoomUserApplication]', N'U') IS NULL
BEGIN
    CREATE TABLE [ChatRoomUserApplication] (
        [ChatRoomsId] int NOT NULL,
        [UsersId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_ChatRoomUserApplication] PRIMARY KEY ([ChatRoomsId], [UsersId]),
        CONSTRAINT [FK_ChatRoomUserApplication_AspNetUsers_UsersId] FOREIGN KEY ([UsersId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ChatRoomUserApplication_chatrooms_ChatRoomsId] FOREIGN KEY ([ChatRoomsId]) REFERENCES [chatrooms] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_ChatRoomUserApplication_UsersId] ON [ChatRoomUserApplication] ([UsersId]);
END

-- Record the migration
IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260323015745_FinalRestore')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260323015745_FinalRestore', N'8.0.25');
END

COMMIT;
