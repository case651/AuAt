# AuAt

Statement: Authentication is a special case of authorization.

Proof:
1. A system is a set of functions.
2. Authorization is the system's function that returns system's functions available to the user.
3. Authentication is the system's function that returns the authorization function (and some optional functions to get user/session information).

Conclusion - authentication is a special case of authorization, since it, like authorization, returns the system functions available to the user. That is authentication is a function of gaining access to authorization. That is authentication is an authorization for authorization.

The code here is an example of such system.
