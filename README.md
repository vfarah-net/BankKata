
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
**Given** a client makes a deposit of 1000 on the first date 

**And** a withdrawal of 100 on the next date

**And** a deposit of 500 on a final date  

**When** they print their bank statements

**Then** they should see  

>	DATE		| AMOUNT  | BALANCE

>	10/04/2014	| 500.00  | 1400.00

>	02/04/2014	| -100.00 | 900.00

>	01/04/2014	| 1000.00 | 1000.00

### Version 1.0 Issues

The first version was too simple and yet and had a few bugs, like the fact that the date was not exactly as the requirments, as in utilising the exact values. 
The second issue is the fact that everything was tightly coupled to Account. 
Version 2.0 we would like to remove this and inject a TransactionRepository within this account to make it more extensible.

### Version 2.0 Issues
The first version was a very quick and simple way to get proof of concept, the requirments almost resolved according to a quick 2 hour session of what basically needed to be done.

The version 1 ideas had some tight coupling and SOLID issues, however did the job. Refactoring is always the third leg of test development and can always safely resolve any school boy mistakes made by quick tighlty focused ideas. Remember, no matter how old or much you are being paid to do the job, you still will make school boy mistakes, so be humble!

The Integration test is a good example of how this will be used in the real world and tests all the components together giving an example in steps how this can be used and setup. The BDD touch is nice for demonstrating to testers or sending to customers as proof that this has been done. This is live documentation that now lives tightly coupled with the solution, just the way we like it. 

FEATURE: [Story 1] AccountFeature
In order to print transactions
As a user
I want to print my bank statement
                                 
> SCENARIO: [Print Account Statement] PrintStatement

> STEP 1/6: GIVEN A Client Makes A Deposit [amount: "1000"]...

> STEP 2/6: AND A Client Makes A Withdrawal [amount: "100"]...

> STEP 3/6: AND A Client Makes A Deposit [amount: "500"]...

> STEP 4/6: WHEN A Client Prints A Statement...

> STEP 5/6: THEN They Should See Three Transactions In Reverse Order...

> STEP 6/6: AND They Should See A Formatted Statement...

FEATURE FINISHED: AccountFeature (Passed) 

Happy Days!
