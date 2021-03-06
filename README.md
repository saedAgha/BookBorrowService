# BookBorrowService
Service should allow given user id and librarian id to borrow an existing book or return it

# System Requirements 

Design a system that have :
-	Library (Support only single
-	Book ( can have multiple author’s , multiple copies)
-	Author (can have multiple books)
-	Librarian
-	Customer

1.	Borrow Book : given book id, librarian id and customer id
Need to check if user can borrow a book ,and register request .

Possible edge cases :
-	Check if book id ,customer id , librarian id exist in DB 
-	If user borrow same book in past and didn’t returned it , he can’t take another one .
-	Can take a book as long as it have number of copies greater than zero.
Input : book id , librarian id , user id 
Output : defined true/false at the interview ( but would consider returning book title , number of copies left,list of authers)

2.	Return Book : given book id , librarian id ad customer id 
Need return the book.

Possible edge cases :
-	Check if book id ,customer id , librarian id exist in DB 
-	Check if customer indeed borrowed the book in the past and didn’t returned it 



# Entities
![image](https://user-images.githubusercontent.com/18490274/110212879-2a0b8100-7ea6-11eb-9bee-d73cec232f46.png)

# Class UML 
![image](https://user-images.githubusercontent.com/18490274/110213679-b4a1af80-7ea9-11eb-840f-272e32abba89.png)


# Swagger
![image](https://user-images.githubusercontent.com/18490274/110214767-c6d21c80-7eae-11eb-91bb-9a66a79cca58.png)
![image](https://user-images.githubusercontent.com/18490274/110214780-dea9a080-7eae-11eb-85f8-0e6cb42f41a3.png)


# Unit tests
Xunit unit test project created to verify unit cases for services,validator and repositiory

# Integration Test 
xunit integration tests applied to cover e2e flows

# Notes
-  entites fields I would prefer to encapsulate with private setters
   But for the sake of this homework i choosed not to implement to keep simplicity
   (In bigger development it should be encapsulated more offcourse)
  
 - in entites diagram you may notice that library contain
   Users and Books, in code i intentionally didnt added collection
   For both Users and Books because it wasn't a demand on requirements
   But i kept to be clear.

-  Library was defined as being single one , and for system requirements it
   was clear there's only one .

