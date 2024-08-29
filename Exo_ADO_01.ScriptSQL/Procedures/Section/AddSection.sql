EXEC sp_addmessage @msgnum=60001, @severity=16, @msgtext= 'The section has no name!', @lang='us_english';
GO

EXEC sp_addmessage @msgnum=60001, @severity=16, @msgtext= 'La section ne possede pas de nom !', @lang='Français';
GO

CREATE PROCEDURE [dbo].[AddSection]
	@SectionId int,
	@SectionName VARCHAR(50)
AS
BEGIN
	IF(TRIM(@SectionName) = '')
	BEGIN
		RAISERROR (60001, 16, 1);
		RETURN;
	END

	INSERT INTO [Section] ([Id], [SectionName])
	 VALUES (@SectionId, @SectionName);
END