# BasicSecurity
Basic Security project

Generating a user will result in his/her passwords being hashed and salted. We will store these in the database over https connection.
A user will also recieve a private and public key, who'm are also encoded into byte arrays thus safely stored and unable to be taken from our database.
If the user is logged in we will store a token in our database. This works for sso.
