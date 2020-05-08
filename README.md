<h3>My Store</h3>

(This project was built and tested in .NET Framework 4.6.1)

This is an example of an online store, where suppliers can sell products and customers can buy them.

This project has 3 parts:

<ul>
  <li><b>Console Application:</b> This is the CLI of the application. It handles all the menus, actions and the connection to the database. </li>
  <li><b>SQL Database</b> The database of the project. Includes stored procedures. </li>
  <li><b>Unit Test:</b> A Test class that verifies that the programs behaves as it should. </li>
</ul>

This is the database's tables diagram:

![storeDB](https://user-images.githubusercontent.com/45973605/81399720-2c8d3500-9134-11ea-9777-ac3fdbaad71d.png)

The database is included in this repository as an MDF file. You can find it in the root directory.

<b>Customers:</b> To be able to buy products, the user will need to create a new customer account. After logging in, he can select any product of his choice from the menu.
The registered customer can also look at all his previous orders.

<b>Suppliers:</b> The supplier will also need to create an account, and then he will be able to add his products to the store and update their quantity any time.

There is also an administrator account that can access the logs table (The program logs every action to this table).

I Hope you will find this repository valuable for you.

