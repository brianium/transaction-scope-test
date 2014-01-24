Using NHibernate with System.Transactions.TransactionScope
==========================================================
This repo demonstrates how System.Transactions.TransactionScope works with NHibernate transactions, particularly
transactions involving multiple databases.

Creating a new transaction scope creates an "ambient" transaction meaning *all* ADO operations on the same thread will 
participate in that same ambient transaction.

This repo contains a test suite demonstrating it's use and proving it works as expected with NHibernate transactions, showing NHibernate
transactions will automatically enlist in the ambient transaction.

Requirements
------------
A SQLExpress instance or equivalent - these tests were built on a Sql Server 2008 R2 express instance and the connection strings reflect that
A database called `ProductDatabase`
A database called `OrderDatabase`

Setting up databases
--------------------
After building the project run `bin\migrateup.bat` to create tables on both databases. You can run `bin\migratedown.bat` to remove the tables.


Running the tests
-----------------
The tests demonstrate saving on both databases using an ambient transaction created using `new System.Transactions.TransactionScope()`
All tests were written in a single `Test` class in the `TransactionScopeTest.SessionTest` project in the `tests` directory.

These tests should prove that `TransactionScope` can be used to acheive transactional behavior between multiple NHibernate sessions, each connected
to a different database.