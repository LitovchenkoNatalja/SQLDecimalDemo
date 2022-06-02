

DROP PROCEDURE MathCalculations
GO

CREATE PROCEDURE MathCalculations @number1 decimal(24, 10), @number2 decimal(24, 10)
AS
SELECT @number1 + @number2 AS Addition, @number1 - @number2 AS Subtraction, @number1 * @number2 AS Multiplication, @number1 / @number2 AS Division
GO

EXEC MathCalculations @number1 = 9.3, @number2 = 9.93;

DROP PROCEDURE MathCalculationsTest
GO

CREATE PROCEDURE MathCalculationsTest @number1 decimal(38, 36), @number2 decimal(38, 36)
AS
SELECT (@number1 + @number2) AS Addition, @number1 - @number2 AS Subtraction, @number1 * @number2 AS Multiplication, @number1 / @number2 AS Division
GO

--EXEC MathCalculations @number1 = 10000000000.1834567800, @number2 = 10000000000.1234567899;

EXEC MathCalculations @number1 = 9.9999, @number2 = 9.9999;