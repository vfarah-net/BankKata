
### Requirements

Start with a class following this structure

```csharp
public class Account {
  public void Deposit(int amount);
  public void Withdraw(int amount);
  public void PrintStatement(int amount);
}
```

### The Rules

1. You are not allowed to add any other public methods on the class
2. Use strings and integers for dates and amounts (keep it simple)
3. Dont worry about the spacing in the statement printed in the console

### Acceptance Criteria

Statements should have transactions in this format

>	DATE		| AMOUNT  | BALANCE

>	10/04/2014	| 500.00  | 1400.00

>	02/04/2014	| -100.00 | 900.00

>	01/04/2014	| 1000.00 | 1000.00

### Scenarios
> Given a client makes a deposit of 1000 on the first date 

And a withdrawal of 100 on the next date

And a deposit of 500 on a final date  

When they print their bank statements

Then they should see  

>	DATE		| AMOUNT  | BALANCE

>	10/04/2014	| 500.00  | 1400.00

>	02/04/2014	| -100.00 | 900.00

>	01/04/2014	| 1000.00 | 1000.00
