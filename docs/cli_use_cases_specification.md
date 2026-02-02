# CLI Use Cases Specification (Draft)

> This document describes current CLI user use cases and interaction scenarios.
> Language, structure, and details may evolve as the application grows.

---

## UC-0. Show Available Commands

**Initiator:** User

**Preconditions:**
User is in main menu
System is running

**Intention:**
View the list of all commands available in the system.

**Success Guarantee:**
The user sees a list of supported commands with short descriptions.

**Trigger:**
User starts the application or enters an empty / help command.

**Main Scenario:**
1. System starts or receives a help request.
2. System displays a list of available commands.

**Extensions:**
– None.

---

## UC-1. List Books

**Initiator:** User

**Intention:**
View a paginated list of books stored in the system.

**Success Guarantee:**
A page of books is displayed to the user.

**Trigger:**
User enters the `list` command.

**Main Scenario:**
1. System requests the first page of books.
2. System displays up to N books (default page size).
3. System shows navigation hints (next / previous page).
4. System stores the displayed list in the session context.

**Extensions:**
1.1. There are more books than fit on one page.
  1.1.1. User requests the next page.
  1.1.2. System displays the next page and updates the session context.

1.2. There are no books in the system.
  1.2.1. System informs the user that the library is empty.

---

## UC-2. Add Book

**Initiator:** User

**Intention:**
Add a new book to the system.

**Success Guarantee:**
A new book is successfully added and persisted.

**Trigger:**
User selects the `add book` command.

**Main Scenario:**
1. System sequentially prompts the user for required book fields.
2. User provides all required data.
3. System validates the input.
4. System creates and saves the book.
5. System confirms successful addition.

**Extensions:**
2.1. User enters `quit` during input.
  2.1.1. System cancels the operation and returns to the main menu.

2.2. User omits a required field.
  2.2.1. System prompts again for the missing field.

2.3. A book with the same ISBN already exists.
  2.3.1. System reports a duplication error.
  2.3.2. System returns to the add book flow.

2.4. ISBN is invalid.
  2.4.1. System reports a validation error.
  2.4.2. System requests corrected input.

2.5. Author does not exist.
  2.5.1. System offers to add a new author.

---

## UC-3. Remove Book

**Initiator:** User

**Intention:**
Remove an existing book from the system.

**Success Guarantee:**
The selected book is removed.

**Trigger:**
User enters the `remove` command.

**Main Scenario:**
1. User has previously listed books.
2. System prompts the user for a book number from the displayed list.
3. User enters a display number.
4. System resolves the display number to a BookId using session context.
5. System removes the book by BookId.
6. System confirms successful removal.
7. System clears the session context.

**Extensions:**
3.1. User has not listed books.
  3.1.1. System asks the user to list books first.

3.2. Display number is out of range or invalid.
  3.2.1. System reports an error and aborts the operation.

3.3. Book not found (concurrent deletion or stale context).
  3.3.1. System reports that the book no longer exists.

---

## UC-4. Add Author

**Initiator:** User

**Intention:**
Add a new author to the system.

**Success Guarantee:**
A new author is successfully added.

**Trigger:**
User selects the `add author` command.

**Main Scenario:**
1. System prompts the user for required author fields.
2. User provides all required data.
3. System validates the input.
4. System creates and saves the author.
5. System confirms successful addition.

**Extensions:**
4.1. User enters `quit` during input.
  4.1.1. System cancels the operation and returns to the main menu.

4.2. Author with the same name already exists.
  4.2.1. System reports a duplication error.

---

## UC-5. Exit Application

**Initiator:** User

**Intention:**
Exit the application.

**Success Guarantee:**
The application terminates.

**Trigger:**
User enters the `quit` command.

**Main Scenario:**
1. System stops execution and exits.

**Extensions:**
– None.

