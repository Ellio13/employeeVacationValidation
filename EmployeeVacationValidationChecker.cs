
//lastname, firstname - validate
//    hire date - validate
//    number of days off - they get 14 days off if they have worked at least one full year (no carry over)
//    they get 21 days off if they have worked more than five years.
//Create a Vacation Approval app...

//Ask their name. Must enter as "lastname, firstname".   Validate loop
//Ask their hire date.  Validate loop
//Ask for the number of days they want off. Validate loop

//Validate each request and loop until they get it right.

//They get 14 days a year if they have worked at least a full year for your company. (no carry over!)

//Bonus...

////They get 21 days a year if that have worked more than five years.

//This is my Employee Time Off Validation Checker



Console.WriteLine("Welcome to the Employee Time Off Validation Checker.");

string employeeInput;
DateTime hireDate;
int daysRequested;
DateTime leaveWorkDate;
DateTime returnWorkDate;
TimeSpan hireDateToLeaveWorkDate;
TimeSpan timeWorked;
int eligibility;

while (true)

{
    while (true) //collect employee name
    {
        Console.WriteLine("Enter your name (format: LastName, FirstName):");
        employeeInput = Console.ReadLine()?.Trim(); // Read input and remove extra spaces

        // Check if input contains a comma and is properly formatted
        if (!string.IsNullOrWhiteSpace(employeeInput) && employeeInput.Contains(", "))
        {
            Console.WriteLine($"You entered {employeeInput}");
            break; // Exit loop if format is correct
        }
        else
        {
            Console.WriteLine("Invalid format! Please use 'LastName, FirstName.'");
        }
    }



    //collect date of hire
    while (true)
    {
        Console.WriteLine("What is your date of hire (yyyy-mm-dd)?");
        string dateInput = Console.ReadLine()?.Trim(); // Read input and remove extra spaces

        // Try to parse the date
        if (DateTime.TryParse(dateInput, out hireDate))
        {
            Console.WriteLine($"You entered: {hireDate}");
            break;
        }
        else
        {
            Console.WriteLine("Invalid date format! Please use 'yyyy-mm-dd'.");
        }
        }


        //vacation approval based on years employed - this one is important
        int yearsEmployed = DateTime.Today.Year - hireDate.Year;

        if (DateTime.Today < hireDate.AddYears(yearsEmployed))
        {
            yearsEmployed--;
        }
        if (yearsEmployed >= 1)
            Console.WriteLine($"You have worked {yearsEmployed} full years and are approved.");
        else
        {
            Console.WriteLine($"You have worked {yearsEmployed} full years and are not approved.");
            return;
        }


        while (true) // beginning date of vacation
        {
            Console.WriteLine("What is your first day out of the office?");
            string? dateInput2 = Console.ReadLine()?.Trim();

            if (DateTime.TryParse(dateInput2, out leaveWorkDate))
            {
                timeWorked = leaveWorkDate - hireDate;
                eligibility = timeWorked.Days;

                if (leaveWorkDate < DateTime.Now)
                {
                    Console.WriteLine("Please enter a valid date.");
                    continue;
                }

                else if (eligibility < 365)
                {
                    Console.WriteLine("You are not eligible for vacation.");
                    return;
                }
                else
                {
                    Console.WriteLine("You are eligible for vacation.");
                }

                break;
            }
            else
            {
                Console.WriteLine("Invalid date format! Please use 'yyyy-mm-dd'.");
            }
        }


        // requested number of days off
        while (true)
        {
            Console.WriteLine("Enter number of requested days.");
            string enteredDays = Console.ReadLine()?.Trim();

            if (!int.TryParse(enteredDays, out daysRequested))

            {
                Console.WriteLine("Invalid input.  Please enter a valid number of full days.");
            }

            else if (daysRequested <= 14 && (eligibility >= 365 && eligibility < 1825))
            {
                Console.WriteLine($"Your request for {daysRequested} has been approved!");
                break;
            }

            else if (daysRequested <= 21 && eligibility >= 1825)
            {
                Console.WriteLine($"Your request for {daysRequested} days off has been approved");
                break;
            }
            else Console.WriteLine("Your request has been denied.");
            break;
        }
    }



