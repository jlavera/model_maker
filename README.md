# model_maker

This is an application that I made to autogenerate the Business Objects and the Data Access Objects given a MSSQL DB.

You need to point it to an address, specify the name of the database, username and password and it will query the system tables to infer the schema and then create the needed classes with the mapping (only supports a subset of data types, but it shouldn't be hard to accept more :) ).

No more creating connections, opening them, creating the query, replacing parameters manually and mapping the results every time. The only thing you need to execute query is:

    List<User> users = DB.ExecuteReader<User>("SELECT * FROM users WHERE id = @1", 152);
    
and you are good to go :D.

![Fisgón morbosón] (http://i.imgur.com/0BZRDgX.png)

Next to come, query builder.

PS: Oh, I forgot, it also provides lazy load for associated entities, that way you don't need to explicitly query to get them.
