CREATE TRIGGER [TR_Student_SoftDelete]
ON [dbo].[Student]
INSTEAD OF DELETE
AS
BEGIN
	SET NOCOUNT ON

	UPDATE [Student]
	 SET [Active] = 0
	FROM [Student]
	 JOIN [deleted] ON [Student].[Id] = [deleted].[Id]
END
