/* uncomment for testing
declare @FirstNameVal nvarchar(max) = 'Adam'
declare @LastNameVal nvarchar(max) = 'Bernau'
declare @LastSeenVal datetime = getdate()
*/
declare @FirstName nvarchar(max) = @FirstNameVal
declare @LastName nvarchar(max) = @LastNameVal
declare @LastSeen datetime = getdate()

BEGIN TRAN
UPDATE [dbo].[Persons] with (serializable) SET 
	[LastSeen] = @LastSeen
	WHERE [FirstName] = @FirstName AND LastName = @LastName
IF @@ROWCOUNT = 0
BEGIN
  INSERT [Persons](
	[FirstName],LastName,[LastSeen]
  ) 
	SELECT @FirstName, @LastName, @LastSeen;
END

select [FirstName],LastName,[LastSeen]
	from [Persons]
WHERE [FirstName] = @FirstName AND LastName = @LastName

COMMIT
