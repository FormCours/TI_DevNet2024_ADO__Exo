CREATE PROCEDURE [dbo].[DeleteStudent]
	@StudentId int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [Student] WHERE [Id] = @StudentId
END
