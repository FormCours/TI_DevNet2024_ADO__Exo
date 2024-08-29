CREATE PROCEDURE [dbo].[AddStudent]
	@FirstName VARCHAR(50), 
	@LastName VARCHAR(50),
	@BirthDate DATETIME2,
	@YearResult INT,
	@SectionId INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Student] ([FirstName], [LastName], [SectionId], [YearResult], [BirthDate])
	 VALUES (@FirstName, @LastName, @SectionId, @YearResult, @BirthDate);
END
