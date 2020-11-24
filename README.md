# How-to-export-data-to-excel-using-Radzen-and-Blazor-.Net-Core-3.1
Creating and exporting data to Excel file is one of the frequently used feature in web apps. We will learn about how to export data to excel in Blazor Server + Radzen project in this post.

**Why I am writing this post?** Few days back i was stragling with radzon export feature. When we tried to export data to excel file, it exported successfully but then application got crash with an exception. When i investigated it in-deep, there was an exception in browser console "**Error: Circuit has been shut down due to error (Export)**" which is known issue of Radzen so far! Even i was unable to show proper spinner for export function or any other operations on page. I need to just reload/refresh page to see it again.

Radzen team is trying solve the issue, but what i should do to provide facility to our clients with radzen/blazor project in meanwhile. So I am writing a short article with the solution about exporting excel in Radzen/Blazor without any exception and export files as many as you would like to do staying on same page.
