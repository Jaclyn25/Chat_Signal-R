BEGIN TRANSACTION;

-- Skip the dropping of constraints and table rename for ChatMessages/messages 
-- since the table 'messages' already exists and 'ChatMessages' does not.

ALTER TABLE [AspNetUsers] ADD [ProfileImageUrl] nvarchar(max) NULL;

CREATE TABLE [ChatRoomUserApplication] (
    [ChatRoomsId] int NOT NULL,
    [UsersId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_ChatRoomUserApplication] PRIMARY KEY ([ChatRoomsId], [UsersId]),
    CONSTRAINT [FK_ChatRoomUserApplication_AspNetUsers_UsersId] FOREIGN KEY ([UsersId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ChatRoomUserApplication_chatrooms_ChatRoomsId] FOREIGN KEY ([ChatRoomsId]) REFERENCES [chatrooms] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_ChatRoomUserApplication_UsersId] ON [ChatRoomUserApplication] ([UsersId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260323015745_FinalRestore', N'8.0.25');

COMMIT;
