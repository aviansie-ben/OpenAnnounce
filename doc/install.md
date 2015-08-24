## Installation Instructions

### SQL Database

#### SQL Server Setup

The supported configuration for OpenAnnounce uses Microsoft SQL Server 2014 Express as a database, although other databases may work with some tweaking.

Begin by installing Microsoft SQL Server 2014 Express (or later) and create a new Database in the instance. Ensure that the user account that you want OpenAnnounce to run as is allowed to read from and write to all tables in this database.

#### Table Creation

In order to set up the tables, execute the scripts in the `OpenAnnounce/SQL` folder in the following order on the new database:

* SecurityPrinciples.sql
* Scopes.sql
* SecurityPrincipleScopes.sql
* Users.sql
* NavbarLinks.sql
* Announcements.sql
* Clubs.sql

These files must be executed in this order, since the tables described in these files have foreign keys on each other, which may fail to create if the tables are created in the wrong order.

Finally, in the `web.config` file, set the `mainDb` connection string to connect to the new database, ensuring that `MultipleActiveResultSets` and `Integrated Security` are both set to true. See [this site](http://www.connectionstrings.com/) for assistance in creating a connection string.

#### Security Principle and Scope Setup

For now, the scopes and security principles need to be created manually in the database, as there is not currently a way to create these from the web interface.

A security principal is a group or user in active directory which can be granted permissions to perform certain actions through the web interface and can be attached to a scope. (more on scopes in a bit)

You will require a row in the `SecurityPrincipals` table for each user or group that you want to directly work with. Set the Domain to the NetBIOS name of the Active Directory domain, the PrincipalName to the login name of the user or the name of the group, and if the entry refers to a user, set the IsUser value to 1. Assign any permissions that you want to by setting their column value to 1. (see [doc/permissions.md](permissions.md) for information about the various permissions)

A scope defines the visibility of an item, determining which users it is shown to. A scope might be for example "Students Only" or "Staff Only". Note that an "Everybody" scope will always exist and will cover even those users who do not fall into any of the given security principals.

For each scope you will need, create a new row in the `Scopes` table with the desired name. Then, add a row in the `SecurityPrincipalScopes` table for every connection that you want to make between a security principal and a scope, using the ID of the scope and security principal to link them together.

You can assign as many security principals to a scope as you'd like and each security principal can fall into as many scopes as required. Note that only **one** scope can be selected for any item, so if you want more complicated control, you'll need to add the necessary scopes to do that.

### IIS Setup

_To be added later_
