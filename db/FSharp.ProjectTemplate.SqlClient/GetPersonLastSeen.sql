/* uncomment for testing
declare @FirstNameVal nvarchar(max) = 'Adam'
declare @LastNameVal nvarchar(max) = 'Bernau'
*/

declare @FirstName nvarchar(max) = @FirstNameVal
declare @LastName nvarchar(max) = @LastNameVal

select [LastSeen] from [dbo].[Persons] 
where [FirstName]=ISNULL(@FirstName, [FirstName]) and [LastName]=ISNULL(@LastName, [LastName])

