# AuAt
Check that authentication is a special case of authorization

1. A system is a set of functions.
2. Authorization is the system's function that returns available functions of a system.
3. Authentication is the function that returns the authorization function (and some optional functions to get user information).

Conclusion - authentication is a special case of authorization, because it returns the system function available to the user.
The code here is an example of such system.
