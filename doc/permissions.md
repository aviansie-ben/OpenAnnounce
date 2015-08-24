## Permissions

### General Permissions

* `CanAccessBackend` - Allows the user to access the admin panel to make changes. Most other permissions will not work if the user has not been granted this permission.
* `CanEditNavbar` - Allows the user to add or remove navbar links. There is no approval mechanism for making modifications to the navbar, so changes will take effect immediately.
* `CanEditProfiles` - Allows the user to make modifications to other users' profiles. This allows them to change other users' display names which are used by the club display.
* `CanSetPermissions` - Allows the user to make modifications to security principals and scopes. This effectively grants the user all permissions, since they can simply give themselves any permission they wish.
  * Note that this functionality is not yet implemented, so this permission currently does nothing.

### Announcements

* `CanSubmitAnnouncement` - Allows the user to submit new announcements for approval.
* `CanApproveAnnouncement` - Allows the user to approve/deny submitted announcements. Users with this permission can also make changes to already approved announcements without having them revert to unapproved.
* `CanAdvancedEditAnnouncement` - Allows the user to make advanced modifications to an announcement. This includes the ability to modify the importance of the announcement on a scale from 0 to 10 to affect display order.
* `CanViewAllAnnouncement` - Allows the user to view all announcements, even those that they did not submit.
* `CanEditAllAnnouncement` - Allows the user to make modifications to any announcement, including those that were submitted by other users.
  * This does not permit users to subvert approval for their changes. If this user cannot approve changes, any announcement they edit will be unapproved and will need to be approved again.
* `CanHardDeleteAnnouncement` - Allows the user to "hard delete" an announcement, completely removing it from the database and preventing recovery.

### Clubs

* `CanSubmitClub` - Allows the user to submit clubs for approval.
* `CanApproveClub` - Allows the user to approve/deny submitted clubs. Users with this permission can also make changes to already approved clubs without having them revert to unapproved.
* `CanViewAllClub` - Allows the user to view all clubs, even those that they did not submit.
* `CanEditAllClub` - Allows the user to make modifications to any club, including those that were submitted by other users.
  * As with the `CanViewAllAnnouncement` permission, this does not allow the user to subvert approval requirements.
* `CanHardDeleteClub` - Allows the user to "hard delete" a club, completely removing it from the database and preventing recovery.
