# Issues

## Google service

### Due date

Problem:
When the due date is set on Googles web interface it does not transfere with
the public API. The `.Due` property is `null`.
This is due to a change in 2019 to add time and not just date and the public API
was not changed.

All google searches talks about missing time porsion which in this context is not
interesting.

More research:
Try to add a date to a task using the API.
Check if it's set on return.
Check if it's set on Google Tasks.

Solution:
Treat `null` as not having a date, there really is no other options.
